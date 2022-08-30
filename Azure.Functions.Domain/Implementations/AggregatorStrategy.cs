using Azure.Functions.Domain.Entities;
using Azure.Functions.Domain.Interfaces;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Azure.Functions.Domain.Implementations
{
    public class AggregatorStrategy : IStrategy<string, int>
    {
        private readonly IDurableClient client;
        private readonly ILogger<AggregatorStrategy> log;

        public AggregatorStrategy(IDurableClient client, ILogger<AggregatorStrategy> log)
        {
            this.client = client;
            this.log = log;
        }

        public async Task<int> ExecuteAsync(string input)
        {
            var entityId = new EntityId(nameof(AggregatorEntity), input);
            await client.SignalEntityAsync<IEntity>(entityId, _ => _.AddAsync(input));
            var result = await client.ReadEntityStateAsync<int>(entityId);
            return await Task.FromResult(result.EntityState);

        }
    }
}
