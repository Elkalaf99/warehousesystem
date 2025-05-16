# Warehouse Management System

A Windows desktop application for managing warehouse inventory, built with C# Windows Forms and Entity Framework Core.

## Features

- **Product Management**: Add, edit, and delete products.
- **Transaction Tracking**: Record incoming and outgoing transactions.
- **Reporting**: Generate reports on inventory and transactions.
- **Settings**: Configure application settings.

## Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- Visual Studio 2022 or later (recommended)

## Building the Project

1. Clone the repository:

   ```sh
   git clone https://github.com/yourusername/WarehouseManagementSystem.git
   cd WarehouseManagementSystem
   ```

2. Build the project:

   ```sh
   dotnet build
   ```

3. Run the application:
   ```sh
   dotnet run
   ```

## Publishing the Application

To create a self-contained executable:

```sh
dotnet publish -c Release -r win-x64 --self-contained true
```

The executable and all required files will be generated in:

```
bin\Release\net7.0-windows\win-x64\publish\
```

## Deployment

- Copy the entire `publish` folder to the client machine.
- Run `WarehouseManagementSystem.exe` directly—no additional installation required.
