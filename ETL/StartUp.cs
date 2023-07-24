using Microsoft.EntityFrameworkCore;
using DaemonTechChallenge.ETL.Pipes;
using DaemonTechChallenge.Helpers;
using CsvHelper.Configuration;
using System.Globalization;
using DaemonTechChallenge.Models;
using System.Threading.Tasks.Dataflow;
using System.Diagnostics;
using DaemonTechChallenge.Data.Connection;

namespace DaemonTechChallenge.ETL;

public class StartUp
{
    private readonly string _extracUrl;
    private readonly string _connectionString;

    public StartUp(IConfiguration config)
    {
        var extractUrl = config.GetSection("Urls").GetSection("extracUrl").Value;
        var connectionString = config.GetConnectionString("MariaDBContext");

        if (string.IsNullOrEmpty(extractUrl))
        {
            throw new InvalidOperationException("The 'Extract' url string is not defined.");
        }

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("The 'MariaDBContext' connection string is not defined.");
        }

        _extracUrl = extractUrl;
        _connectionString = connectionString;
    }

    public async Task ExecuteETL()
    {
        var stopwatch = Stopwatch.StartNew();
        var urls = GenerateUrls();

        foreach (var url in urls)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
            var tableOrder = new[] { "Id", "CnpjFundo", "DtComptc", "VlTotal", "VlQuota", "VlPatrimLiq", "CaptcDia", "ResgDia", "NrCotst" };
            var fileExt = ".csv";
            var tableName = "DailyReport";
            var linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true,
            };

            var zipHelper = new ZipHelper();
            var csvHelper = new CsvHelperLib(csvConfig, new CsvRecordMap());
            var dataTableFormatHelper = new DataTableFormatHelper(tableOrder);
            var dbConnection = new DBConnection(_connectionString);

            var fetchZipData = new FetchZipDataPipe(zipHelper);
            var readCsv = new ReadCsvPipe(csvHelper);
            var batchBlock = new BatchBlock<DailyReport>(15000);
            var convertToDataTable = new ConvertToDataTablePipe(dataTableFormatHelper);
            var persistData = new PersistDataPipe(dbConnection);

            var pipeline = new Pipeline(fetchZipData.CreateFetchZipDataBlock(fileExt), linkOptions);

            pipeline.AddStageToPipeline(readCsv.CreateReadCsvBlock<DailyReport>());
            pipeline.AddStageToPipeline(batchBlock);
            pipeline.AddStageToPipeline(convertToDataTable.CreateConvertToDataTableBlock<DailyReport>());
            pipeline.AddStageToPipeline(persistData.CreateStoreBlock(tableName));

            await pipeline.Startup(url);
        }

        stopwatch.Stop();

        Console.WriteLine($"ETL execution took: {stopwatch.Elapsed.TotalMinutes} minutes");
    }

    private List<string> GenerateUrls()
    {
        var urls = new List<string>();
        var currentDate = DateTime.Now;
        var startDate = new DateTime(2017, 1, 1);

        while (startDate <= currentDate)
        {
            string url;
            if (startDate.Year < 2021)
            {
                url = _extracUrl + "/HIST" + $"/inf_diario_fi_{startDate.Year}.zip";
                startDate = startDate.AddYears(1);
            }
            else
            {
                url = _extracUrl + $"/inf_diario_fi_{startDate:yyyyMM}.zip";
                startDate = startDate.AddMonths(1);
            }

            urls.Add(url);
        }

        return urls;
    }
}
