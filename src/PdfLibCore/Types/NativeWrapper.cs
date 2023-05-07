using System;

namespace PdfLibCore.Types;

public class NativeDocumentWrapper<T> : NativeWrapper<T>
    where T : struct, IHandle<T>
{
    protected PdfDocument Document { get; }

    protected NativeDocumentWrapper(PdfDocument document, T handle)
        : base(handle) =>
        Document = document ?? throw new PdfiumException();
}

public class NativeWrapper<T> : IDisposable
    where T : struct, IHandle<T>
{
    private T _handle;

    /// <summary>
    /// Handle which can be used with the native <see cref="Pdfium"/> functions.
    /// </summary>
    public T Handle => IsDisposed ? throw new ObjectDisposedException(GetType().FullName) : _handle;

    /// <summary>
    /// Gets a value indicating whether <see cref="IDisposable.Dispose"/> was already
    /// called on this instance.
    /// </summary>
    public bool IsDisposed => _handle.IsNull;

    protected NativeWrapper(T handle) =>
        _handle = handle.IsNull ? throw new PdfiumException() : handle;

    /// <summary>
    /// Implementors should clean up here. This method is guaranteed to only be called once.
    /// </summary>
    protected virtual void Dispose(T handle)
    {
    }

    void IDisposable.Dispose()
    {
        var oldHandle = _handle.SetToNull();
        if (!oldHandle.IsNull)
        {
            Dispose(oldHandle);
        }
    }
}