using System;
using System.Runtime.InteropServices;
using System.Text;
using PdfLibCore.Generated;

namespace PdfLibCore.Types;

public abstract class NativeWrapper<T> : IDisposable
    where T : class, ISafePointer
{
    private readonly T _handle;

    /// <summary>
    /// Handle which can be used with the native <see cref="Generated.Pdfium"/> functions.
    /// </summary>
    public T Handle => IsDisposed ? throw new ObjectDisposedException(GetType().FullName) : _handle;

    /// <summary>
    /// Gets a value indicating whether <see cref="IDisposable.Dispose"/> was already
    /// called on this instance.
    /// </summary>
    public bool IsDisposed => _handle.IsNull();

    protected NativeWrapper(T handle) =>
        _handle = handle.IsNull() ? throw new PdfiumException() : handle;

    /// <summary>
    /// Implementors should clean up here. This method is guaranteed to only be called once.
    /// </summary>
    protected virtual void OnDispose(T handle)
    {
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !_handle.IsNull())
        {
            OnDispose(_handle);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}