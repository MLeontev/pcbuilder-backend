using pcbuilder.Application.DTOs.Builds;

namespace pcbuilder.Application.Services.ReportService;

public interface IReportService
{
    Task<byte[]> GenerateBuildExcelReport(GenerateBuildReportDto generateBuildReportDto);
}