using Codefire.Tools.RemapDatabaseId.Configuration;
using Codefire.Tools.RemapDatabaseId.Models.Requests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Codefire.Tools.RemapDatabaseId.Data.Implementation
{
    public class DataRepository : BaseRepository, IDataRepository
    {
        public DataRepository(IOptions<Settings> options, ICommandBuilder commandBuilder)
            : base(options, commandBuilder)
        {
        }

        public async Task<IEnumerable<Guid>> GetDataAsync(GetDataRequest request)
        {
            var results = new List<Guid>();

            var cmd = CommandBuilder.BuildGetDataCommand(Connection, request);

            await Connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            while (await reader.ReadAsync())
            {
                results.Add(reader.GetGuid(0));
            }

            return results;
        }

        public async Task UpdateColumnAsync(UpdateColumnRequest request)
        {
            var cmd = CommandBuilder.BuildUpdateColumnCommand(Connection, request);

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
