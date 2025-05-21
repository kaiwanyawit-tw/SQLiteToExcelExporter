using SQLiteToExcelExporter.Data;
using SQLiteToExcelExporter.Migrations;
using SQLiteToExcelExporter.Services;

namespace SQLiteToExcelExporter
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("SQLite to Excel Exporter");
            Console.WriteLine("=========================");

            try
            {
                // Ensure database and apply migrations
                Console.WriteLine("\nSetting up database...");
                DatabaseMigrator.EnsureDatabaseCreated();
                DatabaseMigrator.ApplySeeding();

                // Create context
                using var context = new AppDbContext();

                // Export tables to Excel
                var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SQLiteExport");
                Console.WriteLine($"\nExport will be saved to: {outputPath}");

                var exportService = new ExcelExportService(context);
                await exportService.ExportAllTablesToExcel(outputPath);

                Console.WriteLine("\nExport completed successfully!");
                Console.WriteLine($"Files are located at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}