using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Threading;

namespace JohanBos.ScaledActivities.Function
{
    internal abstract class QueueStore
    {
        private readonly AsyncLazy<QueueClient> lazyQueueClient;
        private readonly string queueName;

        public QueueStore(ILogger logger, IOptions<AppSettings> options, string queueName)
        {
            this.Settings = options.Value;
            this.Logger = logger;
            this.queueName = queueName;
            this.lazyQueueClient = new AsyncLazy<QueueClient>(() => this.InitializeQueueClient(), null);
        }

        protected AppSettings Settings { get; private set; }

        protected ILogger Logger { get; private set; }

        protected Task<QueueClient> QueueClient => this.lazyQueueClient.GetValueAsync();

        private async Task<QueueClient> InitializeQueueClient()
        {
            var connectionString = this.Settings.AzureWebJobsStorage ?? throw new ArgumentNullException("settings.ConnectionStrings.Storage");
            this.Logger.LogInformation($"{this.GetType().FullName} InitializeQueueClient {this.queueName}");
            var options = new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64,
            };
            var queueClient = new QueueClient(connectionString, this.queueName, options);
            await queueClient.CreateIfNotExistsAsync();
            return queueClient;
        }
    }
}