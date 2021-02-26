using JohanBos.ScaledActivities.Function.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(JohanBos.ScaledActivities.Function.Startup))]

namespace JohanBos.ScaledActivities.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddOptions<AppSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.Bind(settings));

            builder.Services.AddTransient<ICommandStore, QueueCommandStore>();
            builder.Services.AddTransient<IActivityStore, QueueActivityStore>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();
            _ = builder.ConfigurationBuilder
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>();
        }
    }
}