using System.ComponentModel;
using System.Runtime.InteropServices;

using LTRData.Extensions.Buffers;

using Microsoft.Win32.SafeHandles;

namespace DiskMonitoring.Client.Core.DeviceIoControll;

public interface INativeDiskService
{
    DiskExtent[]? GetVolumeDiskExtents(string volumeGuid);
}

public class NativeDiskService : INativeDiskService
{
    public const uint IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x560000U;

    public DiskExtent[]? GetVolumeDiskExtents(string volumeGuid)
    {
        using var volume = new DiskDevice(volumeGuid.TrimEnd('\\'), 0);

        try
        {
            return GetVolumeDiskExtents(volume.SafeFileHandle);
        }
        catch (Win32Exception ex) when (ex.NativeErrorCode == NativeConstants.ERROR_INVALID_FUNCTION)
        {
            throw new Exception(nameof(GetVolumeDiskExtents), ex);
        }
    }

    private DiskExtent[] GetVolumeDiskExtents(SafeFileHandle volume)
    {
        const int outdatasize = 776;

        var buffer = DeviceIoControl(volume, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, default, outdatasize);

        return MemoryMarshal.Cast<byte, DiskExtent>(buffer.Slice(8)).ToArray();
    }

    private static Span<byte> DeviceIoControl(SafeFileHandle device, uint ctrlcode, Span<byte> data, int outdatasize)
    {
        var indata = (ReadOnlySpan<byte>)data;

        var indatasize = indata.Length;

        if (outdatasize > indatasize)
        {
            data = new byte[outdatasize];
        }

        var rc = UnsafeNativeMethods.DeviceIoControl(device,
                                                     ctrlcode,
                                                     indata.AsRef(),
                                                     indatasize,
                                                     out data.AsRef(),
                                                     data.Length,
                                                     out outdatasize,
                                                     0);

        if (!rc)
        {
            throw new Win32Exception();
        }

        return data.Slice(0, outdatasize);
    }
}