// Built from precompiled binaries at https://github.com/bblanchon/pdfium-binaries/releases/tag/chromium/5772
// Github release api https://api.github.com/repos/bblanchon/pdfium-binaries/releases/102934879
// PDFium version v115.0.5772.0 chromium/5772 [master]
// Built on: Wed, 17 May 2023 18:47:10 GMT

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
    public unsafe partial class IFSDK_PAUSE : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Size = 24)]
        public partial struct __Internal
        {
            internal int version;
            internal __IntPtr NeedToPauseNow;
            internal __IntPtr user;

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "??0_IFSDK_PAUSE@@QEAA@AEBU0@@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr cctor(__IntPtr __instance, __IntPtr _0);
        }

        public __IntPtr __Instance { get; protected set; }

        protected bool __ownsNativeInstance;

        internal static IFSDK_PAUSE __CreateInstance(__IntPtr native, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            return new IFSDK_PAUSE(native.ToPointer(), skipVTables);
        }

        internal static IFSDK_PAUSE __CreateInstance(__Internal native, bool skipVTables = false)
        {
            return new IFSDK_PAUSE(native, skipVTables);
        }

        private static void* __CopyValue(__Internal native)
        {
            var ret = Marshal.AllocHGlobal(sizeof(__Internal));
            *(__Internal*) ret = native;
            return ret.ToPointer();
        }

        private IFSDK_PAUSE(__Internal native, bool skipVTables = false)
            : this(__CopyValue(native), skipVTables)
        {
            __ownsNativeInstance = true;
        }

        protected IFSDK_PAUSE(void* native, bool skipVTables = false)
        {
            if (native == null)
                return;
            __Instance = new __IntPtr(native);
        }

        public IFSDK_PAUSE()
        {
            if (GetType().FullName != "PdfLibCore.Generated.IFSDK_PAUSE")
                throw new Exception("PdfLibCore.Generated.IFSDK_PAUSE: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.IFSDK_PAUSE.__Internal));
            __ownsNativeInstance = true;
        }

        public IFSDK_PAUSE(global::PdfLibCore.Generated.IFSDK_PAUSE _0)
        {
            if (GetType().FullName != "PdfLibCore.Generated.IFSDK_PAUSE")
                throw new Exception("PdfLibCore.Generated.IFSDK_PAUSE: Can't inherit from classes with disabled NativeToManaged map");
            __Instance = Marshal.AllocHGlobal(sizeof(global::PdfLibCore.Generated.IFSDK_PAUSE.__Internal));
            __ownsNativeInstance = true;
            *((global::PdfLibCore.Generated.IFSDK_PAUSE.__Internal*) __Instance) = *((global::PdfLibCore.Generated.IFSDK_PAUSE.__Internal*) _0.__Instance);
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

        public global::PdfLibCore.Generated.Delegates.Func_PdfLibCore_Types_FPDF_BOOL___IntPtr NeedToPauseNow
        {
            get
            {
                var __ptr0 = ((__Internal*)__Instance)->NeedToPauseNow;
                return __ptr0 == IntPtr.Zero? null : (global::PdfLibCore.Generated.Delegates.Func_PdfLibCore_Types_FPDF_BOOL___IntPtr) Marshal.GetDelegateForFunctionPointer(__ptr0, typeof(global::PdfLibCore.Generated.Delegates.Func_PdfLibCore_Types_FPDF_BOOL___IntPtr));
            }

            set
            {
                ((__Internal*)__Instance)->NeedToPauseNow = value == null ? global::System.IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value);
            }
        }

        public __IntPtr User
        {
            get
            {
                return ((__Internal*)__Instance)->user;
            }

            set
            {
                ((__Internal*)__Instance)->user = (__IntPtr) value;
            }
        }
    }

    public static unsafe partial class Pdfium
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_RenderPageBitmapWithColorScheme_Start", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_RenderPageBitmapWithColorScheme_Start(__IntPtr bitmap, __IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, int flags, __IntPtr color_scheme, __IntPtr pause);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_RenderPageBitmap_Start", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_RenderPageBitmap_Start(__IntPtr bitmap, __IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, int flags, __IntPtr pause);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_RenderPage_Continue", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_RenderPage_Continue(__IntPtr page, __IntPtr pause);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_RenderPage_Close", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void FPDF_RenderPage_Close(__IntPtr page);
        }

        /// <summary>
        /// Start to render page contents to a device independent bitmap
        /// progressively with a specified color scheme for the content.
        /// </summary>
        /// <param name="bitmap">
        /// Handle to the device independent bitmap (as the
        /// output buffer). Bitmap handle can be created by
        /// FPDFBitmap_Create function.
        /// </param>
        /// <param name="page">Handle to the page as returned by FPDF_LoadPage function.</param>
        /// <param name="start_x">Left pixel position of the display area in the bitmap coordinate.</param>
        /// <param name="start_y">Top pixel position of the display area in the bitmap coordinate.</param>
        /// <param name="size_x">Horizontal size (in pixels) for displaying the page.</param>
        /// <param name="size_y">Vertical size (in pixels) for displaying the page.</param>
        /// <param name="rotate">
        /// Page orientation: 0 (normal), 1 (rotated 90
        /// degrees clockwise), 2 (rotated 180 degrees),
        /// 3 (rotated 90 degrees counter-clockwise).
        /// </param>
        /// <param name="flags">
        /// 0 for normal display, or combination of flags
        /// defined in fpdfview.h. With FPDF_ANNOT flag, it
        /// renders all annotations that does not require
        /// user-interaction, which are all annotations except
        /// widget and popup annotations.
        /// </param>
        /// <param name="color_scheme">
        /// Color scheme to be used in rendering the |page|.
        /// If null, this function will work similar to
        /// FPDF_RenderPageBitmap_Start().
        /// </param>
        /// <param name="pause">The IFSDK_PAUSE interface. A callback mechanism allowing the page rendering process.</param>
        /// <returns>Rendering Status. See flags for progressive process status for the details.</returns>
        public static int FPDF_RenderPageBitmapWithColorScheme_Start(global::PdfLibCore.Generated.FPDF_Bitmap bitmap, global::PdfLibCore.Generated.FPDF_Page page, int start_x, int start_y, int size_x, int size_y, int rotate, int flags, global::PdfLibCore.Generated.FPDF_COLORSCHEME_ color_scheme, global::PdfLibCore.Generated.IFSDK_PAUSE pause)
        {
            var __arg0 = bitmap is null ? __IntPtr.Zero : bitmap.__Instance;
            var __arg1 = page is null ? __IntPtr.Zero : page.__Instance;
            var __arg8 = color_scheme is null ? __IntPtr.Zero : color_scheme.__Instance;
            var __arg9 = pause is null ? __IntPtr.Zero : pause.__Instance;
            var ___ret = __Internal.FPDF_RenderPageBitmapWithColorScheme_Start(__arg0, __arg1, start_x, start_y, size_x, size_y, rotate, flags, __arg8, __arg9);
            return ___ret;
        }

        /// <summary>Start to render page contents to a device independent bitmap progressively.</summary>
        /// <param name="bitmap">
        /// Handle to the device independent bitmap (as the
        /// output buffer). Bitmap handle can be created by
        /// FPDFBitmap_Create().
        /// </param>
        /// <param name="page">Handle to the page, as returned by FPDF_LoadPage().</param>
        /// <param name="start_x">Left pixel position of the display area in the bitmap coordinates.</param>
        /// <param name="start_y">Top pixel position of the display area in the bitmap coordinates.</param>
        /// <param name="size_x">Horizontal size (in pixels) for displaying the page.</param>
        /// <param name="size_y">Vertical size (in pixels) for displaying the page.</param>
        /// <param name="rotate">
        /// Page orientation: 0 (normal), 1 (rotated 90 degrees
        /// clockwise), 2 (rotated 180 degrees), 3 (rotated 90
        /// degrees counter-clockwise).
        /// </param>
        /// <param name="flags">
        /// 0 for normal display, or combination of flags
        /// defined in fpdfview.h. With FPDF_ANNOT flag, it
        /// renders all annotations that does not require
        /// user-interaction, which are all annotations except
        /// widget and popup annotations.
        /// </param>
        /// <param name="pause">The IFSDK_PAUSE interface.A callback mechanism allowing the page rendering process</param>
        /// <returns>Rendering Status. See flags for progressive process status for the details.</returns>
        public static int FPDF_RenderPageBitmap_Start(global::PdfLibCore.Generated.FPDF_Bitmap bitmap, global::PdfLibCore.Generated.FPDF_Page page, int start_x, int start_y, int size_x, int size_y, int rotate, int flags, global::PdfLibCore.Generated.IFSDK_PAUSE pause)
        {
            var __arg0 = bitmap is null ? __IntPtr.Zero : bitmap.__Instance;
            var __arg1 = page is null ? __IntPtr.Zero : page.__Instance;
            var __arg8 = pause is null ? __IntPtr.Zero : pause.__Instance;
            var ___ret = __Internal.FPDF_RenderPageBitmap_Start(__arg0, __arg1, start_x, start_y, size_x, size_y, rotate, flags, __arg8);
            return ___ret;
        }

        /// <summary>Continue rendering a PDF page.</summary>
        /// <param name="page">Handle to the page, as returned by FPDF_LoadPage().</param>
        /// <param name="pause">
        /// The IFSDK_PAUSE interface (a callback mechanism
        /// allowing the page rendering process to be paused
        /// before it's finished). This can be NULL if you
        /// don't want to pause.
        /// </param>
        /// <returns>The rendering status. See flags for progressive process status for the details.</returns>
        public static int FPDF_RenderPage_Continue(global::PdfLibCore.Generated.FPDF_Page page, global::PdfLibCore.Generated.IFSDK_PAUSE pause)
        {
            var __arg0 = page is null ? __IntPtr.Zero : page.__Instance;
            var __arg1 = pause is null ? __IntPtr.Zero : pause.__Instance;
            var ___ret = __Internal.FPDF_RenderPage_Continue(__arg0, __arg1);
            return ___ret;
        }

        /// <summary>
        /// Release the resource allocate during page rendering. Need to be
        /// called after finishing rendering or
        /// cancel the rendering.
        /// </summary>
        /// <param name="page">Handle to the page, as returned by FPDF_LoadPage().</param>
        public static void FPDF_RenderPage_Close(global::PdfLibCore.Generated.FPDF_Page page)
        {
            var __arg0 = page is null ? __IntPtr.Zero : page.__Instance;
            __Internal.FPDF_RenderPage_Close(__arg0);
        }
    }
}
