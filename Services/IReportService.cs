using DaemonTechChallenge.DTOs;

namespace DaemonTechChallenge.Services;

public interface IReportService
{
    public Task<List<DailyReportDTO>> GetReportsAsync(string CNPJ, DateTime? startDate, DateTime? endDate);
}
