using DiskMonitoring.Client.Core.DeviceIoControll;
using DiskMonitoring.Client.Core.Volumes;
using DiskMonitoring.Client.Infrastructure;
using DiskMonitoring.Client.Infrastructure.SignalR;
using DiskMonitoring.Client.Module;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

try
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File("logs/log.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
        .CreateLogger();

    var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddJsonFile("appsettings.json")
        .Build();

    await Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(app =>
        {
            app.AddConfiguration(configuration);
        })
        .ConfigureLogging(logging =>
        {
            logging.AddSerilog();
        })
        .ConfigureServices((context, services) =>
        {            
            context.Configuration.Bind(configuration);
            services.Configure<ConfigSettings>(context.Configuration);

            services.AddTransient<INativeDiskService, NativeDiskService>();
            services.AddTransient<IVolumeService, VolumeService>();
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
    throw;
}