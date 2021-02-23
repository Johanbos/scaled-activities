using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace JohanBos.ScaledActivities
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
            var commands = new List<string>();
            for (int index = 0; index < amount; index++)
            {
                commands.Add(this.CreateMessage(index));
            }

            await this.commandStore.Start(commands);
            return new OkObjectResult("Done");
        }

        private string CreateMessage(int index)
        {
            return string.Concat(DateTime.UtcNow.Ticks, "-", index);
        }
    }
}