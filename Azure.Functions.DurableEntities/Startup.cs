using Azure.Functions.Domain.Configuration;
using Azure.Functions.Domain.Interfaces;
using Azure.Functions.DurableEntities.Implementations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: FunctionsStartup(typeof(Azure.Functions.DurableEntities.Startup))]
namespace Azure.Functions.DurableEntities
{
    public class Startup : FunctionsStartup
    {

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            builder.Services
                .AddOptions<EntityConfiguration>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("EntityConfiguration").Bind(settings));

            builder.Services.AddDurableClientFactory(op =>
            {
                op.ConnectionName = "AzureWebJobsStorage";
                op.TaskHub = "DurableEntitiesHub";
                op.IsExternalClient = true;
            });

            builder.Services.AddSingleton<IStrategy<string,int>>(sp =>
            {
                var configuration = sp.GetRequiredService<IOptions<EntityConfiguration>>();
                var durableClientFactory = sp.GetRequiredService<IDurableClientFactory>();

                return new AggregatorStrategy(durableClientFactory.CreateClient(), sp.GetRequiredService<ILogger<AggregatorStrategy>>());
            });
        }
    }
}
