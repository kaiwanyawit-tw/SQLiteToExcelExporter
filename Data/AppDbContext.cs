using Microsoft.EntityFrameworkCore;
using SQLiteToExcelExporter.Models;

namespace SQLiteToExcelExporter.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        public string DbPath { get; }

        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "sqlite_export.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items" },
                new Category { Id = 3, Name = "Home", Description = "Home goods and furniture" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { 
                    Id = 1, 
                    Name = "Smartphone", 
                    Description = "Latest smartphone with advanced features", 
                    Price = 799.99m, 
                    CategoryId = 1, 
                    CreatedAt = DateTime.Now.AddDays(-30) 
                },
                new Product { 
                    Id = 2, 
                    Name = "Laptop", 
                    Description = "High-performance laptop for professionals", 
                    Price = 1299.99m, 
                    CategoryId = 1, 
                    CreatedAt = DateTime.Now.AddDays(-25) 
                },
                new Product { 
                    Id = 3, 
                    Name = "T-Shirt", 
                    Description = "Cotton t-shirt with logo", 
                    Price = 19.99m, 
                    CategoryId = 2, 
                    CreatedAt = DateTime.Now.AddDays(-20) 
                },
                new Product { 
                    Id = 4, 
                    Name = "Jeans", 
                    Description = "Durable denim jeans", 
                    Price = 49.99m, 
                    CategoryId = 2, 
                    CreatedAt = DateTime.Now.AddDays(-15) 
                },
                new Product { 
                    Id = 5, 
                    Name = "Sofa", 
                    Description = "Comfortable living room sofa", 
                    Price = 599.99m, 
                    CategoryId = 3, 
                    CreatedAt = DateTime.Now.AddDays(-10) 
                }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Phone = "555-123-4567" },
                new Customer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Phone = "555-987-6543" }
            );

            // Seed Orders
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-5), TotalAmount = 1099.98m },
                new Order { Id = 2, CustomerId = 2, OrderDate = DateTime.Now.AddDays(-3), TotalAmount = 649.98m }
            );

            // Seed OrderItems
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 799.99m },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 3, Quantity = 3, UnitPrice = 19.99m },
                new OrderItem { Id = 3, OrderId = 2, ProductId = 5, Quantity = 1, UnitPrice = 599.99m },
                new OrderItem { Id = 4, OrderId = 2, ProductId = 4, Quantity = 1, UnitPrice = 49.99m }
            );
        }
    }
}