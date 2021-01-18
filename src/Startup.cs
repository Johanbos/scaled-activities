using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(JohanBos.ScaledActivities.Startup))]

namespace JohanBos.ScaledActivities
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var context = serviceProvider.GetService<IOptions<ExecutionContextOptions>>().Value;
            var configuration = new ConfigurationBuilder()
                .AddConfiguration(serviceProvider.GetService<IConfiguration>())
                .SetBasePath(context.AppDirectory)
                .AddJsonFile($"appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>()
                .Build();

            builder.Services.AddOptions<AppSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.Bind(settings));
        }
    }
}