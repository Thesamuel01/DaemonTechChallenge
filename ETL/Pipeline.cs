using CsvHelper;
using CsvHelper.Configuration;
using MySqlConnector;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Threading.Tasks.Dataflow;


namespace DaemonTechChallenge.ETL;

abstract public class Pipeline
{
    public static TransformBlock<string, Stream?> CreateDownloadBlock()
    {
        return new TransformBlock<string, Stream?>(async url =>
        {
            int attempts = 5;
            while (attempts > 0)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        Console.WriteLine($"Download data from: {url}");
                        var response = await client.GetAsync(url);

                        response.EnsureSuccessStatusCode();

                        var stream = await response.Content.ReadAsStreamAsync();
                        return stream;
                    }
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


    public static TransformManyBlock<Stream?, CsvRecord> CreateUnzipBlock()
    {
        return new TransformManyBlock<Stream?, CsvRecord>(async downloadResult =>
        {
            if (downloadResult == null)
            {
                return Enumerable.Empty<CsvRecord>();
            }

            using (var zipStream = downloadResult)
            {
                try
                {
                    using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                    {
                        var tasks = new List<(int Index, Task<List<CsvRecord>> Task)>();

                        for (int i = 0; i < zipArchive.Entries.Count; i++)
                        {
                            var entry = zipArchive.Entries[i];
                            if (entry.Name.EndsWith(".csv"))
                            {
                                var task = Task.Run(async () =>
                                {
                                    Console.WriteLine($"Processing {entry.Name}");
                                    using (var entryStream = entry.Open())
                                    {
                                        var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };
                                        using var csvParser = new CsvReader(new StreamReader(entryStream), config);
                                        return await Task.FromResult(csvParser.GetRecords<CsvRecord>().ToList());
                                    }
                                });

                                tasks.Add((i, task));
                            }
                        }

                        var results = await Task.WhenAll(tasks.Select(t => t.Task));
                        var orderedResults = tasks.OrderBy(t => t.Index).SelectMany((t, i) => results[i]).ToList();
                        return orderedResults;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unzip failed. Error: {ex.Message}");
                    return Enumerable.Empty<CsvRecord>();
                }
            }
        });
    }

    public static BatchBlock<CsvRecord> CreateBatchBlock()
    {
        return new BatchBlock<CsvRecord>(20000);
    }

    public static TransformBlock<IEnumerable<CsvRecord>, DataTable> CreateConvertToDataTableBlockAsync()
    {
        return new TransformBlock<IEnumerable<CsvRecord>, DataTable>(bulk =>
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("CnpjFundo", typeof(string));
            dataTable.Columns.Add("DtComptc", typeof(DateTime));
            dataTable.Columns.Add("VlTotal", typeof(decimal));
            dataTable.Columns.Add("VlQuota", typeof(decimal));
            dataTable.Columns.Add("VlPatrimLiq", typeof(decimal));
            dataTable.Columns.Add("CaptcDia", typeof(decimal));
            dataTable.Columns.Add("ResgDia", typeof(decimal));
            dataTable.Columns.Add("NrCotst", typeof(int));

            foreach (var record in bulk)
            {
                dataTable.Rows.Add(
                    0,
                    record.CNPJ_FUNDO,
                    DateTime.Parse(record.DT_COMPTC).Date,
                    decimal.Parse(record.VL_TOTAL, CultureInfo.InvariantCulture),
                    decimal.Parse(record.VL_QUOTA, CultureInfo.InvariantCulture),
                    decimal.Parse(record.VL_PATRIM_LIQ, CultureInfo.InvariantCulture),
                    decimal.Parse(record.CAPTC_DIA, CultureInfo.InvariantCulture),
                    decimal.Parse(record.RESG_DIA, CultureInfo.InvariantCulture),
                    int.Parse(record.NR_COTST));
            }

            return dataTable;
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
            using (var connection = new MySqlConnection(connectionStringBuilder.ConnectionString))
            {
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
            }
        });
    }
}
