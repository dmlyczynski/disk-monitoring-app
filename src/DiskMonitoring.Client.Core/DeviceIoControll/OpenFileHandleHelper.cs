using LTRData.Extensions.Buffers;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace DiskMonitoring.Client.Core.DeviceIoControll;

public static class OpenFileHandleHelper
{
    /// <summary>
    /// Calls Win32 API CreateFile() platform specific
    /// </summary>
    public static SafeFileHandle OpenFileHandle(string FileName, FileAccess DesiredAccess, FileShare ShareMode, FileMode CreationDisposition, bool Overlapped)
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? OpenFileHandleInternal(FileName, DesiredAccess, ShareMode, CreationDisposition, Overlapped)
            : new FileStream(FileName, CreationDisposition, DesiredAccess, ShareMode, 1, Overlapped).SafeFileHandle;

    /// <summary>
    /// Calls Win32 API CreateFile() function and encapsulates returned handle in a SafeFileHandle object.
    /// </summary>
    private static SafeFileHandle OpenFileHandleInternal(string FileName, FileAccess DesiredAccess, FileShare ShareMode, FileMode CreationDisposition, bool Overlapped)
    {
        if (string.IsNullOrWhiteSpace(FileName))
        {
            throw new ArgumentNullException(nameof(FileName));
        }

        var nativeDesiredAccess = ConvertManagedFileAccess(DesiredAccess);

        var nativeCreationDisposition = CreationDisposition switch
        {
            FileMode.Create => NativeConstants.CREATE_ALWAYS,
            FileMode.CreateNew => NativeConstants.CREATE_NEW,
            FileMode.Open => NativeConstants.OPEN_EXISTING,
            FileMode.OpenOrCreate => NativeConstants.OPEN_ALWAYS,
            FileMode.Truncate => NativeConstants.TRUNCATE_EXISTING,
            _ => throw new NotImplementedException(),
        };

        var nativeFlagsAndAttributes = FileAttributes.Normal;

        if (Overlapped)
        {
            nativeFlagsAndAttributes |= (FileAttributes)NativeConstants.FILE_FLAG_OVERLAPPED;
        }

        var handle = UnsafeNativeMethods.CreateFileW(FileName.AsRef(),
                                                     nativeDesiredAccess,
                                                     ShareMode,
                                                     0,
                                                     nativeCreationDisposition,
                                                     (int)nativeFlagsAndAttributes,
                                                     0);

        return handle.IsInvalid
            ? throw new IOException($"Cannot open {FileName}", new Win32Exception())
            : handle;
    }


    [SupportedOSPlatform(NativeConstants.SUPPORTED_WINDOWS_PLATFORM)]
    private static FileSystemRights ConvertManagedFileAccess(FileAccess DesiredAccess)
    {
        var nativeDesiredAccess = FileSystemRights.ReadAttributes;

        if (DesiredAccess.HasFlag(FileAccess.Read))
        {
            nativeDesiredAccess |= FileSystemRights.Read;
        }

        if (DesiredAccess.HasFlag(FileAccess.Write))
        {
            nativeDesiredAccess |= FileSystemRights.Write;
        }

        return nativeDesiredAccess;
    }
}
