using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Interfaces;
using JohanBos.ScaledActivities.Function.Models;
using Microsoft.Azure.WebJobs;

namespace JohanBos.ScaledActivities.Function
{
    public class ProcessCommand
    {
        private readonly IActivityStore activityStore;

        public ProcessCommand(IActivityStore activityStore)
        {
            this.activityStore = activityStore;
        }

        [FunctionName("process-command")]
        public async Task Run([QueueTrigger(QueueCommandStore.QueueName)] string myQueueItem)
        {
            var command = JsonSerializer.Deserialize<Command>(myQueueItem);
            var activities = this.CreateActivities(command);
            await this.activityStore.Add(activities);
        }

        private IEnumerable<Activity> CreateActivities(Command command)
        {
            var startUtc = DateTime.UtcNow;
            for (int index = 0; index < command.ActivitiesCount; index++)
            {
                yield return new Activity
                {
                    Id = string.Concat(command.Id, "-", index),
                    Group = command.ActivitiesGroup,
                    StartUtc = startUtc,
                    CommandStartUtc = command.StartUtc,
                };
            }
        }
    }
}