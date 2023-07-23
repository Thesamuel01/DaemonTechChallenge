using CsvHelper.Configuration;
using CsvHelper;

namespace DaemonTechChallenge.Helpers;

public class CsvHelperLib : ICsvHelper
{
    private readonly CsvConfiguration _configuration;
    private ClassMap? ClassMap { get; set; }

    public CsvHelperLib(CsvConfiguration configuration, ClassMap? classMap)
    {
        _configuration = configuration;
        ClassMap = classMap;
    }

    public async Task<List<T>> ReadAsync<T>(Stream stream)
    {
        var results = new List<T>();

        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _configuration);

        if (ClassMap != null)
        {
            csv.Context.RegisterClassMap(ClassMap);
        }

        await foreach (var record in csv.GetRecordsAsync<T>())
        {
            results.Add(record);
        }

        return results;
    }
}
