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
public struct FPDF_ANNOTATION : IHandle<FPDF_ANNOTATION>
{
    private IntPtr _pointer;
    public bool IsNull => _pointer == IntPtr.Zero;
    public static FPDF_ANNOTATION Null => new();
    private FPDF_ANNOTATION(IntPtr ptr) => _pointer = ptr;
    FPDF_ANNOTATION IHandle<FPDF_ANNOTATION>.SetToNull() => new(Interlocked.Exchange(ref _pointer, IntPtr.Zero));
    public override string ToString() => $"FPDF_ANNOTATION: 0x{_pointer:X16}";
}