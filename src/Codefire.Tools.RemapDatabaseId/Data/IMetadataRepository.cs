using Codefire.Tools.RemapDatabaseId.Models;
using Codefire.Tools.RemapDatabaseId.Models.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Codefire.Tools.RemapDatabaseId.Data
{
    public interface IMetadataRepository
    {
        Task<IEnumerable<TableInfo>> GetTablesAsync();
        Task<IEnumerable<ForeignKeyInfo>> GetForeignKeysAsync();
        Task EnableForeignKeysAsync(EnableForeignKeyRequest request);
        Task DisableForeignKeysAsync(DisableForeignKeyRequest request);
    }
}
