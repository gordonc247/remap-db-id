using Codefire.Tools.RemapDatabaseId.Models.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Codefire.Tools.RemapDatabaseId.Data
{
    public interface IDataRepository
    {
        Task<IEnumerable<Guid>> GetDataAsync(GetDataRequest request);
        Task UpdateColumnAsync(UpdateColumnRequest request);
    }
}
