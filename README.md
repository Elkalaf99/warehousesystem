# Warehouse Management System

A Windows desktop application for managing warehouse inventory and transactions.

## Prerequisites

- Windows 10 or later
- .NET 8.0 Runtime
- Microsoft SQL Server 2022 Express or later
- Visual Studio 2022 (for development)

## Installation

1. Download and install the latest .NET 8.0 Runtime from [Microsoft's website](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Install Microsoft SQL Server 2022 Express from [Microsoft's website](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
3. Run the WarehouseManagementSystem.msi installer
4. On first launch, the application will guide you through database setup

## Database Setup

The application includes two options for database setup:

1. **Automatic Setup (Recommended)**

   - The installer will automatically create the database and required tables
   - Sample data will be included

2. **Manual Setup**
   - Run the `CreateWarehouseDb.sql` script in SQL Server Management Studio
   - Update the connection string in the application settings

## Features

- Product Management (Add/Edit/Delete)
- Transaction Recording (Inbound/Outbound)
- Inventory Reports
- PDF Export
- Data Validation
- User-friendly Interface

## Configuration

The application stores its connection string in `app.config`. You can modify these settings through the application's Settings dialog.


