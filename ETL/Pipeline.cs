using CsvHelper;
using CsvHelper.Configuration;
using DaemonTechChallenge.Models;
using FastMember;
using MySqlConnector;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Threading.Tasks.Dataflow;


namespace DaemonTechChallenge.ETL;

abstract public class Pipeline
{
    public static TransformBlock<string, Stream?> CreateDownloadZipBlock()
    {
        return new TransformBlock<string, Stream?>(async url =>
        {
            int attempts = 5;
            while (attempts > 0)
            {
                try
                {
                    using var client = new HttpClient();
                    Console.WriteLine($"Download data from: {url}");
                    var response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync();
                    return stream;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Download failed. Retry {attempts} of 5. Error: {ex.Message}");
                    attempts--;
                    if (attempts == 0)
                    {
                        return null;
                    }
                }
            }

            return null;
        });
    }


    public static TransformManyBlock<Stream?, DailyReport> CreateUnzipBlock()
    {
        return new TransformManyBlock<Stream?, DailyReport>(async downloadResult =>
        {
            if (downloadResult == null)
            {
                return Enumerable.Empty<DailyReport>();
            }

            try
            {
                using var zipArchive = new ZipArchive(downloadResult, ZipArchiveMode.Read);
                var tasks = new List<(int Index, Task<List<DailyReport>> Task)>();

                for (int i = 0; i < zipArchive.Entries.Count; i++)
                {
                    var entry = zipArchive.Entries[i];
                    if (entry.Name.EndsWith(".csv"))
                    {
                        var task = Task.Run(async () =>
                        {
                            Console.WriteLine($"Processing {entry.Name}");
                            using var entryStream = entry.Open();
                            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
                            using var csvParser = new CsvReader(new StreamReader(entryStream), config);

                            csvParser.Context.RegisterClassMap<CsvRecordMap>();
                            return await Task.FromResult(csvParser.GetRecords<DailyReport>().ToList());
                        });

                        tasks.Add((i, task));
                    }
                }

                var results = await Task.WhenAll(tasks.Select(t => t.Task));
                var orderedResults = tasks.OrderBy(t => t.Index).SelectMany((t, i) => results[i]).ToList();
                return orderedResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unzip failed. Error: {ex.Message}");
                return Enumerable.Empty<DailyReport>();
            }
        });
    }

    public static BatchBlock<DailyReport> CreateBatchBlock()
    {
        return new BatchBlock<DailyReport>(20000);
    }

    public static TransformBlock<IEnumerable<DailyReport>, DataTable> CreateConvertToDataTableBlockAsync()
    {
        return new TransformBlock<IEnumerable<DailyReport>, DataTable>(bulk =>
        {
            var datatable = new DataTable();
            using var reader = ObjectReader.Create(bulk, "Id", "CnpjFundo", "DtComptc", "VlTotal", "VlQuota", "VlPatrimLiq", "CaptcDia", "ResgDia", "NrCotst");
            datatable.Load(reader);

            return datatable;
        });
    }


    public static ActionBlock<DataTable> CreateStoreBlock(string connectionString)
    {
        return new ActionBlock<DataTable>(async dataTable =>
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString)
            {
                AllowLoadLocalInfile = true
            };
            using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
            var bulkCopy = new MySqlBulkCopy(connection)
            {
                DestinationTableName = "DailyReport",
            };

            Console.WriteLine($"Adding {dataTable.Rows.Count} rows in database");

            try
            {
                await bulkCopy.WriteToServerAsync(dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        });
    }
}
