using System;
using System.Security;
using System.Runtime.InteropServices;
using PdfLibCore.Generated.Enums;
using PdfLibCore.Generated.Structs;
using PdfLibCore.Generated.Types;

/*
This file is part of PdfLibCore, a wrapper around the PDFium library for the .NET.
Inspired by the awesome work of PDFiumSharp by Tobias Meyer.

Copyright (C) 2023 J.C.A. Kokenberg & Jan Baarssen
License: Microsoft Reciprocal License (MS-RL)
*/

// AUTOGENERATED FILE - 10-05-2023 (01:50:33) - Utc
// DO NOT MODIFY

namespace PdfLibCore.Generated;
public static partial class PlatformInvoke
{
    ///<remarks>For more information see: https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_thumbnail.h.</remarks>
    [DllImport(DllName, EntryPoint = nameof(FPDFPage_GetDecodedThumbnailData), SetLastError = true), SuppressUnmanagedCodeSecurity]
    public static extern uint FPDFPage_GetDecodedThumbnailData(FPDF_PAGE page, out IntPtr buffer, uint buflen);
    ///<remarks>For more information see: https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_thumbnail.h.</remarks>
    [DllImport(DllName, EntryPoint = nameof(FPDFPage_GetRawThumbnailData), SetLastError = true), SuppressUnmanagedCodeSecurity]
    public static extern uint FPDFPage_GetRawThumbnailData(FPDF_PAGE page, out IntPtr buffer, uint buflen);
    ///<remarks>For more information see: https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_thumbnail.h.</remarks>
    [DllImport(DllName, EntryPoint = nameof(FPDFPage_GetThumbnailAsBitmap), SetLastError = true), SuppressUnmanagedCodeSecurity]
    public static extern FPDF_BITMAP FPDFPage_GetThumbnailAsBitmap(FPDF_PAGE page);
}