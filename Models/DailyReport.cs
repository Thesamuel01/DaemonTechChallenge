using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DaemonTechChallenge.Models;

public class DailyReport
{
    [Key]
    public long Id { get; set; }
    [Required]
    [StringLength(50)]
    public string? CnpjFundo { get; set; }
    [Column(TypeName = "Date")]
    public DateTime DtComptc { get; set; }
    [Column(TypeName = "decimal(14, 2)")]
    public decimal VlTotal { get; set; }
    [Column(TypeName = "decimal(22, 12)")]
    public decimal VlQuota { get; set; }
    [Column(TypeName = "decimal(14, 2)")]
    public decimal VlPatrimLiq { get; set; }
    [Column(TypeName = "decimal(14, 2)")]
    public decimal CaptcDia { get; set; }
    [Column(TypeName = "decimal(14, 2)")]
    public decimal ResgDia { get; set; }
    public int NrCotst { get; set; }
}
