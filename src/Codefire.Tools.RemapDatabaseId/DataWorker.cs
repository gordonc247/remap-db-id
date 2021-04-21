using Codefire.Tools.RemapDatabaseId.Data;
using Codefire.Tools.RemapDatabaseId.Models;
using Codefire.Tools.RemapDatabaseId.Models.Requests;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Codefire.Tools.RemapDatabaseId
{
    public class DataWorker : BackgroundService
    {
        private readonly IMetadataRepository _metadataRepository;
        private readonly IDataRepository _dataRepository;
        private readonly IHostApplicationLifetime _appLifetime;

        public DataWorker(IMetadataRepository metadataRepository, IDataRepository dataRepository, IHostApplicationLifetime appLifetime)
        {
            _metadataRepository = metadataRepository;
            _dataRepository = dataRepository;
            _appLifetime = appLifetime;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tables = await _metadataRepository.GetTablesAsync();
            var foreignKeys = await _metadataRepository.GetForeignKeysAsync();

            foreach (var table in tables)
            {
                if (table.PrimaryKeyType != "uniqueidentifier") continue;

                Console.Write($"Updating {table.Name}  ");

                var stopwatch = Stopwatch.StartNew();

                var references = foreignKeys.Where(x => x.PrimaryTableId == table.Id && x.PrimaryColumnId == table.PrimaryKeyId).ToList();
                await DisableForeignKeys (references);

                var dataRequest = new GetDataRequest
                {
                    SchemaName = table.Schema,
                    TableName = table.Name,
                    ColumnName = table.PrimaryKeyName
                };

                var data = await _dataRepository.GetDataAsync(dataRequest);

                var (cursorLeft, cursorTop) = Console.GetCursorPosition();
                var count = 0;
                foreach (var id in data)
                {
                    count++;
                    if (count % 50 == 0)
                    {
                        Console.SetCursorPosition(cursorLeft, cursorTop);
                        Console.Write($"{count}");
                    }

                    var newId = SequentialGuid.NewGuid();

                    var updateIdRequest = new UpdateColumnRequest
                    {
                        SchemaName = table.Schema,
                        TableName = table.Name,
                        ColumnName = table.PrimaryKeyName,
                        OldValue = id,
                        NewValue = newId
                    };

                    await _dataRepository.UpdateColumnAsync(updateIdRequest);

                    foreach (var fk in references)
                    {
                        var updateKeyRequest = new UpdateColumnRequest
                        {
                            SchemaName = fk.ForeignSchemaName,
                            TableName = fk.ForeignTableName,
                            ColumnName = fk.ForeignColumnName,
                            OldValue = id,
                            NewValue = newId
                        };

                        await _dataRepository.UpdateColumnAsync(updateKeyRequest);
                    }
                }

                await EnableForeignKeys(references);

                stopwatch.Stop();
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.WriteLine($" Done ({stopwatch.Elapsed})");
            }

            _appLifetime.StopApplication();
        }

        private async Task EnableForeignKeys(IEnumerable<ForeignKeyInfo> references)
        {
            foreach (var item in references)
            {
                var request = new EnableForeignKeyRequest
                {
                    ForeignKeyName = item.Name,
                    SchemaName = item.ForeignSchemaName,
                    TableName = item.ForeignTableName
                };

                await _metadataRepository.EnableForeignKeysAsync(request);
            }
        }

        private async Task DisableForeignKeys(IEnumerable<ForeignKeyInfo> references)
        {
            foreach (var item in references)
            {
                var request = new DisableForeignKeyRequest
                {
                    ForeignKeyName = item.Name,
                    SchemaName = item.ForeignSchemaName,
                    TableName = item.ForeignTableName
                };

                await _metadataRepository.DisableForeignKeysAsync(request);
            }
        }
    }
}
