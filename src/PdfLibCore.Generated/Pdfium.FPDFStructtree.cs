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
            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructTree_GetForPage", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDF_StructTree_GetForPage(__IntPtr page);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructTree_Close", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void FPDF_StructTree_Close(__IntPtr struct_tree);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructTree_CountChildren", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructTree_CountChildren(__IntPtr struct_tree);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructTree_GetChildAtIndex", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDF_StructTree_GetChildAtIndex(__IntPtr struct_tree, int index);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetAltText", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetAltText(__IntPtr struct_element, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetActualText", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetActualText(__IntPtr struct_element, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetID", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetID(__IntPtr struct_element, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetLang", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetLang(__IntPtr struct_element, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetStringAttribute", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetStringAttribute(__IntPtr struct_element, string attr_name, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetMarkedContentID", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructElement_GetMarkedContentID(__IntPtr struct_element);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetType", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetType(__IntPtr struct_element, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetObjType", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetObjType(__IntPtr struct_element, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetTitle", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern uint FPDF_StructElement_GetTitle(__IntPtr struct_element, __IntPtr buffer, uint buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_CountChildren", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructElement_CountChildren(__IntPtr struct_element);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetChildAtIndex", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDF_StructElement_GetChildAtIndex(__IntPtr struct_element, int index);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetParent", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDF_StructElement_GetParent(__IntPtr struct_element);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetAttributeCount", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructElement_GetAttributeCount(__IntPtr struct_element);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetAttributeAtIndex", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FPDF_StructElement_GetAttributeAtIndex(__IntPtr struct_element, int index);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_Attr_GetCount", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructElement_Attr_GetCount(__IntPtr struct_attribute);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_Attr_GetName", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetName(__IntPtr struct_attribute, int index, __IntPtr buffer, uint buflen, uint* out_buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_Attr_GetType", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructElement_Attr_GetType(__IntPtr struct_attribute, string name);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_Attr_GetBooleanValue", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetBooleanValue(__IntPtr struct_attribute, string name, global::PdfLibCore.Types.FPDF_BOOL* out_value);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_Attr_GetNumberValue", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetNumberValue(__IntPtr struct_attribute, string name, float* out_value);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_Attr_GetStringValue", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetStringValue(__IntPtr struct_attribute, string name, __IntPtr buffer, uint buflen, uint* out_buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_Attr_GetBlobValue", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetBlobValue(__IntPtr struct_attribute, string name, __IntPtr buffer, uint buflen, uint* out_buflen);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetMarkedContentIdCount", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructElement_GetMarkedContentIdCount(__IntPtr struct_element);

            [SuppressUnmanagedCodeSecurity, DllImport("Pdfium", EntryPoint = "FPDF_StructElement_GetMarkedContentIdAtIndex", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int FPDF_StructElement_GetMarkedContentIdAtIndex(__IntPtr struct_element, int index);
        }

        /// <summary>Get the structure tree for a page.</summary>
        /// <param name="page">Handle to the page, as returned by FPDF_LoadPage().</param>
        /// <returns>A handle to the structure tree or NULL on error.</returns>
        public static global::PdfLibCore.Generated.FPDF_Structtree FPDF_StructTree_GetForPage(global::PdfLibCore.Generated.FPDF_Page page)
        {
            var __arg0 = page is null ? __IntPtr.Zero : page.__Instance;
            var ___ret = __Internal.FPDF_StructTree_GetForPage(__arg0);
            var __result0 = global::PdfLibCore.Generated.FPDF_Structtree.__CreateInstance(___ret, false);
            return __result0;
        }

        /// <summary>Release a resource allocated by FPDF_StructTree_GetForPage().</summary>
        /// <param name="struct_tree">Handle to the structure tree, as returned by FPDF_StructTree_LoadPage().</param>
        public static void FPDF_StructTree_Close(global::PdfLibCore.Generated.FPDF_Structtree struct_tree)
        {
            var __arg0 = struct_tree is null ? __IntPtr.Zero : struct_tree.__Instance;
            __Internal.FPDF_StructTree_Close(__arg0);
        }

        /// <summary>Count the number of children for the structure tree.</summary>
        /// <param name="struct_tree">Handle to the structure tree, as returned by FPDF_StructTree_LoadPage().</param>
        /// <returns>The number of children, or -1 on error.</returns>
        public static int FPDF_StructTree_CountChildren(global::PdfLibCore.Generated.FPDF_Structtree struct_tree)
        {
            var __arg0 = struct_tree is null ? __IntPtr.Zero : struct_tree.__Instance;
            var ___ret = __Internal.FPDF_StructTree_CountChildren(__arg0);
            return ___ret;
        }

        /// <summary>Get a child in the structure tree.</summary>
        /// <param name="struct_tree">Handle to the structure tree, as returned by FPDF_StructTree_LoadPage().</param>
        /// <param name="index">The index for the child, 0-based.</param>
        /// <returns>The child at the n-th index or NULL on error.</returns>
        public static global::PdfLibCore.Generated.FPDF_Structelement FPDF_StructTree_GetChildAtIndex(global::PdfLibCore.Generated.FPDF_Structtree struct_tree, int index)
        {
            var __arg0 = struct_tree is null ? __IntPtr.Zero : struct_tree.__Instance;
            var ___ret = __Internal.FPDF_StructTree_GetChildAtIndex(__arg0, index);
            var __result0 = global::PdfLibCore.Generated.FPDF_Structelement.__CreateInstance(___ret, false);
            return __result0;
        }

        /// <summary>Get the alt text for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="buffer">A buffer for output the alt text. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the alt text, including the terminating NUL
        /// character. The number of bytes is returned regardless of the
        /// |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetAltText(global::PdfLibCore.Generated.FPDF_Structelement struct_element, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetAltText(__arg0, buffer, buflen);
            return ___ret;
        }

        /// <summary>Get the actual text for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="buffer">A buffer for output the actual text. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the actual text, including the terminating
        /// NUL character. The number of bytes is returned regardless of the
        /// |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetActualText(global::PdfLibCore.Generated.FPDF_Structelement struct_element, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetActualText(__arg0, buffer, buflen);
            return ___ret;
        }

        /// <summary>Get the ID for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="buffer">A buffer for output the ID string. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the ID string, including the terminating NUL
        /// character. The number of bytes is returned regardless of the
        /// |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetID(global::PdfLibCore.Generated.FPDF_Structelement struct_element, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetID(__arg0, buffer, buflen);
            return ___ret;
        }

        /// <summary>Get the case-insensitive IETF BCP 47 language code for an element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="buffer">A buffer for output the lang string. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the ID string, including the terminating NUL
        /// character. The number of bytes is returned regardless of the
        /// |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetLang(global::PdfLibCore.Generated.FPDF_Structelement struct_element, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetLang(__arg0, buffer, buflen);
            return ___ret;
        }

        /// <summary>Get a struct element attribute of type &quot;name&quot; or &quot;string&quot;.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="attr_name">The name of the attribute to retrieve.</param>
        /// <param name="buffer">A buffer for output. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the attribute value, including the
        /// terminating NUL character. The number of bytes is returned
        /// regardless of the |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetStringAttribute(global::PdfLibCore.Generated.FPDF_Structelement struct_element, string attr_name, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetStringAttribute(__arg0, attr_name, buffer, buflen);
            return ___ret;
        }

        /// <summary>Get the marked content ID for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <returns>The marked content ID of the element. If no ID exists, returns -1.</returns>
        public static int FPDF_StructElement_GetMarkedContentID(global::PdfLibCore.Generated.FPDF_Structelement struct_element)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetMarkedContentID(__arg0);
            return ___ret;
        }

        /// <summary>Get the type (/S) for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="buffer">A buffer for output. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the type, including the terminating NUL
        /// character. The number of bytes is returned regardless of the
        /// |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetType(global::PdfLibCore.Generated.FPDF_Structelement struct_element, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetType(__arg0, buffer, buflen);
            return ___ret;
        }

        /// <summary>Get the object type (/Type) for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="buffer">A buffer for output. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the object type, including the terminating
        /// NUL character. The number of bytes is returned regardless of the
        /// |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetObjType(global::PdfLibCore.Generated.FPDF_Structelement struct_element, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetObjType(__arg0, buffer, buflen);
            return ___ret;
        }

        /// <summary>Get the title (/T) for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="buffer">A buffer for output. May be NULL.</param>
        /// <param name="buflen">The length of the buffer, in bytes. May be 0.</param>
        /// <remarks>
        /// Regardless of the platform, the |buffer| is always in UTF-16LE
        /// encoding. The string is terminated by a UTF16 NUL character. If
        /// |buflen| is less than the required length, or |buffer| is NULL,
        /// |buffer| will not be modified.
        /// </remarks>
        /// <returns>
        /// The number of bytes in the title, including the terminating NUL
        /// character. The number of bytes is returned regardless of the
        /// |buffer| and |buflen| parameters.
        /// </returns>
        public static uint FPDF_StructElement_GetTitle(global::PdfLibCore.Generated.FPDF_Structelement struct_element, __IntPtr buffer, uint buflen)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetTitle(__arg0, buffer, buflen);
            return ___ret;
        }

        /// <summary>Count the number of children for the structure element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <returns>The number of children, or -1 on error.</returns>
        public static int FPDF_StructElement_CountChildren(global::PdfLibCore.Generated.FPDF_Structelement struct_element)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_CountChildren(__arg0);
            return ___ret;
        }

        /// <summary>Get a child in the structure element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="index">The index for the child, 0-based.</param>
        /// <remarks>
        /// If the child exists but is not an element, then this function will
        /// return NULL. This will also return NULL for out of bounds indices.
        /// </remarks>
        /// <returns>The child at the n-th index or NULL on error.</returns>
        public static global::PdfLibCore.Generated.FPDF_Structelement FPDF_StructElement_GetChildAtIndex(global::PdfLibCore.Generated.FPDF_Structelement struct_element, int index)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetChildAtIndex(__arg0, index);
            var __result0 = global::PdfLibCore.Generated.FPDF_Structelement.__CreateInstance(___ret, false);
            return __result0;
        }

        /// <summary>Get the parent of the structure element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <remarks>If structure element is StructTreeRoot, then this function will return NULL.</remarks>
        /// <returns>The parent structure element or NULL on error.</returns>
        public static global::PdfLibCore.Generated.FPDF_Structelement FPDF_StructElement_GetParent(global::PdfLibCore.Generated.FPDF_Structelement struct_element)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetParent(__arg0);
            var __result0 = global::PdfLibCore.Generated.FPDF_Structelement.__CreateInstance(___ret, false);
            return __result0;
        }

        /// <summary>Count the number of attributes for the structure element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <returns>The number of attributes, or -1 on error.</returns>
        public static int FPDF_StructElement_GetAttributeCount(global::PdfLibCore.Generated.FPDF_Structelement struct_element)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetAttributeCount(__arg0);
            return ___ret;
        }

        /// <summary>Get an attribute object in the structure element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="index">The index for the attribute object, 0-based.</param>
        /// <remarks>
        /// If the attribute object exists but is not a dict, then this
        /// function will return NULL. This will also return NULL for out of
        /// bounds indices.
        /// </remarks>
        /// <returns>The attribute object at the n-th index or NULL on error.</returns>
        public static global::PdfLibCore.Generated.FPDF_Structelement_attr FPDF_StructElement_GetAttributeAtIndex(global::PdfLibCore.Generated.FPDF_Structelement struct_element, int index)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetAttributeAtIndex(__arg0, index);
            var __result0 = global::PdfLibCore.Generated.FPDF_Structelement_attr.__CreateInstance(___ret, false);
            return __result0;
        }

        /// <summary>Count the number of attributes in a structure element attribute map.</summary>
        /// <param name="struct_attribute">Handle to the struct element attribute.</param>
        /// <returns>The number of attributes, or -1 on error.</returns>
        public static int FPDF_StructElement_Attr_GetCount(global::PdfLibCore.Generated.FPDF_Structelement_attr struct_attribute)
        {
            var __arg0 = struct_attribute is null ? __IntPtr.Zero : struct_attribute.__Instance;
            var ___ret = __Internal.FPDF_StructElement_Attr_GetCount(__arg0);
            return ___ret;
        }

        /// <summary>Get the name of an attribute in a structure element attribute map.</summary>
        /// <param name="struct_attribute">Handle to the struct element attribute.</param>
        /// <param name="index">The index of attribute in the map.</param>
        /// <param name="buffer">
        /// A buffer for output. May be NULL. This is only
        /// modified if |buflen| is longer than the length
        /// of the key. Optional, pass null to just
        /// retrieve the size of the buffer needed.
        /// </param>
        /// <param name="buflen">The length of the buffer.</param>
        /// <param name="out_buflen">
        /// A pointer to variable that will receive the
        /// minimum buffer size to contain the key. Not
        /// filled if FALSE is returned.
        /// </param>
        /// <returns>TRUE if the operation was successful, FALSE otherwise.</returns>
        public static global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetName(global::PdfLibCore.Generated.FPDF_Structelement_attr struct_attribute, int index, __IntPtr buffer, uint buflen, ref uint out_buflen)
        {
            var __arg0 = struct_attribute is null ? __IntPtr.Zero : struct_attribute.__Instance;
            fixed (uint* __out_buflen4 = &out_buflen)
            {
                var __arg4 = __out_buflen4;
                var ___ret = __Internal.FPDF_StructElement_Attr_GetName(__arg0, index, buffer, buflen, __arg4);
                return ___ret;
            }
        }

        /// <summary>Get the type of an attribute in a structure element attribute map.</summary>
        /// <param name="struct_attribute">Handle to the struct element attribute.</param>
        /// <param name="name">The attribute name.</param>
        /// <returns>Returns the type of the value, or FPDF_OBJECT_UNKNOWN in case of failure.</returns>
        public static int FPDF_StructElement_Attr_GetType(global::PdfLibCore.Generated.FPDF_Structelement_attr struct_attribute, string name)
        {
            var __arg0 = struct_attribute is null ? __IntPtr.Zero : struct_attribute.__Instance;
            var ___ret = __Internal.FPDF_StructElement_Attr_GetType(__arg0, name);
            return ___ret;
        }

        /// <summary>
        /// Get the value of a boolean attribute in an attribute map by name as
        /// FPDF_BOOL. FPDF_StructElement_Attr_GetType() should have returned
        /// FPDF_OBJECT_BOOLEAN for this property.
        /// </summary>
        /// <param name="struct_attribute">Handle to the struct element attribute.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="out_value">A pointer to variable that will receive the value. Not filled if false is returned.</param>
        /// <returns>Returns TRUE if the name maps to a boolean value, FALSE otherwise.</returns>
        public static global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetBooleanValue(global::PdfLibCore.Generated.FPDF_Structelement_attr struct_attribute, string name, ref global::PdfLibCore.Types.FPDF_BOOL out_value)
        {
            var __arg0 = struct_attribute is null ? __IntPtr.Zero : struct_attribute.__Instance;
            fixed (global::PdfLibCore.Types.FPDF_BOOL* __out_value2 = &out_value)
            {
                var __arg2 = __out_value2;
                var ___ret = __Internal.FPDF_StructElement_Attr_GetBooleanValue(__arg0, name, __arg2);
                return ___ret;
            }
        }

        /// <summary>
        /// Get the value of a number attribute in an attribute map by name as
        /// float. FPDF_StructElement_Attr_GetType() should have returned
        /// FPDF_OBJECT_NUMBER for this property.
        /// </summary>
        /// <param name="struct_attribute">Handle to the struct element attribute.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="out_value">A pointer to variable that will receive the value. Not filled if false is returned.</param>
        /// <returns>Returns TRUE if the name maps to a number value, FALSE otherwise.</returns>
        public static global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetNumberValue(global::PdfLibCore.Generated.FPDF_Structelement_attr struct_attribute, string name, ref float out_value)
        {
            var __arg0 = struct_attribute is null ? __IntPtr.Zero : struct_attribute.__Instance;
            fixed (float* __out_value2 = &out_value)
            {
                var __arg2 = __out_value2;
                var ___ret = __Internal.FPDF_StructElement_Attr_GetNumberValue(__arg0, name, __arg2);
                return ___ret;
            }
        }

        /// <summary>
        /// Get the value of a string attribute in an attribute map by name as
        /// string. FPDF_StructElement_Attr_GetType() should have returned
        /// FPDF_OBJECT_STRING or FPDF_OBJECT_NAME for this property.
        /// </summary>
        /// <param name="struct_attribute">Handle to the struct element attribute.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="buffer">
        /// A buffer for holding the returned key in
        /// UTF-16LE. This is only modified if |buflen| is
        /// longer than the length of the key. Optional,
        /// pass null to just retrieve the size of the
        /// buffer needed.
        /// </param>
        /// <param name="buflen">The length of the buffer.</param>
        /// <param name="out_buflen">
        /// A pointer to variable that will receive the
        /// minimum buffer size to contain the key. Not
        /// filled if FALSE is returned.
        /// </param>
        /// <returns>Returns TRUE if the name maps to a string value, FALSE otherwise.</returns>
        public static global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetStringValue(global::PdfLibCore.Generated.FPDF_Structelement_attr struct_attribute, string name, __IntPtr buffer, uint buflen, ref uint out_buflen)
        {
            var __arg0 = struct_attribute is null ? __IntPtr.Zero : struct_attribute.__Instance;
            fixed (uint* __out_buflen4 = &out_buflen)
            {
                var __arg4 = __out_buflen4;
                var ___ret = __Internal.FPDF_StructElement_Attr_GetStringValue(__arg0, name, buffer, buflen, __arg4);
                return ___ret;
            }
        }

        /// <summary>Get the value of a blob attribute in an attribute map by name as string.</summary>
        /// <param name="struct_attribute">Handle to the struct element attribute.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="buffer">
        /// A buffer for holding the returned value. This
        /// is only modified if |buflen| is at least as
        /// long as the length of the value. Optional, pass
        /// null to just retrieve the size of the buffer
        /// needed.
        /// </param>
        /// <param name="buflen">The length of the buffer.</param>
        /// <param name="out_buflen">
        /// A pointer to variable that will receive the
        /// minimum buffer size to contain the key. Not
        /// filled if FALSE is returned.
        /// </param>
        /// <returns>Returns TRUE if the name maps to a string value, FALSE otherwise.</returns>
        public static global::PdfLibCore.Types.FPDF_BOOL FPDF_StructElement_Attr_GetBlobValue(global::PdfLibCore.Generated.FPDF_Structelement_attr struct_attribute, string name, __IntPtr buffer, uint buflen, ref uint out_buflen)
        {
            var __arg0 = struct_attribute is null ? __IntPtr.Zero : struct_attribute.__Instance;
            fixed (uint* __out_buflen4 = &out_buflen)
            {
                var __arg4 = __out_buflen4;
                var ___ret = __Internal.FPDF_StructElement_Attr_GetBlobValue(__arg0, name, buffer, buflen, __arg4);
                return ___ret;
            }
        }

        /// <summary>Get the count of marked content ids for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        public static int FPDF_StructElement_GetMarkedContentIdCount(global::PdfLibCore.Generated.FPDF_Structelement struct_element)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetMarkedContentIdCount(__arg0);
            return ___ret;
        }

        /// <summary>Get the marked content id at a given index for a given element.</summary>
        /// <param name="struct_element">Handle to the struct element.</param>
        /// <param name="index">The index of the marked content id, 0-based.</param>
        /// <returns>The marked content ID of the element. If no ID exists, returns -1.</returns>
        public static int FPDF_StructElement_GetMarkedContentIdAtIndex(global::PdfLibCore.Generated.FPDF_Structelement struct_element, int index)
        {
            var __arg0 = struct_element is null ? __IntPtr.Zero : struct_element.__Instance;
            var ___ret = __Internal.FPDF_StructElement_GetMarkedContentIdAtIndex(__arg0, index);
            return ___ret;
        }
    }
}
