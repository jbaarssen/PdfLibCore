using System;
using System.Threading;

namespace PdfLibCore.Parser;

//public struct FPDF_ACTION : IHandle<FPDF_ACTION>
//{
//    private IntPtr _pointer;
//    public bool IsNull => _pointer == IntPtr.Zero;
//    public static FPDF_ACTION Null => new();
//    private FPDF_ACTION(IntPtr ptr) => _pointer = ptr;
//    FPDF_ACTION IHandle<FPDF_ACTION>.SetToNull() => new(Interlocked.Exchange(ref _pointer, IntPtr.Zero));
//    public override string ToString() => $"FPDF_ANNOTATION: 0x{_pointer:X16}";
//}