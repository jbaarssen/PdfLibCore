using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable all
namespace CppSharp.Runtime;

// HACK: .NET Standard 2.0 which we use in auto-building to support .NET Framework, lacks UnmanagedType.LPUTF8Str
#pragma warning disable S101, S1186, CS0436
public class UTF8Marshaller : ICustomMarshaler
{
    private static UTF8Marshaller _marshaler;

    public void CleanUpManagedData(object ManagedObj)
    {
    }

    public void CleanUpNativeData(IntPtr pNativeData) =>
        Marshal.FreeHGlobal(pNativeData);

    public int GetNativeDataSize() => -1;

    public IntPtr MarshalManagedToNative(object ManagedObj)
    {
        if (ManagedObj == null)
        {
            return IntPtr.Zero;
        }
        if (ManagedObj is not string obj)
        {
            throw new MarshalDirectiveException($"{nameof(UTF8Marshaller)} must be used on a string.");
        }

        // not null terminated
        var strbuf = Encoding.UTF8.GetBytes(obj);
        var buffer = Marshal.AllocHGlobal(strbuf.Length + 1);
        Marshal.Copy(strbuf, 0, buffer, strbuf.Length);

        // write the terminating null
        Marshal.WriteByte(buffer + strbuf.Length, 0);
        return buffer;
    }

    public unsafe object MarshalNativeToManaged(IntPtr pNativeData) =>
        MarshalUtil.GetString(Encoding.UTF8, pNativeData);

    public static ICustomMarshaler GetInstance(string pstrCookie) =>
        _marshaler ??= new UTF8Marshaller();
}
#pragma warning restore