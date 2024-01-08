using DiskMonitoring.Client.Infrastructure.SignalR;
using DiskMonitoring.Client.Module;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client;

public class ReportHostedService : IHostedService
{
    private readonly IMessageHubProxy _messageHubProxy;
    private readonly ILogger<ReportHostedService> _logger;
    private readonly IReportService _reportService;

    public ReportHostedService(
        IMessageHubProxy messageHubProxy,
        ILogger<ReportHostedService> logger,
        IReportService reportService)
    {        
        _logger = logger;
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

            await Task.Delay(TimeSpan.FromSeconds(59)); // todo - add to configuration
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _messageHubProxy.Dispose();

        _logger.LogInformation($"Stopping report service");
    }
}
