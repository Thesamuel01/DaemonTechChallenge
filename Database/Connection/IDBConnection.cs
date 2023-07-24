using System.Data;

namespace DaemonTechChallenge.Data.Connection;

public interface IDBConnection
{
    public Task<bool> CloseAsync();
    public Task<bool> OpenAsync();
    public Task<bool> SaveAsync(DataTable bulk, string tablename);
    public bool Close();
    public bool Open();
}
