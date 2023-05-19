// Built from precompiled binaries at https://github.com/bblanchon/pdfium-binaries/releases/tag/chromium/5772
// Github release api https://api.github.com/repos/bblanchon/pdfium-binaries/releases/102934879
// PDFium version v115.0.5772.0 chromium/5772 [master]
// Built on: Fri, 19 May 2023 18:28:28 GMT

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
    public unsafe partial class Pdfium
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFCatalog_IsTagged", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDFCatalog_IsTagged(__IntPtr document);
        }


        public static global::PdfLibCore.Types.FPDF_BOOL FPDFCatalog_IsTagged(global::PdfLibCore.Generated.FPDF_Document document)
        {
            var __arg0 = document is null ? __IntPtr.Zero : document.__Instance;
            var ___ret = __Internal.FPDFCatalog_IsTagged(__arg0);
            return ___ret;
        }
    }
}
#pragma warning restore
