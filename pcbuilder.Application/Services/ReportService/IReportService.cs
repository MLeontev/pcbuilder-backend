using pcbuilder.Application.DTOs.Builds;

namespace pcbuilder.Application.Services.ReportService;

public interface IReportService
{
    public Task<byte[]> GenerateBuildExcelReport(GenerateBuildReportDto build);
    
    public byte[] GenerateBuildPdfReport(GenerateBuildReportDto build);
}