using System;
using JohanBos.ScaledActivities.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(JohanBos.ScaledActivities.Startup))]

namespace JohanBos.ScaledActivities
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddOptions<AppSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.Bind(settings));

            builder.Services.AddTransient<ICommandStore, QueueCommandStore>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            _ = builder.ConfigurationBuilder
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile($"appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>();
        }
    }
}