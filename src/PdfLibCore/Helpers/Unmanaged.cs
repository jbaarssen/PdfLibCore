// ReSharper disable once CheckNamespace

namespace System.Runtime.InteropServices;

public static class Unmanaged
{
    public static void WithHandle<T>(T managedObject, Action<IntPtr, T> action) => WithHandle(managedObject, (ptr, target) =>
    {
        action(ptr, target);
        return true;
    });

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