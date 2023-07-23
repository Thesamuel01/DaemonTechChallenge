using DaemonTechChallenge.Models;

namespace DaemonTechChallenge.Services;

public interface IReportService
{
    public Task<List<DailyReport>> GetReportsAsync(string CNPJ, DateTime? startDate, DateTime? endDate);
}
