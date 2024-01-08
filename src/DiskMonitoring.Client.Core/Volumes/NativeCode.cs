using System.Runtime.InteropServices;
using System.Text;

namespace DiskMonitoring.Client.Core.Volumes;

public static class VolumeServiceHelper
{
    internal const int MaxPathUnicode = 32000;

    public static IEnumerable<string> EnumerateVolumes()
    {
        var buffer = new StringBuilder(MaxPathUnicode);

        using var handle = NativeFindVolumeMethods.FindFirstVolume(buffer, (uint)buffer.Capacity);

        while (handle != null && !handle.IsInvalid)
        {
            if (NativeFindVolumeMethods.FindNextVolume(handle, buffer, (uint)buffer.Capacity))
            {
                yield return buffer.ToString();
            }
            else
            {
                var lastError = Marshal.GetLastWin32Error();

                handle.Close();

                if (lastError == Win32Errors.ERROR_NO_MORE_FILES)
                    yield break;
            }
        }
    }
}
