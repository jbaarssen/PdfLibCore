using System;

// ReSharper disable once CheckNamespace
namespace PdfLibCore.Generated;

public static class SafePointerExtensions
{
    public static bool IsNull(this ISafePointer pointer) =>
        pointer == null || pointer.__Instance == IntPtr.Zero || pointer.__Instance == new IntPtr(-1);
}