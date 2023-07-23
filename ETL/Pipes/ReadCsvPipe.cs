using CsvHelper;
using DaemonTechChallenge.Helpers;
using DaemonTechChallenge.Models;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallenge.ETL.Pipes;

public class ReadCsvPipe
{
    private readonly ICsvHelper _csvHelper;

    public ReadCsvPipe(ICsvHelper csvHelper)
    {
        _csvHelper = csvHelper;
    }

    public TransformManyBlock<List<Stream>, T> CreateReadCsvBlock<T>()
    {
        var transformBlock = new TransformManyBlock<List<Stream>, T>(async listStream =>
        {
            var results = new List<T>();

            Console.WriteLine("Reading CSV file.");

            foreach (var stream in listStream)
            {
                using var csvStream = stream;
                var items = await _csvHelper.ReadAsync<T>(csvStream);
                results.AddRange(items);
            }

            return results;
        });

        return transformBlock;
    }
}
