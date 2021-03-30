using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QueuesFunctions.Extensions;
using QueuesFunctions.Models.Dtos;
using QueuesFunctions.Services;
using System;
using System.Threading.Tasks;

namespace QueuesFunctions
{
    public static class HttpRegister
    {
        [FunctionName("HttpRegister")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"HttpRegister function processed");
            var body = await req.GetBodyAsync<UserRequest>();

            var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");            
            var service = new StorageQueueService(connectionString, "");

            await service.Insert(JsonConvert.SerializeObject(body.Value));
            return new OkResult();
        }
    }
}

