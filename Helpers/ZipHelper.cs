using System.IO.Compression;

namespace DaemonTechChallenge.Helpers;

public class ZipHelper : IZiperHelper
{
    public List<MemoryStream> Read(Stream stream, string fileType)
    {
        var filesStreams = new List<MemoryStream>();

        using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
        for (int i = 0; i < zipArchive.Entries.Count; i++)
        {
            var entry = zipArchive.Entries[i];
            if (entry.Name.EndsWith(fileType))
            {
                var fileStream = new MemoryStream();
                entry.Open().CopyTo(fileStream);
                fileStream.Seek(0, SeekOrigin.Begin);
                filesStreams.Add(fileStream);
            }
        }

        return filesStreams;
    }
}
