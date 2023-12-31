﻿using DaemonTechChallenge.Helpers;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallenge.ETL.Pipes;

public class FetchZipDataPipe
{
    private readonly IZiperHelper _ziperHelper;
    private readonly HttpClient _httpClient;

    public FetchZipDataPipe(IZiperHelper ziperHelper, HttpClient httpClient)
    {
        _ziperHelper = ziperHelper;
        _httpClient = httpClient;
    }

    public TransformManyBlock<string, MemoryStream> CreateFetchZipDataBlock(string fileExt)
    {
        var transformBlock = new TransformManyBlock<string, MemoryStream>(async url =>
        {
            var attempts = 5;

            while (attempts > 0)
            {
                try
                {
                    Console.WriteLine($"Download data from: {url}");
                    var response = await _httpClient.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync();

                    return _ziperHelper.Read(stream, fileExt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Download failed. Retry {attempts} of 5. Error: {ex.Message}");
                    attempts--;
                    if (attempts == 0)
                    {
                        return Enumerable.Empty<MemoryStream>();
                    }
                }
            }

            return Enumerable.Empty<MemoryStream>();
        });

        return transformBlock;
    }
}
