namespace DaemonTechChallenge.DTOs;

public record DailyReportDTO(
    long Id,
    string? CnpjFundo,
    DateTime DtComptc,
    decimal VlTotal,
    decimal VlQuota,
    decimal VlPatrimLiq,
    decimal CaptcDia,
    decimal ResgDia,
    int NrCotst
);

