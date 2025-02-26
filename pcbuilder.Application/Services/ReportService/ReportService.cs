using System.Drawing;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using pcbuilder.Application.DTOs.Builds;

namespace pcbuilder.Application.Services.ReportService;

public class ReportService : IReportService
{
    public async Task<byte[]> GenerateBuildExcelReport(GenerateBuildReportDto build)
    {
        ExcelPackage.License.SetNonCommercialPersonal("MaxLeontev");
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Конфигурация ПК");
        
        var headerStyle = worksheet.Cells["A1:B3"].Style;
        headerStyle.Font.Bold = true;
        headerStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerStyle.Fill.PatternType = ExcelFillStyle.Solid;
        headerStyle.Fill.BackgroundColor.SetColor(Color.LightGray);

        worksheet.Cells["A1:B1"].Merge = true;
        worksheet.Cells["A1"].Value = $"Конфигурация ПК \"{build.Name}\"";

        worksheet.Cells["A2:B2"].Merge = true;
        worksheet.Cells["A2"].Value = build.Description ?? "Описание отсутствует";

        worksheet.Cells["A3"].Value = "Категория";
        worksheet.Cells["B3"].Value = "Наименование";
        
        var row = 4;

        AddRow("Процессор", build.Components.Cpu?.FullName);
        AddRow("Материнская плата", build.Components.Motherboard?.FullName);
        AddRow("Система охлаждения", build.Components.Cooler?.FullName);
        
        if (build.Components.Rams?.Count > 0)
        {
            foreach (var ram in build.Components.Rams)
                AddRow("Оперативная память", ram.FullName);
        }
        else
        {
            AddRow("Оперативная память", "Не добавлено");
        }

        if (build.Components.Storages?.Count > 0)
        {
            foreach (var storage in build.Components.Storages)
                AddRow("Накопитель", storage.FullName);
        }
        else
        {
            AddRow("Накопитель", "Не добавлено");
        }
        
        AddRow("Видеокарта", build.Components.Gpu?.FullName);
        AddRow("Блок питания", build.Components.Psu?.FullName);
        AddRow("Корпус", build.Components.Case?.FullName);
        
        worksheet.Cells[$"A{row}:B{row}"].Merge = true;
        worksheet.Cells[$"A{row}"].Value = $"Дата: {DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
        worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        var borderCells = worksheet.Cells[$"A1:B{row}"];
        borderCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        borderCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        borderCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        borderCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        
        worksheet.Cells.AutoFitColumns();

        return await package.GetAsByteArrayAsync();

        void AddRow(string category, string? name)
        {
            worksheet.Cells[$"A{row}"].Value = category;
            worksheet.Cells[$"B{row}"].Value = name ?? "Не добавлено";
            row++;
        }
    }
}