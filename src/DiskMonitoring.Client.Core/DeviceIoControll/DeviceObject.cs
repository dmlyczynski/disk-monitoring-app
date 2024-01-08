using Microsoft.Win32.SafeHandles;

namespace DiskMonitoring.Client.Core.DeviceIoControll;

public abstract class DeviceObject : IDisposable
{
    public SafeFileHandle SafeFileHandle { get; }

    public FileAccess AccessMode { get; }

    protected DeviceObject(string path, FileAccess accessMode)
        : this(OpenFileHandleHelper.OpenFileHandle(path, accessMode, FileShare.ReadWrite, FileMode.Open, Overlapped: false), accessMode)
    {
    }

    protected DeviceObject(SafeFileHandle handle, FileAccess access)
    {
        SafeFileHandle = handle;
        AccessMode = access;
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                SafeFileHandle?.Dispose();
            }
        }

        disposedValue = true;
    }

    ~DeviceObject()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}