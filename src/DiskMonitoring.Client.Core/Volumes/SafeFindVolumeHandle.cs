using Microsoft.Win32.SafeHandles;

namespace DiskMonitoring.Client.Core.Volumes;

public sealed class SafeFindVolumeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeFindVolumeHandle()
       : base(true)
    {
    }

    public SafeFindVolumeHandle(IntPtr handle, bool callerHandle) : base(callerHandle)
    {
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        return NativeFindVolumeMethods.FindVolumeClose(handle);
    }
}
