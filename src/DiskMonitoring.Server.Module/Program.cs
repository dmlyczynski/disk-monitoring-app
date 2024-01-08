using DiskMonitoring.Server.Infrastructure;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog();
builder.Services.AddSignalR();
builder.Services.AddLogging();

builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.MapHub<ReportsHub>("reports");

app.Run();