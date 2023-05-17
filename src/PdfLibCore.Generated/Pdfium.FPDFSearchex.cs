// Built from precompiled binaries at https://github.com/bblanchon/pdfium-binaries/releases/tag/chromium/5772
// Github release api https://api.github.com/repos/bblanchon/pdfium-binaries/releases/102934879
// PDFium version v115.0.5772.0 chromium/5772 [master]
// Built on: Wed, 17 May 2023 14:53:10 GMT

// ReSharper disable all
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

// ----------------------------------------------------------------------------
// <auto-generated>
// This is autogenerated code by CppSharp.
// Do not edit this file or all your changes will be lost after re-generation.
// </auto-generated>
// ----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Security;
using __CallingConvention = global::System.Runtime.InteropServices.CallingConvention;
using __IntPtr = global::System.IntPtr;

#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required

namespace PdfLibCore.Generated
{
    public static unsafe partial class Pdfium
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFText_GetCharIndexFromTextIndex", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFText_GetCharIndexFromTextIndex(__IntPtr text_page, int nTextIndex);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFText_GetTextIndexFromCharIndex", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFText_GetTextIndexFromCharIndex(__IntPtr text_page, int nCharIndex);
        }


        public static int FPDFText_GetCharIndexFromTextIndex(global::PdfLibCore.Generated.FPDF_Textpage text_page, int nTextIndex)
        {
            var __arg0 = text_page is null ? __IntPtr.Zero : text_page.__Instance;
            var ___ret = __Internal.FPDFText_GetCharIndexFromTextIndex(__arg0, nTextIndex);
            return ___ret;
        }


        public static int FPDFText_GetTextIndexFromCharIndex(global::PdfLibCore.Generated.FPDF_Textpage text_page, int nCharIndex)
        {
            var __arg0 = text_page is null ? __IntPtr.Zero : text_page.__Instance;
            var ___ret = __Internal.FPDFText_GetTextIndexFromCharIndex(__arg0, nCharIndex);
            return ___ret;
        }
    }
}
