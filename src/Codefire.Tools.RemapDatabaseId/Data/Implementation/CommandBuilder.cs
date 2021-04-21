using Codefire.Tools.RemapDatabaseId.Models.Requests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Codefire.Tools.RemapDatabaseId.Data.Implementation
{
    public class CommandBuilder : ICommandBuilder
    {
        public SqlCommand BuildGetTablesCommand(SqlConnection connection)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT t.object_id AS Id, s.name AS SchemaName, t.name AS TableName, ic.column_id AS PrimaryKeyId, c.name AS PrimaryKeyName, tp.name AS PrimaryKeyType
                                FROM sys.tables AS t
                                INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id
                                INNER JOIN sys.indexes AS i ON i.object_id = t.object_id
                                INNER JOIN sys.index_columns AS ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
                                INNER JOIN sys.columns as c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                                INNER JOIN sys.types AS tp ON tp.user_type_id = c.user_type_id
                                WHERE t.type = 'U' AND i.is_primary_key = 1
                                ORDER BY s.name, t.name";

            return cmd;
        }

        public SqlCommand BuildGetForeignKeysCommand(SqlConnection connection)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"select fk.name AS ForeignKeyName,
                                fkc.referenced_object_id AS PrimaryTableId,
                                fkc.referenced_column_id AS PrimaryColumnId,
                                t.object_id AS ForeignTableId,
                                s.name AS ForeignSchemaName,
                                t.Name AS ForeignTableName,
                                c.column_id AS ForeignColumnId,
                                c.name AS ForeignColumnName
                                from sys.foreign_keys fk 
                                inner join sys.foreign_key_columns AS fkc on fk.object_id = fkc.constraint_object_id
                                inner join sys.tables AS t on t.object_id = fkc.parent_object_id
                                inner join sys.schemas AS s on s.schema_id = t.schema_id
                                inner join sys.columns c on c.object_id = fkc.parent_object_id and c.column_id=fkc.parent_column_id 
                                ORDER BY PrimaryTableId";

            return cmd;
        }

        public SqlCommand BuildEnableForeignKeyCommand(SqlConnection connection, EnableForeignKeyRequest request)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"ALTER TABLE [{request.SchemaName}].[{request.TableName}] WITH CHECK CHECK CONSTRAINT [{request.ForeignKeyName}]";

            return cmd;
        }

        public SqlCommand BuildDisableForeignKeyCommand(SqlConnection connection, DisableForeignKeyRequest request)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"ALTER TABLE [{request.SchemaName}].[{request.TableName}] NOCHECK CONSTRAINT [{request.ForeignKeyName}]";

            return cmd;
        }

        public SqlCommand BuildGetDataCommand(SqlConnection connection, GetDataRequest request)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"SELECT [{request.ColumnName}] FROM [{request.SchemaName}].[{request.TableName}]";

            return cmd;
        }

        public SqlCommand BuildUpdateColumnCommand(SqlConnection connection, UpdateColumnRequest request)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $@"UPDATE [{request.SchemaName}].[{request.TableName}] SET
                                 [{request.ColumnName}] = @NewValue
                                 WHERE [{request.ColumnName}] = @OldValue";

            cmd.Parameters.AddWithValue("@OldValue", request.OldValue);
            cmd.Parameters.AddWithValue("@NewValue", request.NewValue);

            return cmd;
        }
    }
}
