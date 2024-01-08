using Microsoft.Win32.SafeHandles;

namespace DiskMonitoring.Client.Core.DeviceIoControll;

public class DiskDevice : DeviceObject
{
    public string DevicePath { get; }

    protected internal DiskDevice(KeyValuePair<string, SafeFileHandle> deviceNameAndHandle, FileAccess accessMode)
        : base(deviceNameAndHandle.Value, accessMode)
    {
        DevicePath = deviceNameAndHandle.Key;
    }

    public DiskDevice(string devicePath, FileAccess accessMode)
        : base(devicePath, accessMode)
    {
        this.DevicePath = devicePath;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}
