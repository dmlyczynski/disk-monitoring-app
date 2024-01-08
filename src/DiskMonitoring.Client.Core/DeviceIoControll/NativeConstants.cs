namespace DiskMonitoring.Client.Core.DeviceIoControll;

public static class NativeConstants
{
    public const string SUPPORTED_WINDOWS_PLATFORM = "windows";

    public const FileOptions FILE_FLAG_OVERLAPPED = (FileOptions)0x40000000;

    public const int VOLUME_NAME_DOS = 0x0;
    public const int VOLUME_NAME_GUID = 0x1;
    public const int VOLUME_NAME_NONE = 0x4;
    public const int VOLUME_NAME_NT = 0x2;
    public const int FILE_NAME_NORMALIZED = 0x0;
    public const int FILE_NAME_OPENED = 0x8;

    public const uint OPEN_ALWAYS = 4U;
    public const uint OPEN_EXISTING = 3U;
    public const uint CREATE_ALWAYS = 2U;
    public const uint CREATE_NEW = 1U;
    public const uint TRUNCATE_EXISTING = 5U;

    public const int NO_ERROR = 0;
    public const int ERROR_INVALID_FUNCTION = 1;
}