using System.Configuration;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JohanBos.ScaledActivities.Queue
{
    public static class Start
    {
        private static AppSettings settings;

        [FunctionName("start-queue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest httpRequest,
            ILogger log,
            IOptions<AppSettings> options)
        {
            settings = options.Value;
            log.LogInformation("{function} started with {queueName}", typeof(Start).FullName, settings.QueueName);
            var queueClient = new QueueClient(settings.ConnectionStrings.Storage, settings.QueueName);
            await queueClient.CreateIfNotExistsAsync();
            var exists = await queueClient.ExistsAsync();
            if (!exists)
            {
                throw new System.Exception("Queue does not exist");
            }

            await queueClient.SendMessageAsync("Hello World");
            var responseMessage = "Done";
            return new OkObjectResult(responseMessage);
        }
    }
}