using System.Text;
using System.Text.Json;
using DiskMonitoring.Client.Core.DeviceIoControll;
using DiskMonitoring.Client.Core.Volumes;
using Microsoft.Extensions.Logging;

namespace DiskMonitoring.Client.Module;

public interface IReportService
{
    string CalculateReport();
}

internal class ReportService : IReportService
{
    private readonly ILogger<ReportService> _logger;

    public ReportService(ILogger<ReportService> logger)
    {
        _logger = logger;
    }

    public string CalculateReport()
    {
        StringBuilder stringBuilder = new();

        var list = VolumeServiceHelper.EnumerateVolumes();

        foreach (var item in list)
        {
            stringBuilder.AppendLine($"Volume: {item}");

            var list2 = NativeDiskServiceHelper.GetVolumeDiskExtents(item);

            for (int i = 0; i < list2?.Length; i++)
            {
                DiskExtent file = list2[i];
                stringBuilder.AppendLine($"DiskExtent: {JsonSerializer.Serialize(file)}");
            }
        }

        var result = stringBuilder.ToString();

        _logger.LogInformation("Calculate report:\r\n{0}", result);

        return result;
    }

}


