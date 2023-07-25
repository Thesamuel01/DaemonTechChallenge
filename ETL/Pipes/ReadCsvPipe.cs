using DaemonTechChallenge.Helpers;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallenge.ETL.Pipes;

public class ReadCsvPipe
{
    private readonly ICsvHelper _csvHelper;

    public ReadCsvPipe(ICsvHelper csvHelper)
    {
        _csvHelper = csvHelper;
    }

    public TransformManyBlock<Stream, T> CreateReadCsvBlock<T>() where T : class
    {
        var transformBlock = new TransformManyBlock<Stream, T>(async stream =>
        {
            var results = new List<T>();

            Console.WriteLine("Reading CSV file.");

            using var csvStream = stream;
            var items = await _csvHelper.ReadAsync<T>(csvStream);

            return items;
        });

        return transformBlock;
    }
}
