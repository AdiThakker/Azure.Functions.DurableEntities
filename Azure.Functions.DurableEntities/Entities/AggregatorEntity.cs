using Azure.Functions.Domain.Configuration;
using Azure.Functions.Domain.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Functions.DurableEntities.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AggregatorEntity : IEntity
    {
        [JsonProperty]
        private int max;
        private readonly ILogger<AggregatorEntity> logger;

        [JsonProperty]
        public List<string> Total = new();

        public AggregatorEntity(IOptions<EntityConfiguration> configuration, ILogger<AggregatorEntity> loger)
        {
            this.max = configuration.Value.Max;
            this.logger = loger;
        }

        public Task<int> AddAsync(string input)
        {
            Total.Add(input);
            if (Total.Count >this.max)
                this.logger.LogError($"Escalating as current count: {Total.Count} is greater than configured value of: {this.max}");


            return Task.FromResult(Total.Count);
        }

        public Task<int> GetAsync(string input)
        {
            return Task.FromResult(Total.Count);
        }

        [FunctionName(nameof(AggregatorEntity))]
        public static async Task Run([EntityTrigger] IDurableEntityContext ctx) => await ctx?.DispatchAsync<AggregatorEntity>();
    }
}
