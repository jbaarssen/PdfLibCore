using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable all
namespace CppSharp.Runtime;

public unsafe static class MarshalUtil
{
    public static string GetString(Action<IntPtr> action, Encoding encoding = null)
    {
        var buffer = GCHandle.Alloc(Array.Empty<byte>(), GCHandleType.Pinned);
        try
        {
            var ptr = buffer.AddrOfPinnedObject();
            action(ptr);
            return GetString(encoding ?? Encoding.UTF8, ptr);
        }
        finally
        {
            buffer.Free();
        }
    }

    public static string GetString(Encoding encoding, IntPtr str)
    {
        if (str == IntPtr.Zero)
        {
            return null;
        }

        var byteCount = encoding switch
        {
            { } when Encoding.UTF32.Equals(encoding) => CountBytes<int>((int*) str),
            { } when Encoding.Unicode.Equals(encoding) => CountBytes<short>((short*) str),
            { } when Encoding.BigEndianUnicode.Equals(encoding) => CountBytes<short>((short*) str),
            _ => CountBytes<byte>((byte*) str)
        };

        return (encoding ?? Encoding.UTF8).GetString((byte*) str, byteCount);
    }

    public static T[] GetArray<T>(void* array, int size) where T : unmanaged
    {
        if (array == null)
        {
            return Array.Empty<T>();
        }
        var result = new T[size];
        fixed (void* fixedResult = result)
        {
            Buffer.MemoryCopy(array, fixedResult, sizeof(T) * size, sizeof(T) * size);
        }
        return result;
    }

    public static char[] GetCharArray(sbyte* array, int size)
    {
        if (array == null)
        {
            return Array.Empty<char>();
        }
        var result = new char[size];
        for (var i = 0; i < size; ++i)
        {
            result[i] = Convert.ToChar(array[i]);
        }
        return result;
    }

    public static IntPtr[] GetIntPtrArray(IntPtr* array, int size) =>
        GetArray<IntPtr>(array, size);

    public static T GetDelegate<T>(IntPtr[] vtables, short table, int i) where T : class =>
        Marshal.GetDelegateForFunctionPointer<T>(*(IntPtr*) (vtables[table] + i * sizeof(IntPtr)));

    public static unsafe int CountBytes<T>(T* str) where T : unmanaged
    {
        int byteCount = 0;
        T nullValue = default(T);

        while (!EqualityComparer<T>.Default.Equals(*str, nullValue))
        {
            byteCount += sizeof(T);
        }

        return byteCount;
    }
}