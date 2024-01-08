using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace DiskMonitoring.Client.Core.DeviceIoControll;

public static partial class UnsafeNativeMethods
{
    [LibraryImport("kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    internal static partial SafeFileHandle CreateFileW(in char lpFileName, FileSystemRights dwDesiredAccess, FileShare dwShareMode, nint lpSecurityAttributes, uint dwCreationDisposition, int dwFlagsAndAttributes, nint hTemplateFile);


    [LibraryImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool DeviceIoControl(SafeFileHandle hDevice, uint dwIoControlCode, in byte lpInBuffer, int nInBufferSize, out byte lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, nint lpOverlapped);
}
