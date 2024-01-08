using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace DiskMonitoring.Client.Core.Volumes;

internal static partial class NativeFindVolumeMethods
{
    [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FindVolumeClose(IntPtr hFindVolume);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstVolumeW"), SuppressUnmanagedCodeSecurity]
    internal static extern SafeFindVolumeHandle FindFirstVolume(StringBuilder lpszVolumeName, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindNextVolumeW"), SuppressUnmanagedCodeSecurity]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FindNextVolume(SafeFindVolumeHandle hFindVolume, StringBuilder lpszVolumeName, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);
}