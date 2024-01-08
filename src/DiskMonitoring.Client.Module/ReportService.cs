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

public class ReportService : IReportService
{
    private readonly ILogger<ReportService> _logger;
    private readonly IVolumeService _volumeService;
    private readonly INativeDiskService _nativeDiskService;

    public ReportService(
        ILogger<ReportService> logger, 
        IVolumeService volumeService, 
        INativeDiskService nicService)
    {
        _logger = logger;
        _volumeService = volumeService;
        _nativeDiskService = nicService;
    }

    public string CalculateReport()
    {
        StringBuilder stringBuilder = new();

        var list = _volumeService.EnumerateVolumes();

        foreach (var item in list)
        {
            stringBuilder.AppendLine($"Volume: {item}");

            var volumeDiskExtents = _nativeDiskService.GetVolumeDiskExtents(item);

            for (int i = 0; i < volumeDiskExtents?.Length; i++)
            {
                DiskExtent file = volumeDiskExtents[i];
                stringBuilder.AppendLine($"DiskExtent: {JsonSerializer.Serialize(file)}");
            }
        }

        var result = stringBuilder.ToString();

        _logger.LogInformation("Calculate report:\r\n{0}", result);

        return result;
    }
}


