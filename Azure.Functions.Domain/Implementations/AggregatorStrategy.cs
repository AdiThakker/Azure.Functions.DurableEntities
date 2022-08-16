﻿using Azure.Functions.Domain.Interfaces;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
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
        
        public Task<int> ExecuteAsync(string input)
        {
            throw new NotImplementedException();
        }
    }
}