using Codefire.Tools.RemapDatabaseId.Configuration;
using Codefire.Tools.RemapDatabaseId.Models;
using Codefire.Tools.RemapDatabaseId.Models.Requests;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Codefire.Tools.RemapDatabaseId.Data.Implementation
{
    public class MetadataRepository : BaseRepository, IMetadataRepository
    {
        public MetadataRepository(IOptions<Settings> options, ICommandBuilder commandBuilder)
            : base(options, commandBuilder)
        {
        }

        public async Task<IEnumerable<TableInfo>> GetTablesAsync()
        {
            var tables = new List<TableInfo>();

            var cmd = CommandBuilder.BuildGetTablesCommand(Connection);

            await Connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            while (await reader.ReadAsync())
            {
                var table = new TableInfo
                {
                    Id = reader.GetInt32(0),
                    Schema = reader.GetString(1),
                    Name = reader.GetString(2),
                    PrimaryKeyId = reader.GetInt32(3),
                    PrimaryKeyName = reader.GetString(4),
                    PrimaryKeyType = reader.GetString(5),
                };

                tables.Add(table);
            }

            return tables;
        }

        public async Task<IEnumerable<ForeignKeyInfo>> GetForeignKeysAsync()
        {
            var foreignKeys = new List<ForeignKeyInfo>();

            var cmd = CommandBuilder.BuildGetForeignKeysCommand(Connection);

            await Connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            while (await reader.ReadAsync())
            {
                var item = new ForeignKeyInfo
                {
                    Name = reader.GetString(0),
                    PrimaryTableId = reader.GetInt32(1),
                    PrimaryColumnId = reader.GetInt32(2),
                    ForeignTableId = reader.GetInt32(3),
                    ForeignSchemaName = reader.GetString(4),
                    ForeignTableName = reader.GetString(5),
                    ForeignColumnId = reader.GetInt32(6),
                    ForeignColumnName = reader.GetString(7)
                };

                foreignKeys.Add(item);
            }

            return foreignKeys;
        }

        public async Task EnableForeignKeysAsync(EnableForeignKeyRequest request)
        {
            var cmd = CommandBuilder.BuildEnableForeignKeyCommand(Connection, request);

            try
            {
                await Connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                await Connection.CloseAsync();
            }
        }

        public async Task DisableForeignKeysAsync(DisableForeignKeyRequest request)
        {
            var cmd = CommandBuilder.BuildDisableForeignKeyCommand(Connection, request);

            try
            {
                await Connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                await Connection.CloseAsync();
            }
        }
    }
}
