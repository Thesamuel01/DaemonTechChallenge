using DaemonTechChallenge.Data;
using DaemonTechChallenge.Models;
using DaemonTechChallenge.ETL;
using Microsoft.EntityFrameworkCore;
using DaemonTechChallenge.Services;
using DaemonTechChallenge.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = builder.Configuration;
var connectionString = config.GetConnectionString("MariaDBContext");

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IRepositoryBase, RepositoryBase>();

var app = builder.Build();
var runEtl = true;

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    runEtl = !await dbContext.Set<DailyReport>().AnyAsync();
}

if (runEtl)
{
    var etl = new StartUp(config);

    await etl.ExecuteETL();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

