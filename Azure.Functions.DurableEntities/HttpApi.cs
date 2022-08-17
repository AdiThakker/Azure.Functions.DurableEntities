using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Functions.Domain.Interfaces;

namespace Azure.Functions.DurableEntities
{
    public class HttpApi
    {
        public IStrategy<string, int> Strategy { get; }
        public ILogger<HttpApi> Logger { get; }

        public HttpApi(IStrategy<string, int> strategy, ILogger<HttpApi> loger)
        {
            Strategy = strategy;
            Logger = loger;
        }


        [FunctionName("HttpApi")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            Logger.LogInformation("HttpApi function received a request.");

            string entity = req.Query["entity"];

            var result = await Strategy.ExecuteAsync(entity ?? "defaultEntity");

            return new OkObjectResult(result);
        }
    }
}
