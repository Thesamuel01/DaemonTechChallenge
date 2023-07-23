using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;

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
        var urls = GenerateUrls();

        foreach (var url in urls)
        {
            var downloadBlock = Pipeline.CreateDownloadZipBlock();
            var unzipBlock = Pipeline.CreateUnzipBlock();
            var convertToDataTableBlock = Pipeline.CreateConvertToDataTableBlockAsync();
            var batchBlock = Pipeline.CreateBatchBlock();
            var storeBlock = Pipeline.CreateStoreBlock(_connectionString);

            var linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true,
            };
            downloadBlock.LinkTo(unzipBlock, linkOptions);
            unzipBlock.LinkTo(batchBlock, linkOptions);
            batchBlock.LinkTo(convertToDataTableBlock, linkOptions);
            convertToDataTableBlock.LinkTo(storeBlock, linkOptions);

            downloadBlock.Post(url);
            downloadBlock.Complete();

            await storeBlock.Completion;
        }

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
