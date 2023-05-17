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
    public unsafe partial class FX_FILEAVAIL : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public partial struct __Internal
        {
            internal int version;
            internal __IntPtr IsDataAvail;

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "??0_FX_FILEAVAIL@@QEAA@AEBU0@@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr cctor(__IntPtr __instance, __IntPtr _0);
        }

        public __IntPtr __Instance { get; protected set; }

        protected bool __ownsNativeInstance;

        internal static FX_FILEAVAIL __CreateInstance(__IntPtr native, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            return new FX_FILEAVAIL(native.ToPointer(), skipVTables);
        }

        internal static FX_FILEAVAIL __CreateInstance(__Internal native, bool skipVTables = false)
        {
            return new FX_FILEAVAIL(native, skipVTables);
        }

        private static void* __CopyValue(__Internal native)
        {
            var ret = Marshal.AllocHGlobal(sizeof(__Internal));
            *(__Internal*) ret = native;
            return ret.ToPointer();
        }

        private FX_FILEAVAIL(__Internal native, bool skipVTables = false)
            : this(__CopyValue(native), skipVTables)
        {
            __ownsNativeInstance = true;
        }

        protected FX_FILEAVAIL(void* native, bool skipVTables = false)
        {
            if (native == null)
                return;
            __Instance = new __IntPtr(native);
        }

        public FX_FILEAVAIL()
        {
            if (GetType().FullName != "PdfLibCore.Generated.FX_FILEAVAIL")
                throw new Exception("PdfLibCore.Generated.FX_FILEAVAIL: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.FX_FILEAVAIL.__Internal));
            __ownsNativeInstance = true;
        }

        public FX_FILEAVAIL(global::PdfLibCore.Generated.FX_FILEAVAIL _0)
        {
            if (GetType().FullName != "PdfLibCore.Generated.FX_FILEAVAIL")
                throw new Exception("PdfLibCore.Generated.FX_FILEAVAIL: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.FX_FILEAVAIL.__Internal));
            __ownsNativeInstance = true;
            *((global::PdfLibCore.Generated.FX_FILEAVAIL.__Internal*) __Instance) = *((global::PdfLibCore.Generated.FX_FILEAVAIL.__Internal*) _0.__Instance);
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

        public global::PdfLibCore.Generated.Delegates.Func_PdfLibCore_Types_FPDF_BOOL___IntPtr_ulong_ulong IsDataAvail
        {
            get
            {
                var __ptr0 = ((__Internal*)__Instance)->IsDataAvail;
                return __ptr0 == IntPtr.Zero? null : (global::PdfLibCore.Generated.Delegates.Func_PdfLibCore_Types_FPDF_BOOL___IntPtr_ulong_ulong) Marshal.GetDelegateForFunctionPointer(__ptr0, typeof(global::PdfLibCore.Generated.Delegates.Func_PdfLibCore_Types_FPDF_BOOL___IntPtr_ulong_ulong));
            }

            set
            {
                ((__Internal*)__Instance)->IsDataAvail = value == null ? global::System.IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value);
            }
        }
    }

    public unsafe partial class FX_DOWNLOADHINTS : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public partial struct __Internal
        {
            internal int version;
            internal __IntPtr AddSegment;

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "??0_FX_DOWNLOADHINTS@@QEAA@AEBU0@@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr cctor(__IntPtr __instance, __IntPtr _0);
        }

        public __IntPtr __Instance { get; protected set; }

        protected bool __ownsNativeInstance;

        internal static FX_DOWNLOADHINTS __CreateInstance(__IntPtr native, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            return new FX_DOWNLOADHINTS(native.ToPointer(), skipVTables);
        }

        internal static FX_DOWNLOADHINTS __CreateInstance(__Internal native, bool skipVTables = false)
        {
            return new FX_DOWNLOADHINTS(native, skipVTables);
        }

        private static void* __CopyValue(__Internal native)
        {
            var ret = Marshal.AllocHGlobal(sizeof(__Internal));
            *(__Internal*) ret = native;
            return ret.ToPointer();
        }

        private FX_DOWNLOADHINTS(__Internal native, bool skipVTables = false)
            : this(__CopyValue(native), skipVTables)
        {
            __ownsNativeInstance = true;
        }

        protected FX_DOWNLOADHINTS(void* native, bool skipVTables = false)
        {
            if (native == null)
                return;
            __Instance = new __IntPtr(native);
        }

        public FX_DOWNLOADHINTS()
        {
            if (GetType().FullName != "PdfLibCore.Generated.FX_DOWNLOADHINTS")
                throw new Exception("PdfLibCore.Generated.FX_DOWNLOADHINTS: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.FX_DOWNLOADHINTS.__Internal));
            __ownsNativeInstance = true;
        }

        public FX_DOWNLOADHINTS(global::PdfLibCore.Generated.FX_DOWNLOADHINTS _0)
        {
            if (GetType().FullName != "PdfLibCore.Generated.FX_DOWNLOADHINTS")
                throw new Exception("PdfLibCore.Generated.FX_DOWNLOADHINTS: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.FX_DOWNLOADHINTS.__Internal));
            __ownsNativeInstance = true;
            *((global::PdfLibCore.Generated.FX_DOWNLOADHINTS.__Internal*) __Instance) = *((global::PdfLibCore.Generated.FX_DOWNLOADHINTS.__Internal*) _0.__Instance);
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

        public global::PdfLibCore.Generated.Delegates.Action___IntPtr_ulong_ulong AddSegment
        {
            get
            {
                var __ptr0 = ((__Internal*)__Instance)->AddSegment;
                return __ptr0 == IntPtr.Zero? null : (global::PdfLibCore.Generated.Delegates.Action___IntPtr_ulong_ulong) Marshal.GetDelegateForFunctionPointer(__ptr0, typeof(global::PdfLibCore.Generated.Delegates.Action___IntPtr_ulong_ulong));
            }

            set
            {
                ((__Internal*)__Instance)->AddSegment = value == null ? global::System.IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value);
            }
        }
    }

    public static unsafe partial class Pdfium
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_Create", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDFAvail_Create(__IntPtr file_avail, __IntPtr file);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_Destroy", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void FPDFAvail_Destroy(__IntPtr avail);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_IsDocAvail", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFAvail_IsDocAvail(__IntPtr avail, __IntPtr hints);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_GetDocument", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDFAvail_GetDocument(__IntPtr avail, string password);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_GetFirstPageNum", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFAvail_GetFirstPageNum(__IntPtr doc);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_IsPageAvail", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFAvail_IsPageAvail(__IntPtr avail, int page_index, __IntPtr hints);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_IsFormAvail", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFAvail_IsFormAvail(__IntPtr avail, __IntPtr hints);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDFAvail_IsLinearized", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDFAvail_IsLinearized(__IntPtr avail);
        }


        public static global::PdfLibCore.Generated.FPDF_Avail FPDFAvail_Create(global::PdfLibCore.Generated.FX_FILEAVAIL file_avail, global::PdfLibCore.Generated.FPDF_FILEACCESS file)
        {
            var __arg0 = file_avail is null ? __IntPtr.Zero : file_avail.__Instance;
            var __arg1 = file is null ? __IntPtr.Zero : file.__Instance;
            var ___ret = __Internal.FPDFAvail_Create(__arg0, __arg1);
            var __result0 = global::PdfLibCore.Generated.FPDF_Avail.__CreateInstance(___ret, false);
            return __result0;
        }


        public static void FPDFAvail_Destroy(global::PdfLibCore.Generated.FPDF_Avail avail)
        {
            var __arg0 = avail is null ? __IntPtr.Zero : avail.__Instance;
            __Internal.FPDFAvail_Destroy(__arg0);
        }


        public static int FPDFAvail_IsDocAvail(global::PdfLibCore.Generated.FPDF_Avail avail, global::PdfLibCore.Generated.FX_DOWNLOADHINTS hints)
        {
            var __arg0 = avail is null ? __IntPtr.Zero : avail.__Instance;
            var __arg1 = hints is null ? __IntPtr.Zero : hints.__Instance;
            var ___ret = __Internal.FPDFAvail_IsDocAvail(__arg0, __arg1);
            return ___ret;
        }


        public static global::PdfLibCore.Generated.FPDF_Document FPDFAvail_GetDocument(global::PdfLibCore.Generated.FPDF_Avail avail, string password)
        {
            var __arg0 = avail is null ? __IntPtr.Zero : avail.__Instance;
            var ___ret = __Internal.FPDFAvail_GetDocument(__arg0, password);
            var __result0 = global::PdfLibCore.Generated.FPDF_Document.__CreateInstance(___ret, false);
            return __result0;
        }


        public static int FPDFAvail_GetFirstPageNum(global::PdfLibCore.Generated.FPDF_Document doc)
        {
            var __arg0 = doc is null ? __IntPtr.Zero : doc.__Instance;
            var ___ret = __Internal.FPDFAvail_GetFirstPageNum(__arg0);
            return ___ret;
        }


        public static int FPDFAvail_IsPageAvail(global::PdfLibCore.Generated.FPDF_Avail avail, int page_index, global::PdfLibCore.Generated.FX_DOWNLOADHINTS hints)
        {
            var __arg0 = avail is null ? __IntPtr.Zero : avail.__Instance;
            var __arg2 = hints is null ? __IntPtr.Zero : hints.__Instance;
            var ___ret = __Internal.FPDFAvail_IsPageAvail(__arg0, page_index, __arg2);
            return ___ret;
        }


        public static int FPDFAvail_IsFormAvail(global::PdfLibCore.Generated.FPDF_Avail avail, global::PdfLibCore.Generated.FX_DOWNLOADHINTS hints)
        {
            var __arg0 = avail is null ? __IntPtr.Zero : avail.__Instance;
            var __arg1 = hints is null ? __IntPtr.Zero : hints.__Instance;
            var ___ret = __Internal.FPDFAvail_IsFormAvail(__arg0, __arg1);
            return ___ret;
        }


        public static int FPDFAvail_IsLinearized(global::PdfLibCore.Generated.FPDF_Avail avail)
        {
            var __arg0 = avail is null ? __IntPtr.Zero : avail.__Instance;
            var ___ret = __Internal.FPDFAvail_IsLinearized(__arg0);
            return ___ret;
        }
    }
}