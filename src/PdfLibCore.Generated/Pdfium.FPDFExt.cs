// Built from precompiled binaries at https://github.com/bblanchon/pdfium-binaries/releases/tag/chromium/5772
// Github release api https://api.github.com/repos/bblanchon/pdfium-binaries/releases/102934879
// PDFium version v115.0.5772.0 chromium/5772 [master]

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
    public unsafe partial class UNSUPPORT_INFO : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public partial struct __Internal
        {
            internal int version;
            internal __IntPtr FSDK_UnSupport_Handler;

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "??0_UNSUPPORT_INFO@@QEAA@AEBU0@@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr cctor(__IntPtr __instance, __IntPtr _0);
        }

        public __IntPtr __Instance { get; protected set; }

        protected bool __ownsNativeInstance;

        internal static UNSUPPORT_INFO __CreateInstance(__IntPtr native, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            return new UNSUPPORT_INFO(native.ToPointer(), skipVTables);
        }

        internal static UNSUPPORT_INFO __CreateInstance(__Internal native, bool skipVTables = false)
        {
            return new UNSUPPORT_INFO(native, skipVTables);
        }

        private static void* __CopyValue(__Internal native)
        {
            var ret = Marshal.AllocHGlobal(sizeof(__Internal));
            *(__Internal*) ret = native;
            return ret.ToPointer();
        }

        private UNSUPPORT_INFO(__Internal native, bool skipVTables = false)
            : this(__CopyValue(native), skipVTables)
        {
            __ownsNativeInstance = true;
        }

        protected UNSUPPORT_INFO(void* native, bool skipVTables = false)
        {
            if (native == null)
                return;
            __Instance = new __IntPtr(native);
        }

        public UNSUPPORT_INFO()
        {
            if (GetType().FullName != "PdfLibCore.Generated.UNSUPPORT_INFO")
                throw new Exception("PdfLibCore.Generated.UNSUPPORT_INFO: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.UNSUPPORT_INFO.__Internal));
            __ownsNativeInstance = true;
        }

        public UNSUPPORT_INFO(global::PdfLibCore.Generated.UNSUPPORT_INFO _0)
        {
            if (GetType().FullName != "PdfLibCore.Generated.UNSUPPORT_INFO")
                throw new Exception("PdfLibCore.Generated.UNSUPPORT_INFO: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.UNSUPPORT_INFO.__Internal));
            __ownsNativeInstance = true;
            *((global::PdfLibCore.Generated.UNSUPPORT_INFO.__Internal*) __Instance) = *((global::PdfLibCore.Generated.UNSUPPORT_INFO.__Internal*) _0.__Instance);
        }

        public void Dispose()
        {
            Dispose(disposing: true, callNativeDtor : __ownsNativeInstance );
        }

        partial void DisposePartial(bool disposing);

        internal protected virtual void Dispose(bool disposing, bool callNativeDtor )
        {
            if (__Instance == IntPtr.Zero)
                return;
            DisposePartial(disposing);
            if (__ownsNativeInstance)
                Marshal.FreeHGlobal(__Instance);
            __Instance = IntPtr.Zero;
        }

        public int Version
        {
            get
            {
                return ((__Internal*)__Instance)->version;
            }

            set
            {
                ((__Internal*)__Instance)->version = value;
            }
        }

        public global::PdfLibCore.Generated.Delegates.Action___IntPtr_int FSDK_UnSupportHandler
        {
            get
            {
                var __ptr0 = ((__Internal*)__Instance)->FSDK_UnSupport_Handler;
                return __ptr0 == IntPtr.Zero? null : (global::PdfLibCore.Generated.Delegates.Action___IntPtr_int) Marshal.GetDelegateForFunctionPointer(__ptr0, typeof(global::PdfLibCore.Generated.Delegates.Action___IntPtr_int));
            }

            set
            {
                ((__Internal*)__Instance)->FSDK_UnSupport_Handler = value == null ? global::System.IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value);
            }
        }
    }

    public unsafe partial class Pdfium
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FSDK_SetUnSpObjProcessHandler", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FSDK_SetUnSpObjProcessHandler(__IntPtr unsp_info);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FSDK_SetTimeFunction", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void FSDK_SetTimeFunction(__IntPtr func);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFDoc_GetPageMode", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFDoc_GetPageMode(__IntPtr document);
        }


        public static global::PdfLibCore.Types.FPDF_BOOL FSDK_SetUnSpObjProcessHandler(global::PdfLibCore.Generated.UNSUPPORT_INFO unsp_info)
        {
            var __arg0 = unsp_info is null ? __IntPtr.Zero : unsp_info.__Instance;
            var ___ret = __Internal.FSDK_SetUnSpObjProcessHandler(__arg0);
            return ___ret;
        }


        public static void FSDK_SetTimeFunction(global::PdfLibCore.Generated.Delegates.Func_long func)
        {
            var __arg0 = func == null ? global::System.IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(func);
            __Internal.FSDK_SetTimeFunction(__arg0);
        }


        public static int FPDFDoc_GetPageMode(global::PdfLibCore.Generated.FPDF_Document document)
        {
            var __arg0 = document is null ? __IntPtr.Zero : document.__Instance;
            var ___ret = __Internal.FPDFDoc_GetPageMode(__arg0);
            return ___ret;
        }
    }
}
#pragma warning restore
