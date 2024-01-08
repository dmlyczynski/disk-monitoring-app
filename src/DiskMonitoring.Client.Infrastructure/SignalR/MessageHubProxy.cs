using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;

namespace DiskMonitoring.Client.Infrastructure.SignalR;

public interface IMessageHubProxy
{
    Task SendReport(string message);
    Task InitiateConenction();
    Task Dispose();
}

public class MessageHubProxy : IMessageHubProxy
{
    private readonly HubConnection _hubConnection;
    private readonly IAsyncPolicy _retryPolicy;
    private readonly IAsyncPolicy _retryPolicySendReport;
    private readonly ILogger<MessageHubProxy> _logger;

    public MessageHubProxy(ILogger<MessageHubProxy> logger, IOptions<ConfigSettings> options)
    {
        _logger = logger;
        _hubConnection = new HubConnectionBuilder()
                .WithUrl(options.Value.HUB_CONNECTION_URL)
                .WithAutomaticReconnect()
                .Build();

        _retryPolicy = Policy
           .Handle<Exception>()
           .Or<TaskCanceledException>()
           .Or<InvalidOperationException>()
           .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(2), onRetry: (_, _, _) => { _logger.LogError("Error connecting to hub. Retrying ..."); })
           ;

        _retryPolicySendReport = Policy
           .Handle<InvalidOperationException>()
           .Or<TaskCanceledException>()
           .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(2), onRetry: async (_, _, _) => await InitiateConenction());
        ;
    }

    public async Task InitiateConenction()
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
            }
        });
    }

    public async Task SendReport(string message)
    {
        await _retryPolicySendReport.ExecuteAsync(() => _hubConnection.InvokeAsync("SendReport", message));

        _logger.LogInformation("Report sent");
    }

    public async Task Dispose()
    {
        await _hubConnection.DisposeAsync();
    }
}