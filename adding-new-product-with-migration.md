# Adding New Product with Migration

## 1. Create New Migration

Run in terminal:
```bash
dotnet ef migrations add AddNewProduct
```
## 2. Create Migration Class
```C#
// filepath: /Migrations/[timestamp]_AddNewProduct.cs
using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLiteToExcelExporter.Migrations
{
    public partial class AddNewProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Description", "Price", "CategoryId", "CreatedAt" },
                values: new object[] {
                    6,
                    "Gaming Chair",
                    "Ergonomic gaming chair with lumbar support",
                    299.99m,
                    3, // Home category
                    DateTime.Now.AddDays(-5)
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
```
## 3. Apply Migration
Run in terminal:
```bash
dotnet ef database update
```

## 4. Update AppDbContext Seed Data (Optional)
```C#
// filepath: /Data/AppDbContext.cs
// In the SeedData method, add the new product to the Products seed data:
modelBuilder.Entity<Product>().HasData(
    // ... existing products ...
    new Product {
        Id = 6,
        Name = "Gaming Chair",
        Description = "Ergonomic gaming chair with lumbar support",
        Price = 299.99m,
        CategoryId = 3,
        CreatedAt = DateTime.Now.AddDays(-5)
    }
);
```

## Summary
This will:

1. Create a new migration to add the product
2. Apply the migration to the database
3. Keep the seed data in sync with the database state

The new product will be added to the "Home" category (CategoryId = 3) and will be included in future Excel exports. EOF