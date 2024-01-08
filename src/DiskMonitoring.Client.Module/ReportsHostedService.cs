using DiskMonitoring.Client.Infrastructure.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client;

public class ReportsHostedService : IHostedService
{
    private readonly IMessageHubProxy _messageHubProxy;
    private readonly ILogger<ReportsHostedService> _logger;

    public ReportsHostedService(
        IMessageHubProxy messageHubProxy,
        ILogger<ReportsHostedService> logger)
    {
        _messageHubProxy = messageHubProxy;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting reports service");

        await _messageHubProxy.InitiateConenction();

        while (!cancellationToken.IsCancellationRequested)
        {
            await _messageHubProxy.SendReport("test"); // todo

            await Task.Delay(TimeSpan.FromSeconds(59)); // todo - add to configuration
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _messageHubProxy.Dispose();

        _logger.LogInformation($"Stopping reports service");
    }
}
