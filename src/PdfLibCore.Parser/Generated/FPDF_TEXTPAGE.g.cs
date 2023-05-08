using System;
using System.Threading;

/*
This file is part of PdfLibCore, a wrapper around the PDFium library for the .NET.
Inspired by the awesome work of PDFiumSharp by Tobias Meyer.

Copyright (C) 2023 J.C.A. Kokenberg & Jan Baarssen
License: Microsoft Reciprocal License (MS-RL)
*/
// AUTOGENERATED FILE - 08-05-2023 (03:41:46) - Utc
// DO NOT MODIFY
// Sourcefile: fpdfview.h
namespace PdfLibCore;
public struct FPDF_TEXTPAGE : IHandle<FPDF_TEXTPAGE>
{
    private IntPtr _pointer;
    public bool IsNull => _pointer == IntPtr.Zero;
    public static FPDF_TEXTPAGE Null => new();
    private FPDF_TEXTPAGE(IntPtr ptr) => _pointer = ptr;
    FPDF_TEXTPAGE IHandle<FPDF_TEXTPAGE>.SetToNull() => new(Interlocked.Exchange(ref _pointer, IntPtr.Zero));
    public override string ToString() => $"FPDF_TEXTPAGE: 0x{_pointer:X16}";
}