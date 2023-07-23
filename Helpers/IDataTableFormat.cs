using System.Data;

namespace DaemonTechChallenge.Helpers;

public interface IDataTableFormat
{
    public DataTable Format<T>(IEnumerable<T> bulk);
}
