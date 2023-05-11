using System;
using System.Runtime.InteropServices;

/*
This file is part of PdfLibCore, a wrapper around the PDFium library for the .NET.
Inspired by the awesome work of PDFiumSharp by Tobias Meyer.

Copyright (C) 2023 J.C.A. Kokenberg & Jan Baarssen
License: Microsoft Reciprocal License (MS-RL)
*/

// AUTOGENERATED FILE - 10-05-2023 (01:50:33) - Utc
// DO NOT MODIFY
// Sourcefile: https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h

namespace PdfLibCore.Generated.Structs;
// Structure for custom file access.
[StructLayout(LayoutKind.Sequential)]
public partial struct FPDF_FILEACCESS
{
    // File length, in bytes.
    public uint FileLen { get; set; }

    // A custom pointer for all implementation specific data.  This pointer will
    // be used as the first parameter to the m_GetBlock callback.
    public IntPtr Aram { get; set; }
}