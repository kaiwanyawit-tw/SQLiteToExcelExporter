using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SQLiteToExcelExporter.Data;
using System.Reflection;

namespace SQLiteToExcelExporter.Services
{
    public class ExcelExportService
    {
        private readonly AppDbContext _context;

        public ExcelExportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> ExportTableToExcel<T>(string tableName, string outputPath) where T : class
        {
            try
            {
                Console.WriteLine($"Exporting table {tableName} to Excel...");

                // Create a new workbook
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(tableName);

                // Get the data
                var data = await _context.Set<T>().ToListAsync();
                if (data.Count == 0)
                {
                    Console.WriteLine($"No data found in table {tableName}.");
                    return string.Empty;
                }

                // Create header row
                IRow headerRow = sheet.CreateRow(0);
                var properties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType.IsSimpleType())
                    .ToList();

                for (int i = 0; i < properties.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    var headerStyle = workbook.CreateCellStyle();
                    var headerFont = workbook.CreateFont();
                    headerFont.IsBold = true;
                    headerStyle.SetFont(headerFont);
                    cell.CellStyle = headerStyle;
                    cell.SetCellValue(properties[i].Name);
                }

                // Create data rows
                int rowIndex = 1;
                foreach (var item in data)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    
                    for (int i = 0; i < properties.Count; i++)
                    {
                        var cell = dataRow.CreateCell(i);
                        var value = properties[i].GetValue(item);
                        
                        if (value == null)
                        {
                            cell.SetCellValue(string.Empty);
                        }
                        else if (value is DateTime dateTime)
                        {
                            cell.SetCellValue(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else if (value is decimal decimalValue)
                        {
                            cell.SetCellValue((double)decimalValue);
                            
                            // Apply currency format for price columns
                            if (properties[i].Name.Contains("Price") || properties[i].Name.Contains("Amount"))
                            {
                                var style = workbook.CreateCellStyle();
                                var format = workbook.CreateDataFormat();
                                style.DataFormat = format.GetFormat("$#,##0.00");
                                cell.CellStyle = style;
                            }
                        }
                        else if (value is double doubleValue)
                        {
                            cell.SetCellValue(doubleValue);
                        }
                        else if (value is int intValue)
                        {
                            cell.SetCellValue(intValue);
                        }
                        else if (value is bool boolValue)
                        {
                            cell.SetCellValue(boolValue);
                        }
                        else
                        {
                            cell.SetCellValue(value.ToString());
                        }
                    }
                    
                    rowIndex++;
                }

                // Auto-size columns
                for (int i = 0; i < properties.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                // Save the workbook
                string filePath = Path.Combine(outputPath, $"{tableName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                workbook.Write(fs);

                Console.WriteLine($"Excel file created successfully: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting table {tableName} to Excel: {ex.Message}");
                throw;
            }
        }

        public async Task ExportAllTablesToExcel(string outputPath)
        {
            try
            {
                Console.WriteLine("Starting export of all tables...");

                // Create output directory if it doesn't exist
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                    Console.WriteLine($"Created output directory: {outputPath}");
                }

                // Export each table
                await ExportTableToExcel<Models.Product>("Products", outputPath);
                await ExportTableToExcel<Models.Category>("Categories", outputPath);
                await ExportTableToExcel<Models.Customer>("Customers", outputPath);
                await ExportTableToExcel<Models.Order>("Orders", outputPath);
                await ExportTableToExcel<Models.OrderItem>("OrderItems", outputPath);

                Console.WriteLine("All tables exported successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting all tables: {ex.Message}");
                throw;
            }
        }
    }

    // Extension method to check if a type is a simple type (not a complex object)
    public static class TypeExtensions
    {
        public static bool IsSimpleType(this Type type)
        {
            return
                type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(string) ||
                type == typeof(decimal) ||
                type == typeof(DateTime) ||
                type == typeof(DateTimeOffset) ||
                type == typeof(TimeSpan) ||
                type == typeof(Guid) ||
                Nullable.GetUnderlyingType(type) != null && IsSimpleType(Nullable.GetUnderlyingType(type)!);
        }
    }
}