using Codefire.Tools.RemapDatabaseId.Configuration;
using Codefire.Tools.RemapDatabaseId.Data;
using Codefire.Tools.RemapDatabaseId.Data.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Codefire.Tools.RemapDatabaseId
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            // Configure options
            services.AddOptions();
            services.Configure<Settings>(hostContext.Configuration.GetSection("Settings"));

            services.AddTransient<ICommandBuilder, CommandBuilder>();
            services.AddTransient<IMetadataRepository, MetadataRepository>();
            services.AddTransient<IDataRepository, DataRepository>();

            services.AddHostedService<DataWorker>();
        }
    }
}
