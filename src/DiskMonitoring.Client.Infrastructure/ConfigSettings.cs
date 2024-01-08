namespace DiskMonitoring.Client.Infrastructure;

public class ConfigSettings
{
    public string HUB_CONNECTION_URL { get; set; } = null!;
    public int REPORT_JOB_SCHEDULE_TIME { get; set; } = 59;
}