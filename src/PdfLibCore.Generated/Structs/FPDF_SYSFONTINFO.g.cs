using System;
using System.Runtime.InteropServices;

/*
This file is part of PdfLibCore, a wrapper around the PDFium library for the .NET.
Inspired by the awesome work of PDFiumSharp by Tobias Meyer.

Copyright (C) 2023 J.C.A. Kokenberg & Jan Baarssen
License: Microsoft Reciprocal License (MS-RL)
*/

// AUTOGENERATED FILE - 10-05-2023 (08:37:23) - Utc
// DO NOT MODIFY
// Sourcefile: fpdf_sysfontinfo.h

namespace PdfLibCore.Generated.Structs;
// Interface: FPDF_SYSFONTINFO
// Interface for getting system font information and font mapping
[StructLayout(LayoutKind.Sequential)]
public partial struct FPDF_SYSFONTINFO
{
    // Version number of the interface. Currently must be 1.
    public int Version { get; set; }
}