using System;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace JohanBos.ScaledActivities.Function
{
    public class View
    {
        private const string QSMaxMessages = "maxmessages";
        private readonly ICommandStore commandStore;

        public View(ICommandStore commandStore)
        {
            this.commandStore = commandStore;
        }

        [FunctionName("view")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest httpRequest)
        {
            int? maxMessages = null;
            if (httpRequest.Query.ContainsKey(QSMaxMessages))
            {
                maxMessages = Convert.ToInt32(httpRequest.Query[QSMaxMessages]);
            }

            var messages = await this.commandStore.View(maxMessages);
            return new OkObjectResult(messages);
        }
    }
}