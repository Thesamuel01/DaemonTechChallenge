namespace DaemonTechChallenge.Helpers;

public interface IZiperHelper
{
    public List<MemoryStream> Read(Stream stream, string fileType);
}
