// Built from precompiled binaries at https://github.com/bblanchon/pdfium-binaries/releases/tag/chromium/5772
// Github release api https://api.github.com/repos/bblanchon/pdfium-binaries/releases/102934879
// PDFium version v115.0.5772.0 chromium/5772 [master]
// Built on: Wed, 17 May 2023 13:34:04 GMT

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
            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFDoc_GetAttachmentCount", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFDoc_GetAttachmentCount(__IntPtr document);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFDoc_AddAttachment", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDFDoc_AddAttachment(__IntPtr document, string name);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFDoc_GetAttachment", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDFDoc_GetAttachment(__IntPtr document, int index);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFDoc_DeleteAttachment", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDFDoc_DeleteAttachment(__IntPtr document, int index);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAttachment_GetName", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDFAttachment_GetName(__IntPtr attachment, string* buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAttachment_HasKey", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_HasKey(__IntPtr attachment, string key);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAttachment_GetValueType", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFAttachment_GetValueType(__IntPtr attachment, string key);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAttachment_SetStringValue", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_SetStringValue(__IntPtr attachment, string key, string value);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAttachment_GetStringValue", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDFAttachment_GetStringValue(__IntPtr attachment, string key, string* buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAttachment_SetFile", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_SetFile(__IntPtr attachment, __IntPtr document, __IntPtr contents, uint len);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAttachment_GetFile", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_GetFile(__IntPtr attachment, __IntPtr buffer, uint buflen, uint* out_buflen);
        }


        public static int FPDFDoc_GetAttachmentCount(global::PdfLibCore.Generated.FPDF_Document document)
        {
            var __arg0 = document is null ? __IntPtr.Zero : document.__Instance;
            var ___ret = __Internal.FPDFDoc_GetAttachmentCount(__arg0);
            return ___ret;
        }


        public static global::PdfLibCore.Generated.FPDF_Attachment FPDFDoc_AddAttachment(global::PdfLibCore.Generated.FPDF_Document document, ref string name)
        {
            var __arg0 = document is null ? __IntPtr.Zero : document.__Instance;
            var ___ret = __Internal.FPDFDoc_AddAttachment(__arg0, name);
            var __result0 = global::PdfLibCore.Generated.FPDF_Attachment.__CreateInstance(___ret, false);
            return __result0;
        }


        public static global::PdfLibCore.Generated.FPDF_Attachment FPDFDoc_GetAttachment(global::PdfLibCore.Generated.FPDF_Document document, int index)
        {
            var __arg0 = document is null ? __IntPtr.Zero : document.__Instance;
            var ___ret = __Internal.FPDFDoc_GetAttachment(__arg0, index);
            var __result0 = global::PdfLibCore.Generated.FPDF_Attachment.__CreateInstance(___ret, false);
            return __result0;
        }


        public static global::PdfLibCore.Types.FPDF_BOOL FPDFDoc_DeleteAttachment(global::PdfLibCore.Generated.FPDF_Document document, int index)
        {
            var __arg0 = document is null ? __IntPtr.Zero : document.__Instance;
            var ___ret = __Internal.FPDFDoc_DeleteAttachment(__arg0, index);
            return ___ret;
        }


        public static uint FPDFAttachment_GetName(global::PdfLibCore.Generated.FPDF_Attachment attachment, ref string buffer, uint buflen)
        {
            var __arg0 = attachment is null ? __IntPtr.Zero : attachment.__Instance;
            fixed (string* __buffer1 = &buffer)
            {
                var __arg1 = __buffer1;
                var ___ret = __Internal.FPDFAttachment_GetName(__arg0, __arg1, buflen);
                return ___ret;
            }
        }


        public static global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_HasKey(global::PdfLibCore.Generated.FPDF_Attachment attachment, string key)
        {
            var __arg0 = attachment is null ? __IntPtr.Zero : attachment.__Instance;
            var ___ret = __Internal.FPDFAttachment_HasKey(__arg0, key);
            return ___ret;
        }


        public static int FPDFAttachment_GetValueType(global::PdfLibCore.Generated.FPDF_Attachment attachment, string key)
        {
            var __arg0 = attachment is null ? __IntPtr.Zero : attachment.__Instance;
            var ___ret = __Internal.FPDFAttachment_GetValueType(__arg0, key);
            return ___ret;
        }


        public static global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_SetStringValue(global::PdfLibCore.Generated.FPDF_Attachment attachment, string key, ref string value)
        {
            var __arg0 = attachment is null ? __IntPtr.Zero : attachment.__Instance;
            var ___ret = __Internal.FPDFAttachment_SetStringValue(__arg0, key, value);
            return ___ret;
        }


        public static uint FPDFAttachment_GetStringValue(global::PdfLibCore.Generated.FPDF_Attachment attachment, string key, ref string buffer, uint buflen)
        {
            var __arg0 = attachment is null ? __IntPtr.Zero : attachment.__Instance;
            fixed (string* __buffer2 = &buffer)
            {
                var __arg2 = __buffer2;
                var ___ret = __Internal.FPDFAttachment_GetStringValue(__arg0, key, __arg2, buflen);
                return ___ret;
            }
        }


        public static global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_SetFile(global::PdfLibCore.Generated.FPDF_Attachment attachment, global::PdfLibCore.Generated.FPDF_Document document, __IntPtr contents, uint len)
        {
            var __arg0 = attachment is null ? __IntPtr.Zero : attachment.__Instance;
            var __arg1 = document is null ? __IntPtr.Zero : document.__Instance;
            var ___ret = __Internal.FPDFAttachment_SetFile(__arg0, __arg1, contents, len);
            return ___ret;
        }


        public static global::PdfLibCore.Types.FPDF_BOOL FPDFAttachment_GetFile(global::PdfLibCore.Generated.FPDF_Attachment attachment, __IntPtr buffer, uint buflen, ref uint out_buflen)
        {
            var __arg0 = attachment is null ? __IntPtr.Zero : attachment.__Instance;
            fixed (uint* __out_buflen3 = &out_buflen)
            {
                var __arg3 = __out_buflen3;
                var ___ret = __Internal.FPDFAttachment_GetFile(__arg0, buffer, buflen, __arg3);
                return ___ret;
            }
        }
    }
}