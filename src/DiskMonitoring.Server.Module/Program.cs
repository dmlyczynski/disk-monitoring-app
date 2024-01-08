using DiskMonitoring.Server.Infrastructure;

using Microsoft.Extensions.Configuration;

using Serilog;

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

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Bind(configuration);
builder.Services.Configure<ConfigSettings>(builder.Configuration);

builder.Logging.AddSerilog();
builder.Services.AddSignalR();
builder.Services.AddLogging();

builder.WebHost.UseUrls(urls: builder.Configuration[nameof(ConfigSettings.WEB_HOST_URL)]!);

var app = builder.Build();

app.MapHub<ReportsHub>("reports");

app.Run();