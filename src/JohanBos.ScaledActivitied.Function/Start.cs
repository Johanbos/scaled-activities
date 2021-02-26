using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Interfaces;
using JohanBos.ScaledActivities.Function.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace JohanBos.ScaledActivities.Function
{
    public class Start
    {
        private const string QSMessages = "messages";
        private readonly ICommandStore commandStore;

        public Start(ICommandStore commandStore)
        {
            this.commandStore = commandStore;
        }

        [FunctionName("start")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest httpRequest)
        {
            var amount = httpRequest.Query.ContainsKey(QSMessages) ? Convert.ToInt32(httpRequest.Query[QSMessages]) : 32;
            var commands = this.CreateCommands(amount);
            await this.commandStore.Start(commands);
            return new OkObjectResult("Done");
        }

        private IEnumerable<Command> CreateCommands(int amount)
        {
            var startUtc = DateTime.UtcNow;
            for (int index = 0; index < amount; index++)
            {
                yield return new Command
                {
                    Id = string.Concat(startUtc.Ticks, "-", index),
                    StartUtc = startUtc,
                    ActivitiesGroup = index,
                    ActivitiesCount = 500,
                };
            }
        }
    }
}