using DiskMonitoring.Client.Infrastructure;
using DiskMonitoring.Client.Infrastructure.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiskMonitoring.Client.Module;

public class ReportHostedService : IHostedService
{
    private readonly IMessageHubProxy _messageHubProxy;
    private readonly ILogger<ReportHostedService> _logger;
    private readonly IReportService _reportService;
    private readonly ConfigSettings _configSettings;

    public ReportHostedService(
        IMessageHubProxy messageHubProxy,
        ILogger<ReportHostedService> logger,
        IReportService reportService,
        IOptions<ConfigSettings> options)
    {
        _logger = logger;
        _configSettings = options.Value;
        _reportService = reportService;
        _messageHubProxy = messageHubProxy;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting report service");

        await _messageHubProxy.InitiateConenction();

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = _reportService.CalculateReport();

            await _messageHubProxy.SendReport(message);

            await Task.Delay(TimeSpan.FromMinutes(_configSettings.REPORT_JOB_SCHEDULE_TIME), cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _messageHubProxy.Dispose();

        _logger.LogInformation($"Stopping report service");
    }
}
