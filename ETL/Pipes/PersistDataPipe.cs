using DaemonTechChallenge.Helpers;
using System.Data;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallenge.ETL.Pipes;

public class PersistDataPipe
{
    private readonly IDBConnection _connection;

    public PersistDataPipe(IDBConnection connection)
    {
        _connection = connection;
    }

    public ActionBlock<DataTable> CreateStoreBlock(string tableName)
    {
        var actionBlock = new ActionBlock<DataTable>(async dataTable =>
        {
            var isOpen = _connection.Open();

            Console.WriteLine($"Adding {dataTable.Rows.Count} rows in database");

            try
            {
                if (isOpen)
                {
                    await _connection.SaveAsync(dataTable, tableName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _ = await _connection.CloseAsync();
            }
        });

        _ = actionBlock.Completion.ContinueWith(task =>
        {
            _connection.Close();

            Console.WriteLine("Pipeline finished.");
        });

        return actionBlock;
    }
}
