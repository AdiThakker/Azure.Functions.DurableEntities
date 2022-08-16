using Azure.Functions.Domain.Configuration;
using Azure.Functions.Domain.Implementations;
using Azure.Functions.Domain.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(Azure.Functions.DurableEntities.Startup))]
namespace Azure.Functions.DurableEntities
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            builder.Services
                .AddOptions<EntityConfiguration>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("EntityConfiguration").Bind(settings));

            builder.Services.AddDurableClientFactory(op =>
            {
                op.ConnectionName = "AzureWebJobsStorage";
                op.TaskHub = "TestHubName";
            });

            builder.Services.AddSingleton(typeof(AggregatorStrategy), sp =>
            {
                var configuration = sp.GetRequiredService<IOptions<EntityConfiguration>>();
                var durableClientFactory = sp.GetRequiredService<IDurableClientFactory>();

                return new AggregatorStrategy(durableClientFactory.CreateClient(), sp.GetRequiredService<ILogger<AggregatorStrategy>>());
            });
        }
    }
}
