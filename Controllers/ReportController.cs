using DaemonTechChallenge.Models;
using DaemonTechChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace DaemonTechChallenge.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReports([FromQuery] string CNPJ, [FromQuery] DateTime? StartDate, [FromQuery] DateTime? EndDate)
    {
        if (string.IsNullOrEmpty(CNPJ))
        {
            return BadRequest("O parâmetro CNPJ é obrigatório.");
        }

        List<DailyReport> reports = await _reportService.GetReportsAsync(CNPJ, StartDate, EndDate);

        return Ok(reports);
    }
}
