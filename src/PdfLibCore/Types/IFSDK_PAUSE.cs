using System;
using System.Runtime.InteropServices;

// ReSharper disable NotAccessedField.Local
namespace PdfLibCore.Generated.Structs;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
[StructLayout(LayoutKind.Sequential)]
public partial struct IFSDK_PAUSE
{
    [MarshalAs(UnmanagedType.FunctionPtr)]
    private readonly Func<IntPtr, bool> _needToPauseCore;
}