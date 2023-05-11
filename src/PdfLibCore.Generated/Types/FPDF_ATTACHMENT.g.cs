using System;
using System.Threading;
using PdfLibCore.Generated.Interfaces;

/*
This file is part of PdfLibCore, a wrapper around the PDFium library for the .NET.
Inspired by the awesome work of PDFiumSharp by Tobias Meyer.

Copyright (C) 2023 J.C.A. Kokenberg & Jan Baarssen
License: Microsoft Reciprocal License (MS-RL)
*/

// AUTOGENERATED FILE - 10-05-2023 (01:50:33) - Utc
// DO NOT MODIFY
// Sourcefile: https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h

namespace PdfLibCore.Generated.Types;
public struct FPDF_ATTACHMENT : IHandle<FPDF_ATTACHMENT>
{
    private IntPtr _pointer;
    public bool IsNull => _pointer == IntPtr.Zero;
    public static FPDF_ATTACHMENT Null => new();
    private FPDF_ATTACHMENT(IntPtr ptr) => _pointer = ptr;
    FPDF_ATTACHMENT IHandle<FPDF_ATTACHMENT>.SetToNull() => new(Interlocked.Exchange(ref _pointer, IntPtr.Zero));
    public override string ToString() => $"FPDF_ATTACHMENT: 0x{_pointer:X16}";
}