using FastMember;
using System.Data;

namespace DaemonTechChallenge.Helpers;

public class DataTableFormatHelper : IDataTableFormat
{
    private string[]? MembersOrder { get; set; }

    public DataTableFormatHelper(string[]? membersOrder)
    {
        MembersOrder = membersOrder;
    }

    public DataTable Format<T>(IEnumerable<T> bulk)
    {
        var datatable = new DataTable();
        using var reader = ObjectReader.Create(bulk, MembersOrder);
        datatable.Load(reader);

        return datatable;
    }
}
