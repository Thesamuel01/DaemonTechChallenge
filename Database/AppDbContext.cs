using DaemonTechChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace DaemonTechChallenge.Data;

public class AppDbContext : DbContext
{
    public DbSet<DailyReport> DailyReport { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
