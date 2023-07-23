using CsvHelper.Configuration;
using DaemonTechChallenge.Models;

namespace DaemonTechChallenge.ETL;

public sealed class CsvRecordMap : ClassMap<DailyReport>
{
    public CsvRecordMap()
    {
        Map(m => m.CnpjFundo).Name("CNPJ_FUNDO");
        Map(m => m.DtComptc).Name("DT_COMPTC");
        Map(m => m.VlTotal).Name("VL_TOTAL").TypeConverterOption.Format("N2");
        Map(m => m.VlQuota).Name("VL_QUOTA").TypeConverterOption.Format("N12");
        Map(m => m.VlPatrimLiq).Name("VL_PATRIM_LIQ").TypeConverterOption.Format("N2");
        Map(m => m.CaptcDia).Name("CAPTC_DIA").TypeConverterOption.Format("N2");
        Map(m => m.ResgDia).Name("RESG_DIA").TypeConverterOption.Format("N2");
        Map(m => m.NrCotst).Name("NR_COTST");
    }
}