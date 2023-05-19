using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable all
namespace CppSharp.Runtime;

public unsafe static class MarshalUtil
{
    public static string GetString(Func<IntPtr, uint, uint> func, int lengthUnit = 1, bool lengthIncludesTerminator = true)
    {
        var buffer = GetBuffer(func, out var length);
        length *= lengthUnit;
        if (lengthIncludesTerminator)
        {
            length -= 2;
        }
        return GetString(buffer, length);
    }

    public static string GetString(byte[] target, int size) =>
        size > 0 ? Encoding.Unicode.GetString(target, 0, size) : null;

    public static string GetString(Encoding encoding, IntPtr str)
    {
        if (str == IntPtr.Zero)
        {
            return null;
        }

        var bytes = Marshal.PtrToStructure<byte[]>(str);

        return (encoding ?? Encoding.UTF8).GetString((byte*) str, encoding switch
        {
            { } when Encoding.UTF32.Equals(encoding) => CountBytes<int>((int*) str),
            { } when Encoding.Unicode.Equals(encoding) => CountBytes<short>((short*) str),
            { } when Encoding.BigEndianUnicode.Equals(encoding) => CountBytes<short>((short*) str),
            _ => CountBytes<byte>((byte*) str)
        });
    }

    private unsafe static byte[] GetBuffer(Func<IntPtr, uint, uint> func, out int length)
    {
        fixed (byte* bufferPointer = Array.Empty<byte>())
        {
            length = (int) func(new IntPtr(bufferPointer), 0);
        }

        var buffer = new byte[length];
        fixed (byte* pointer = buffer)
        {
            length = (int) func(new IntPtr(pointer), (uint) buffer.Length);
        }
        return buffer;
    }

    private static unsafe int CountBytes<T>(T* str) where T : unmanaged
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