using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DiskMonitoring.Server.Infrastructure;

public class ReportsHub : Hub
{
    private readonly ILogger<ReportsHub> _logger;

    public ReportsHub(ILogger<ReportsHub> logger)
    {
        _logger = logger;
    }

    public void SendReport(string value)
    {
        _logger.LogInformation(value);
    }
}