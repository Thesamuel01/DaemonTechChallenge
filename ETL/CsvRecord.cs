namespace DaemonTechChallenge.ETL;

public record CsvRecord
{
    public string CNPJ_FUNDO { get; set; } = string.Empty;
    public string DT_COMPTC { get; set; } = string.Empty;
    public string VL_TOTAL { get; set; } = string.Empty;
    public string VL_QUOTA { get; set; } = string.Empty;
    public string VL_PATRIM_LIQ { get; set; } = string.Empty;
    public string CAPTC_DIA { get; set; } = string.Empty;
    public string RESG_DIA { get; set; } = string.Empty;
    public string NR_COTST { get; set; } = string.Empty;
}
