using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Interfaces;
using JohanBos.ScaledActivities.Function.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JohanBos.ScaledActivities.Function
{
    internal class QueueActivityStore : QueueStore, IActivityStore
    {
        public const string QueueName = "activities";

        public QueueActivityStore(ILogger<QueueActivityStore> logger, IOptions<AppSettings> options)
            : base(logger, options, QueueName)
        {
        }

        public async Task Add(IEnumerable<Activity> activities)
        {
            var queueClient = await this.QueueClient;
            this.Logger.LogInformation($"{this.GetType().FullName} add {queueClient.Name}");
            await queueClient.CreateIfNotExistsAsync();
            var tasks = new List<Task>();
            foreach (var activity in activities)
            {
                tasks.Add(queueClient.SendMessageAsync(JsonSerializer.Serialize(activity)));
            }

            await Task.WhenAll(tasks.AsParallel());
        }
    }
}