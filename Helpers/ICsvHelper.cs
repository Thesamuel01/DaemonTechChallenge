namespace DaemonTechChallenge.Helpers;

public interface ICsvHelper
{
    public Task<List<T>> ReadAsync<T>(Stream stream);
}
