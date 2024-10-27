using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.UOW;
using SchedulingReportingService.Services;

namespace SchedulingReportingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleController(IBackgroundJobClient backgroundJobClient, IUnitOfWork unitOfWork)
        {
            _backgroundJobClient = backgroundJobClient;
            _unitOfWork = unitOfWork;
        }
        #region Get All ScheduledTasks
        [HttpGet("GetAll")]
        public IActionResult GetAllScheduledTasks()
        {
            var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            var scheduledTasks = new List<ScheduledTasks>();

            foreach (var job in recurringJobs)
            {
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (job.Job.Arguments.Count() > 0 && DateTime.TryParse(job.Job.Arguments[0]?.ToString(), out var parsedStartDate))
                {
                    startDate = parsedStartDate;
                }

                if (job.Job.Arguments.Count() > 1 && DateTime.TryParse(job.Job.Arguments[1]?.ToString(), out var parsedEndDate))
                {
                    endDate = parsedEndDate;
                }

                var scheduledTask = new ScheduledTasks
                {
                    TaskType = job.Job.Method.Name,
                    CronExpression = job.Cron,
                    StartDate = startDate,
                    EndDate = endDate
                };

                scheduledTasks.Add(scheduledTask);
            }

            return Ok(scheduledTasks);
        }
        #endregion

        #region one time job
        [HttpPost("manual")]
        public IActionResult ManualJob()
        {
            BackgroundJob.Enqueue<ReportService>(x => x.GenerateAndExportReportAsync(DateTime.Now, DateTime.Now, "LocalHost", "http://localhost:5212/", "ApiKey"));
            return Ok("Job manually enqueued.");
        }
        #endregion

        #region Create Scheduled Task
        [HttpPost]
        public async Task<IActionResult> ScheduleTask([FromBody] ScheduledTasks task)
        {
            try
            {
                // Generate the cron expression
                string cronExpression = GetCronExpression(task.CronExpression);
                if (cronExpression == null)
                {
                    return BadRequest("Invalid Task Type. Please provide Daily, Weekly, or Monthly.");
                }

                // Save the task to ScheduledTasks
                await _unitOfWork.ScheduledTasks.AddAsync(task);

                Console.WriteLine($"Scheduling job with ID: GenerateReport_{task.ScheduledTaskId} using Cron: {cronExpression}");

                // Schedule the report generation
                RecurringJob.AddOrUpdate<ReportService>(
                    "GenerateReport_" + task.ScheduledTaskId,
                    x => x.GenerateAndExportReportAsync(task.StartDate ?? DateTime.MinValue, task.EndDate ?? DateTime.MaxValue, task.Source, task.Url, task.ApiKey),
                    cronExpression
                );

                task.ScheduledTime = DateTime.Now;
                await _unitOfWork.SaveAsync();

                return Ok("Scheduled successfully and saved to the database!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scheduling task: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        private string GetCronExpression(string cronExpression)
        {
            return cronExpression.ToLower() switch
            {
                "daily" => "0 0 * * *",         // Every day at midnight
                "weekly" => "0 0 * * 1",         // Every Monday at midnight
                "monthly" => "0 0 1 * *",        // First day of every month at midnight
                _ => null
            };
        }
        #endregion

        #region Update Scheduled Task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScheduleAsync(int id, [FromBody] ScheduledTasks updatedTask)
        {
            // First, ensure the task exists in the database before updating
            var existingTask = await _unitOfWork.ScheduledTasks.GetByIdAsync(id); // Assuming you have a method to get the task by ID
            if (existingTask == null)
            {
                return NotFound("Scheduled task not found.");
            }

            // Remove the existing Hangfire job if it exists
            RecurringJob.RemoveIfExists($"task-{existingTask.ScheduledTaskId}");

            // Update the Hangfire job with new cron expression and parameters
            RecurringJob.AddOrUpdate<ReportService>(
                $"task-{existingTask.ScheduledTaskId}",
                x => x.GenerateAndExportReportAsync(updatedTask.StartDate, updatedTask.EndDate, updatedTask.Source, updatedTask.Url, updatedTask.ApiKey),
                updatedTask.CronExpression
            );

            return Ok("Scheduled task updated successfully!");
        }
        #endregion

        #region Delete Scheduled Task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduleAsync(int id)
        {
            // First, check if the task exists in the database
            var existingTask = await _unitOfWork.ScheduledTasks.GetByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound("Scheduled task not found.");
            }

            // Remove the corresponding Hangfire job
            RecurringJob.RemoveIfExists($"task-{existingTask.ScheduledTaskId}");

            // Delete the task from the database
            await _unitOfWork.ScheduledTasks.DeleteAsync(existingTask.ScheduledTaskId);
            await _unitOfWork.SaveAsync();

            return Ok("Scheduled task deleted.");
        }
        #endregion
    }
}
