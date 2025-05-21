using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SQLiteToExcelExporter.Data;

namespace SQLiteToExcelExporter.Migrations
{
    public class DatabaseMigrator
    {
        public static void EnsureDatabaseCreated()
        {
            using var context = new AppDbContext();
            
            Console.WriteLine($"Database path: {context.DbPath}");
            
            // Check if database exists and create it if it doesn't
            context.Database.EnsureCreated();
            
            Console.WriteLine("Database has been created if it didn't exist.");
            
            // Check if any pending migrations
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"Found {pendingMigrations.Count} pending migrations. Applying...");
                context.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
        }
        
        public static void ApplySeeding()
        {
            using var context = new AppDbContext();
            
            // Check if database has any data
            if (!context.Categories.Any())
            {
                Console.WriteLine("Database is empty. Applying seed data...");
                
                // Use the migrator service to apply seeding if needed
                var migrator = context.GetService<IMigrator>();
                migrator.Migrate("Initial");
                
                Console.WriteLine("Seed data applied successfully.");
            }
            else
            {
                Console.WriteLine("Database already has data. Skipping seeding.");
            }
        }
    }
}