using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PdfLibCore.Helpers;

internal  static class Helper
{
    internal static string GetString(Func<IntPtr, uint, uint> func, int lengthUnit = 1, bool lengthIncludesTerminator = true)
    {
        var buffer = GetBuffer(func, out var length, Array.Empty<byte>());
        length *= lengthUnit;
        if (lengthIncludesTerminator)
        {
            length -= 2;
        }
        return Encoding.Unicode.GetString(buffer, 0, length > 0 ? length : 0);
    }

    private static T GetBuffer<T>(Func<IntPtr, uint, uint> func, out int length, T defaultValue)
    {
        const byte b = 0;
        var buffer = GCHandle.Alloc(b, GCHandleType.Pinned);
        try
        {
            length = (int)func(buffer.AddrOfPinnedObject(), 0);
            if (length == 0)
            {
                return defaultValue;
            }
        }
        finally
        {
            buffer.Free();
        }

        buffer = GCHandle.Alloc(new byte[length], GCHandleType.Pinned);
        try
        {
            func(buffer.AddrOfPinnedObject(), (uint) length);
            return (T) buffer.Target;
        }
        finally
        {
            buffer.Free();
        }
    }
}