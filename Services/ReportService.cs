using SchedulingReportingService.Domain.Dtos;
using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.UOW;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SchedulingReportingService.Services
{
    public class ReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;

        public ReportService(IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
        }

        // Generates and exports a report
        public async Task GenerateAndExportReportAsync(DateTime? startDate, DateTime? endDate, string source = null, string url = null, string apiKey = null)
        {
            Console.WriteLine("Job is executing...");
            var report = await CreateReportAsync(startDate, endDate, source, url, apiKey);
            Console.WriteLine($"Report generated: Total Sales: {report.TotalSales}, New Users: {report.NewUsers}");

            await ExportReportAsync(report);
        }

        // Creates the report, either from the database or external source
        public async Task<ReportDto> CreateReportAsync(DateTime? startDate, DateTime? endDate, string source = null, string url = null, string apiKey = null)
        {
            ReportDto reportDto;

            if (string.Equals(source, "LocalHost", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(url, "http://localhost:5212/", StringComparison.OrdinalIgnoreCase))
            {
                // Use database data
                var totalSales = await _unitOfWork.Sales.GetTotalSalesAsync(startDate, endDate);
                var newUsers = await _unitOfWork.Users.GetNewUsersCountAsync(startDate, endDate);
                var orderStats = await _unitOfWork.Orders.GetOrderStatsAsync(startDate, endDate);

                reportDto = new ReportDto
                {
                    TotalSales = totalSales,
                    NewUsers = newUsers,
                    Orders = new OrderStatsDto
                    {
                        Placed = orderStats.Placed,
                        Shipped = orderStats.Shipped,
                        Delivered = orderStats.Delivered
                    }
                };
            }
            else
            {
                // Ensure the URL is valid
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    throw new ArgumentException("The provided URL is not valid or not an absolute URI.", nameof(url));
                }

                try
                {
                    // Use external API data if source, URL, and API key are provided
                    var requestUri = $"{url}?startDate={startDate:O}&endDate={endDate:O}&apiKey={apiKey}";
                    Console.WriteLine($"Requesting report from URL: {requestUri}");

                    var response = await _httpClient.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response from API: {jsonResponse}");

                    reportDto = JsonSerializer.Deserialize<ReportDto>(jsonResponse)
                                ?? throw new InvalidOperationException("Failed to deserialize the response into ReportDto.");
                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
                    throw new Exception("Error while fetching the report from the external source.", httpEx);
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"JSON Parsing Error: {jsonEx.Message}");
                    throw new Exception("Error while parsing the report response.", jsonEx);
                }
            }

            // Export the report to file storage
            await ExportReportAsync(reportDto);

            // Save the report to the database history
            await SaveReportHistoryAsync(reportDto);

            return reportDto;
        }

        // Exports the report to file storage
        public async Task ExportReportAsync(ReportDto report)
        {
            var csvContent = GenerateCsvContent(report);
            var fileName = $"report_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            var fileData = System.Text.Encoding.UTF8.GetBytes(csvContent);

            // Save the file locally
            await SaveToFileStorageAsync(fileData, fileName);
        }

        private string GenerateCsvContent(ReportDto report)
        {
            var csvBuilder = new StringBuilder();

            // Add the headers
            csvBuilder.AppendLine("Total Sales, New Users, Total Orders, Orders Placed, Orders Shipped, Orders Delivered");

            var placed = report.Orders?.Placed ?? 0;
            var shipped = report.Orders?.Shipped ?? 0;
            var delivered = report.Orders?.Delivered ?? 0;
            var totalOrders = placed + shipped + delivered;

            csvBuilder.AppendLine($"{report.TotalSales}, {report.NewUsers}, {totalOrders}, {placed}, {shipped}, {delivered}");

            return csvBuilder.ToString();
        }


        private async Task SaveToFileStorageAsync(byte[] fileData, string fileName)
        {
            string localFolderPath = Path.Combine(Directory.GetCurrentDirectory(),"exports", "ScheduledReports");

            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }

            string filePath = Path.Combine(localFolderPath, fileName);
            await File.WriteAllBytesAsync(filePath, fileData);

            Console.WriteLine($"File saved successfully at: {filePath}");
        }

        // Save the report history in the database
        public async Task SaveReportHistoryAsync(ReportDto report)
        {
            var orderIds = (report.Orders as IEnumerable<OrderDto>)?.Select(o => o.OrderId.ToString()) ?? Array.Empty<string>();

            var reportHistory = new ReportHistory
            {
                GeneratedDate = DateTime.UtcNow,
                TotalSales = report.TotalSales,
                NewUsers = report.NewUsers,
                OrderIds = string.Join(";", orderIds)
            };

            await _unitOfWork.ReportHistory.AddAsync(reportHistory);
            await _unitOfWork.SaveAsync();
        }
    }
    }
