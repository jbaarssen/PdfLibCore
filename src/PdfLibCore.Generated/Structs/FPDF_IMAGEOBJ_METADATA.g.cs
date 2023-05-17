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
// Sourcefile: https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_edit.h

namespace PdfLibCore.Generated.Structs;
[StructLayout(LayoutKind.Sequential)]
public partial struct FPDF_IMAGEOBJ_METADATA
{
    // The image width in pixels.
    public uint Width { get; set; }

    // The image height in pixels.
    public uint Height { get; set; }

    // The image's horizontal pixel-per-inch.
    public float HorizontalDpi { get; set; }

    // The image's vertical pixel-per-inch.
    public float VerticalDpi { get; set; }

    // The number of bits used to represent each pixel.
    public uint BitsPerPixel { get; set; }

    // The image's colorspace. See above for the list of FPDF_COLORSPACE_*.
    public int Colorspace { get; set; }

    // The image's marked content ID. Useful for pairing with associated alt-text.
    // A value of -1 indicates no ID.
    public int MarkedContentId { get; set; }
}