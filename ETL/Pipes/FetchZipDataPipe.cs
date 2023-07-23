using DaemonTechChallenge.Helpers;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallenge.ETL.Pipes;

public class FetchZipDataPipe
{
    private readonly IZiperHelper _ziperHelper;

    public FetchZipDataPipe(IZiperHelper ziperHelper)
    {
        _ziperHelper = ziperHelper;
    }

    public TransformBlock<string, List<Stream>> CreateFetchZipDataBlock(string fileExt)
    {
        var transformBlock = new TransformBlock<string, List<Stream>>(async url =>
        {
            var attempts = 5;
            var streamList = new List<Stream>();

            while (attempts > 0)
            {
                try
                {
                    using var client = new HttpClient();
                    Console.WriteLine($"Download data from: {url}");
                    var response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync();
                    streamList.AddRange(_ziperHelper.Read(stream, fileExt));

                    return streamList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Download failed. Retry {attempts} of 5. Error: {ex.Message}");
                    attempts--;
                    if (attempts == 0)
                    {
                        return streamList;
                    }
                }
            }

            return streamList;
        });

        return transformBlock;
    }
}
