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
public enum FPDF_TEXT_RENDERMODE
{
    FPDF_TEXTRENDERMODE_UNKNOWN = -1,
    FPDF_TEXTRENDERMODE_FILL = 0,
    FPDF_TEXTRENDERMODE_STROKE = 1,
    FPDF_TEXTRENDERMODE_FILL_STROKE = 2,
    FPDF_TEXTRENDERMODE_INVISIBLE = 3,
    FPDF_TEXTRENDERMODE_FILL_CLIP = 4,
    FPDF_TEXTRENDERMODE_STROKE_CLIP = 5,
    FPDF_TEXTRENDERMODE_FILL_STROKE_CLIP = 6,
    FPDF_TEXTRENDERMODE_CLIP = 7,
    FPDF_TEXTRENDERMODE_LAST = FPDF_TEXTRENDERMODE_CLIP
}