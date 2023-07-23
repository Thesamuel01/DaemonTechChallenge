using DaemonTechChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace DaemonTechChallenge.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<DailyReport> DailyReport { get; set; }
}
