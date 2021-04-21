using Codefire.Tools.RemapDatabaseId.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;

namespace Codefire.Tools.RemapDatabaseId.Data.Implementation
{
    public class BaseRepository : IDisposable
    {
        protected readonly SqlConnection Connection;
        protected readonly ICommandBuilder CommandBuilder;
        private bool _disposedValue;

        public BaseRepository(IOptions<Settings> options, ICommandBuilder commandBuilder)
        {
            Connection = new SqlConnection(options.Value.ConnectionString);
            CommandBuilder = commandBuilder;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Connection.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
