using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace JohanBos.ScaledActivities.Function
{
    public class Clear
    {
        private readonly ICommandStore commandStore;

        public Clear(ICommandStore commandStore)
        {
            this.commandStore = commandStore;
        }

        [FunctionName("clear")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed for Function discovery")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest httpRequest)
        {
            await this.commandStore.Clear();
            return new OkObjectResult("Done");
        }
    }
}