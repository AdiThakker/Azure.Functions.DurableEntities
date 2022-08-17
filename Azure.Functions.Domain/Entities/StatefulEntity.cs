﻿using Azure.Functions.Domain.Configuration;
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

namespace Azure.Functions.Domain.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StatefulEntity : IEntity
    {
        private readonly IOptions<EntityConfiguration> configuration;
        private readonly ILogger<StatefulEntity> logger;

        [JsonProperty]
        public List<string> Total = new();

        public StatefulEntity(IOptions<EntityConfiguration> configuration, ILogger<StatefulEntity> loger)
        {
            this.configuration = configuration;
            this.logger = loger;
        }
        
        public Task<int> AddAsync(string input)
        {
            Total.Add(input);
            return Task.FromResult(Total.Count);
        }

        public Task<int> GetAsync(string input)
        {
            return Task.FromResult(Total.Count);
        }

        [FunctionName(nameof(StatefulEntity))]
        public static async Task Run([EntityTrigger] IDurableEntityContext ctx) => await ctx?.DispatchAsync<StatefulEntity>();
    }
}
