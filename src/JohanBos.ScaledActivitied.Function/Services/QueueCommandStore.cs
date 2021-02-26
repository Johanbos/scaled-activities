using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Interfaces;
using JohanBos.ScaledActivities.Function.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JohanBos.ScaledActivities.Function
{
    internal class QueueCommandStore : QueueStore, ICommandStore
    {
        public const string QueueName = "commands";

        public QueueCommandStore(ILogger<QueueCommandStore> logger, IOptions<AppSettings> options)
            : base(logger, options, QueueName)
        {
        }

        public async Task Start(IEnumerable<Command> commands)
        {
            var queueClient = await this.QueueClient;
            this.Logger.LogInformation($"{this.GetType().FullName} started on {queueClient.Name}");
            var tasks = new List<Task>();
            foreach (var command in commands)
            {
                var serialized = JsonSerializer.Serialize(command);
                tasks.Add(queueClient.SendMessageAsync(serialized));
            }

            await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<Command>> View(int? maxMessages)
        {
            var queueClient = await this.QueueClient;
            this.Logger.LogInformation($"{this.GetType().FullName} viewed {queueClient.Name}");
            var messages = await queueClient.PeekMessagesAsync(maxMessages);
            return messages.Value
                .ToList()
                .ConvertAll(peekedMessage => peekedMessage.Body.ToObjectFromJson<Command>());
        }

        public async Task Clear()
        {
            var queueClient = await this.QueueClient;
            this.Logger.LogInformation($"{this.GetType().FullName} cleared {queueClient.Name}");
            await queueClient.ClearMessagesAsync();
        }
    }
}