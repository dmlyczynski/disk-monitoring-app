using Client;
using DiskMonitoring.Client.Infrastructure.SignalR;
using DiskMonitoring.Client.Module;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true)
    .CreateLogger();

try
{
    await Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.AddSerilog();
        })
        .ConfigureServices(services =>
        {
            services.AddTransient<IMessageHubProxy, MessageHubProxy>();
            services.AddTransient<IReportService, ReportService>();
            services.AddHostedService<ReportHostedService>();
        })
        .Build()
        .RunAsync();
}
catch (Exception ex)
{
    Log.Logger.Error("Error occured", ex);
}