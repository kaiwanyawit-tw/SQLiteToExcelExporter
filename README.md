# SQLite to Excel Exporter

A C# application that exports SQLite database tables to Excel files using NPOI and Entity Framework Core migrations.

## Features

- SQLite database integration with Entity Framework Core
- Migration support for database schema evolution
- Seeding mechanism to populate database with sample data
- Excel export functionality using NPOI library
- Proper formatting of Excel files (headers, data types, styling)
- Export all tables or specific tables

## Project Structure

- **Models/** - Entity models (Product, Category, Customer, Order, OrderItem)
- **Data/** - Database context and configuration
- **Migrations/** - Database migration tool
- **Services/** - Excel export service 
- **Program.cs** - Main application entry point

## Requirements

- .NET 8.0 or higher
- Visual Studio 2022 or other compatible IDE

## Dependencies

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Sqlite
- NPOI

## How to Use

1. Clone the repository
2. Restore NuGet packages
3. Build the solution
4. Run the application

The application will:
1. Create a SQLite database (if not exists)
2. Apply any pending migrations
3. Seed the database with sample data (if empty)
4. Export all tables to Excel files
5. Save the Excel files to the Desktop/SQLiteExport folder

## Customization

- Modify the `AppDbContext.cs` file to change database schema
- Update seeding data in the `SeedData` method
- Add new entity models in the `Models` folder
- Customize Excel export formatting in the `ExcelExportService.cs` file

## License

MIT
