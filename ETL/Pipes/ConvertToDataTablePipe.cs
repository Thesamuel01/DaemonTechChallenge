using DaemonTechChallenge.Helpers;
using FastMember;
using System.Data;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallenge.ETL.Pipes;

public class ConvertToDataTablePipe
{
    public readonly IDataTableFormat _dataTableFormat;

    public ConvertToDataTablePipe(IDataTableFormat dataTableFormat)
    {
        _dataTableFormat = dataTableFormat;
    }
    
    public TransformBlock<IEnumerable<T>, DataTable> CreateConvertToDataTableBlock<T>() where T : class
    {
        var transformBLock = new TransformBlock<IEnumerable<T>, DataTable>(bulk =>
        {
            Console.WriteLine("Transforming data in DataTable.");

            return _dataTableFormat.Format(bulk);
        });

        return transformBLock;
    }
}
