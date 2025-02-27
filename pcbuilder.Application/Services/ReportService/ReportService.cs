using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using pcbuilder.Application.DTOs.Builds;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Color = System.Drawing.Color;

namespace pcbuilder.Application.Services.ReportService;

public class ReportService : IReportService
{
    public async Task<byte[]> GenerateBuildExcelReport(GenerateBuildReportDto build)
    {
        ExcelPackage.License.SetNonCommercialPersonal("MaxLeontev");
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Конфигурация ПК");
        
        worksheet.Cells["A1:B1"].Merge = true;
        worksheet.Cells["A1"].Value = $"Конфигурация ПК \"{build.Name}\"";
        worksheet.Cells["A1"].Style.Font.Bold = true;
        worksheet.Cells["A1"].Style.Font.Size = 16;
        worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        worksheet.Cells["A2:B2"].Merge = true;
        worksheet.Cells["A2"].Value = build.Description ?? "Описание отсутствует";
        worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        worksheet.Cells["A3"].Value = "Категория";
        worksheet.Cells["B3"].Value = "Наименование";
    
        var headerStyle = worksheet.Cells["A3:B3"].Style;
        headerStyle.Font.Bold = true;
        headerStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerStyle.Fill.PatternType = ExcelFillStyle.Solid;
        headerStyle.Fill.BackgroundColor.SetColor(Color.LightGray);
        headerStyle.Border.BorderAround(ExcelBorderStyle.Thin);
        
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

        var borderCells = worksheet.Cells[$"A3:B{row}"];
        borderCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        borderCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        borderCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        borderCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        worksheet.Cells[$"A1:B{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        
        worksheet.Cells.AutoFitColumns();

        return await package.GetAsByteArrayAsync();

        void AddRow(string category, string? name)
        {
            worksheet.Cells[$"A{row}"].Value = category;
            worksheet.Cells[$"B{row}"].Value = name ?? "Не добавлено";
            row++;
        }
    }

    public byte[] GenerateBuildPdfReport(GenerateBuildReportDto build)
    {
        QuestPDF.Settings.License = LicenseType.Community; 
        var pdfDocument = Document.Create(container =>
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header()
                    .AlignCenter()
                    .Text($"Конфигурация ПК \"{build.Name}\"")
                    .Bold()
                    .FontSize(16);

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text(build.Description ?? "Описание отсутствует");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten3).Border(1).Padding(5).Text("Категория")
                                .Bold();
                            header.Cell().Background(Colors.Grey.Lighten3).Border(1).Padding(5).Text("Наименование")
                                .Bold();
                        });
                        
                        AddRow(table, "Процессор", build.Components.Cpu?.FullName);
                        AddRow(table, "Материнская плата", build.Components.Motherboard?.FullName);
                        AddRow(table, "Система охлаждения", build.Components.Cooler?.FullName);

                        if (build.Components.Rams?.Count > 0)
                        {
                            foreach (var ram in build.Components.Rams)
                                AddRow(table, "Оперативная память", ram.FullName);
                        }
                        else
                        {
                            AddRow(table, "Оперативная память", "Не добавлено");
                        }

                        if (build.Components.Storages?.Count > 0)
                        {
                            foreach (var storage in build.Components.Storages)
                                AddRow(table, "Накопитель", storage.FullName);
                        }
                        else
                        {
                            AddRow(table, "Накопитель", "Не добавлено");
                        }

                        AddRow(table, "Видеокарта", build.Components.Gpu?.FullName);
                        AddRow(table, "Блок питания", build.Components.Psu?.FullName);
                        AddRow(table, "Корпус", build.Components.Case?.FullName);
                    });
                    
                    col.Item().Text($"Дата: {DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
                });
            }));
        
        using var stream = new MemoryStream();
        pdfDocument.GeneratePdf(stream);
        return stream.ToArray();
        
        void AddRow(TableDescriptor table, string category, string? name)
        {
            table.Cell().Border(1).Padding(5).Text(category);
            table.Cell().Border(1).Padding(5).Text(name ?? "Не добавлено");
        }
    }
}