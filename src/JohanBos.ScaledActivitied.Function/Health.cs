using System;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Options;

namespace JohanBos.ScaledActivities.Function
{
    public class Health
    {
        public Health(IOptions<AppSettings> options)
        {
            this.Settings = options.Value;
        }

        private AppSettings Settings { get; }

        [FunctionName("health")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest httpRequest)
        {
            return new OkObjectResult(this.Settings.Johan);
        }
    }
}