using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using JohanBos.ScaledActivities.Function.Interfaces;
using JohanBos.ScaledActivities.Function.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JohanBos.ScaledActivities.Function
{
    internal class QueueCommandStore : ICommandStore
    {
        private readonly AppSettings settings;
        private readonly ILogger<Start> logger;

        public QueueCommandStore(ILogger<Start> logger, IOptions<AppSettings> options)
        {
            this.settings = options.Value;
            this.logger = logger;
        }

        public async Task Start(IEnumerable<string> commands)
        {
            this.logger.LogInformation($"{typeof(QueueCommandStore).FullName} started {{queueName}}", this.settings.QueueName);
            var queueClient = new QueueClient(this.settings.ConnectionStrings.Storage, this.settings.QueueName);
            await queueClient.CreateIfNotExistsAsync();
            var tasks = new List<Task>();
            foreach (var command in commands)
            {
                tasks.Add(queueClient.SendMessageAsync(command));
            }

            await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<Command>> View(int? maxMessages)
        {
            this.logger.LogInformation($"{typeof(QueueCommandStore).FullName} viewed {{queueName}}", this.settings.QueueName);
            var queueClient = new QueueClient(this.settings.ConnectionStrings.Storage, this.settings.QueueName);
            await queueClient.CreateIfNotExistsAsync();
            var messages = await queueClient.PeekMessagesAsync(maxMessages);
            return messages.Value.ToList().ConvertAll(peekedMessage => new Command
            {
                Message = peekedMessage.MessageText,
            });
        }

        public async Task Clear()
        {
            this.logger.LogInformation($"{typeof(QueueCommandStore).FullName} cleared {{queueName}}", this.settings.QueueName);
            var queueClient = new QueueClient(this.settings.ConnectionStrings.Storage, this.settings.QueueName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.ClearMessagesAsync();
        }
    }
}