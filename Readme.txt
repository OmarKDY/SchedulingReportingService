# Scheduling Reporting Service

## Overview
The Scheduling Reporting Service is an ASP.NET Core 8 application designed to generate reports based on scheduled tasks using Hangfire. It allows users to schedule tasks such as daily, weekly, or monthly reports and maintains a history of generated reports in the database.

## Features
- **Schedule Reports**: Set up reports to be generated daily, weekly, or monthly using cron expressions.
- **Export Reports**: Export generated reports in CSV format.
- **Report History**: Save a history of all generated reports for future reference.
- **CRUD Operations**: Manage scheduled tasks with create, read, update, and delete functionalities.

## Technologies Used
- ASP.NET Core 8
- Hangfire
- Entity Framework Core
- SQL Server

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/) (for API testing)

### Cloning the Repository

To clone the repository, run the following command in your terminal or command prompt:

```bash
git clone https://github.com/OmarKDY/SchedulingReportingService.git

##Note
Postman Collection Attached
SchedulingReportingService.postman_collection