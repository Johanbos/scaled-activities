using System.Text.Json;
using JohanBos.ScaledActivities.Function.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace JohanBos.ScaledActivities.Function
{
    public class ProcessActivity
    {
        private readonly ILogger<ProcessActivity> logger;

        public ProcessActivity(ILogger<ProcessActivity> logger)
        {
            this.logger = logger;
        }

        [FunctionName("process-activity")]
        public void Run([QueueTrigger(QueueActivityStore.QueueName)] string myQueueItem)
        {
            var activity = JsonSerializer.Deserialize<Activity>(myQueueItem);
            this.logger.LogInformation($"{this.GetType().FullName} processed {activity.Id}");
        }
    }
}