// ReSharper disable once CheckNamespace

namespace System.Runtime.InteropServices;

public static class Unmanaged
{
    public static TReturn WithHandle<T, TReturn>(T managedObject, Func<IntPtr, T, TReturn> func)
    {
        var ptr = GCHandle.Alloc(managedObject, GCHandleType.Pinned);
        try
        {
            return func(ptr.AddrOfPinnedObject(), (T) ptr.Target);
        }
        finally
        {
            ptr.Free();
        }
    }
}