using Microsoft.EntityFrameworkCore;
using DaemonTechChallenge.Data;
using DaemonTechChallenge.Models;
using DaemonTechChallenge.DTOs;

namespace DaemonTechChallenge.Services;

public class ReportService : IReportService
{
    private readonly IRepositoryBase _context;

    public ReportService(IRepositoryBase context)
    {
        _context = context;
    }

    public async Task<List<DailyReportDTO>> GetReportsAsync(string? CNPJ, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.GetQueryable<DailyReport>();

        if (string.IsNullOrEmpty(CNPJ))
        {
            throw new ArgumentException("CNPJ param is required.");
        }

        query = query.Where(r => r.CnpjFundo == CNPJ);

        if (startDate.HasValue)
        {
            query = query.Where(r => r.DtComptc >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.DtComptc < endDate.Value);
        }

        var reports = (await query.ToListAsync())
            .Select(r => new DailyReportDTO(
                r.Id,
                r.CnpjFundo,
                r.DtComptc,
                r.VlTotal,
                r.VlQuota,
                r.VlPatrimLiq,
                r.CaptcDia,
                r.ResgDia,
                r.NrCotst
            )).ToList();

        return reports;
    }
}
