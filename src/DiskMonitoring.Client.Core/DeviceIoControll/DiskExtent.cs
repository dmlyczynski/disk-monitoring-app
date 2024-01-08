namespace DiskMonitoring.Client.Core.DeviceIoControll;

public readonly struct DiskExtent
{
    public uint DiskNumber { get; }
    public long StartingOffset { get; }
    public long ExtentLength { get; }
}
