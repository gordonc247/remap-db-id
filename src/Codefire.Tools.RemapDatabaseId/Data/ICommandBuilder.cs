using Codefire.Tools.RemapDatabaseId.Models.Requests;
using System.Data.SqlClient;

namespace Codefire.Tools.RemapDatabaseId.Data
{
    public interface ICommandBuilder
    {
        SqlCommand BuildGetTablesCommand(SqlConnection connection);
        SqlCommand BuildGetForeignKeysCommand(SqlConnection connection);
        SqlCommand BuildEnableForeignKeyCommand(SqlConnection connection, EnableForeignKeyRequest request);
        SqlCommand BuildDisableForeignKeyCommand(SqlConnection connection, DisableForeignKeyRequest request);

        SqlCommand BuildGetDataCommand(SqlConnection connection, GetDataRequest request);
        SqlCommand BuildUpdateColumnCommand(SqlConnection connection, UpdateColumnRequest request);
    }
}
