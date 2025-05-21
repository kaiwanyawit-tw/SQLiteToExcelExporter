# Adding New Column with Migration
## 1. Add Property to Model
```C#
public class Product
{
    // ...existing code...
    public bool IsDiscontinued { get; set; }
}
```
## 2. Create New Migration
Run in terminal:
```bash
dotnet ef migrations add AddIsDiscontinuedToProducts
```
## 3. Migration Class
```C#
using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLiteToExcelExporter.Migrations
{
    public partial class AddIsDiscontinuedToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDiscontinued",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            // Update existing records if needed
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDiscontinued",
                value: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDiscontinued",
                table: "Products");
        }
    }
}
```
## 4. Update Seed Data in AppDbContext
```C#
private void SeedData(ModelBuilder modelBuilder)
{
    // ...existing code...
    modelBuilder.Entity<Product>().HasData(
        new Product {
            Id = 1,
            Name = "Smartphone",
            // ...existing properties...
            IsDiscontinued = false
        },
        // Update other product seed data
    );
}
```
## 5. Apply Migration
Run in terminal:
```bash
dotnet ef database update
```
## Summary
This will:

Add a new boolean column IsDiscontinued to the Products table
Create a migration with up/down methods
Set default value for existing records
Update seed data to include the new column
The new column will be included in future Excel exports automatically since we're using reflection to get properties in the ExcelExportService.