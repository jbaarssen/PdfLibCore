using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using PdfLibCore.Enums;
using PdfLibCore.Types;

// ReSharper disable UnusedMember.Local
namespace PdfLibCore
{
    /// <summary>
	/// Static class containing the native (imported) PDFium functions.
	/// In case of missing documentation, refer to the <see href="https://pdfium.googlesource.com/pdfium/+/master/public">PDFium header files</see>. 
	/// </summary>
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
	public static partial class Pdfium
    {
	    private delegate int GetStringHandler(ref byte buffer, int length);
	    
		/// <summary>
		/// Gets a value indicating whether the PDFium library is available.
		/// <c>false</c> is returned if the native libraries could not be
		/// loaded for some reason.
		/// </summary>
		public static bool IsAvailable { get; }

		static Pdfium() => 
			IsAvailable = Initialize();

		private static bool Initialize()
		{
			try
			{
				FPDF_InitLibrary();
			}
			catch
			{
				return false;
			}
			return true;
		}

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h
        /// <summary>
        /// Loads a PDF document from memory.
        /// </summary>
        /// <param name="data">The data to load the document from.</param>
        /// <param name="index">The index of the first byte to be copied from <paramref name="data"/>.</param>
        /// <param name="count">The number of bytes to copy from <paramref name="data"/> or a negative value to copy all bytes.</param>
        /// <param name="password">Pdf password</param>
        public static FPDF_DOCUMENT FPDF_LoadDocument(byte[] data, int index = 0, int count = -1, string password = null)
		{
			if (count < 0)
			{
				count = data.Length - index;
			}
			return FPDF_LoadMemDocument(ref data[index], count, password);
		}

		/// <summary>
		/// Loads a PDF document from a stream.
		/// </summary>
		/// <param name="fileRead"></param>
		/// <param name="password">Pdf password</param>
		public static FPDF_DOCUMENT FPDF_LoadDocument(Stream fileRead, string password = null) => 
			FPDF_LoadCustomDocument(FPDF_FILEREAD.FromStream(fileRead), password);

		public static string FPDF_VIEWERREF_GetName(FPDF_DOCUMENT document, string key)
		{
			byte b = 0;
			var length = FPDF_VIEWERREF_GetName(document, key, ref b, 0);
			if (length == 0)
			{
				return null;
			}
			var buffer = new byte[length];
			FPDF_VIEWERREF_GetName(document, key, ref buffer[0], length);
			return Encoding.UTF8.GetString(buffer);
		}

		/// <summary>
		/// Get the named destination by index.
		/// </summary>
		/// <param name="document">Handle to a document.</param>
		/// <param name="index">The index of a named destination.</param>
		/// <returns>
		/// The destination handle and name for a given index, or (<see cref="FPDF_DEST.Null"/>, <c>null</c>)
		/// if there is no named destination corresponding to <paramref name="index"/>.
		/// </returns>
		/// <seealso cref="PdfDestinationCollection"/>
		/// <seealso cref="PdfDocument.Destinations"/>
		public static (FPDF_DEST Destination, string Name) FPDF_GetNamedDest(FPDF_DOCUMENT document, int index)
		{
			FPDF_GetNamedDest(document, index, IntPtr.Zero, out var length);
			if (length < 1)
			{
				return (FPDF_DEST.Null, null);
			}
			var buffer = new byte[length];
			return (FPDF_GetNamedDest(document, index, ref buffer[0], ref length), Encoding.Unicode.GetString(buffer, 0, length - 2));
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_doc.h

		/// <summary>
		/// Get the title of <paramref name="bookmark"/>.
		/// </summary>
		/// <param name="bookmark">Handle to the bookmark.</param>
		/// <returns>The title of the bookmark.</returns>
		public static string FPDFBookmark_GetTitle(FPDF_BOOKMARK bookmark) => 
			GetUtf16String((ref byte buffer, int length) => (int)FPDFBookmark_GetTitle(bookmark, ref buffer, (uint)length), sizeof(byte), true);

		/// <summary>
		/// Gets the file path of a <see cref="FPDF_ACTION"/> of type <see cref="ActionTypes.RemoteGoTo"/> or <see cref="ActionTypes.Launch"/>.
		/// </summary>
		/// <param name="action">Handle to the action. Must be of type <see cref="ActionTypes.RemoteGoTo"/> or <see cref="ActionTypes.Launch"/>.</param>
		/// <returns>The file path of <paramref name="action"/>.</returns>
		/// <seealso cref="PdfAction.FilePath"/>
		public static string FPDFAction_GetFilePath(FPDF_ACTION action) => 
			GetUtf16String((ref byte buffer, int length) => (int)FPDFAction_GetFilePath(action, ref buffer, (uint)length), sizeof(byte), true);

		/// <summary>
		/// Gets URI path of a <see cref="FPDF_ACTION"/> of type <see cref="ActionTypes.Uri"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="action">Handle to the action. Must be of type <see cref="ActionTypes.Uri"/>.</param>
		/// <returns>The URI path of <paramref name="action"/>.</returns>
		/// <seealso cref="PdfAction.Uri"/>
		public static string FPDFAction_GetURIPath(FPDF_DOCUMENT document, FPDF_ACTION action) => 
			GetAsciiString((ref byte buffer, int length) => (int)FPDFAction_GetURIPath(document, action, ref buffer, (uint)length));

		/// <summary>
		/// Enumerates all the link annotations in <paramref name="page"/>.
		/// </summary>
		/// <param name="page">Handle to the page.</param>
		/// <returns>All the link annotations in <paramref name="page"/>.</returns>
		public static IEnumerable<FPDF_LINK> FPDFLink_Enumerate(FPDF_PAGE page)
		{
			var pos = 0;
			while (FPDFLink_Enumerate(page, ref pos, out var link))
			{
				yield return link;
			}
		}

		/// <summary>
		/// Get meta-data <paramref name="tag"/> content from <paramref name="document"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="tag">
		/// The tag to retrieve. The tag can be one of:
		/// Title, Author, Subject, Keywords, Creator, Producer,
		/// CreationDate, or ModDate.
		/// </param>
		/// <returns>The meta-data.</returns>
		/// <remarks>
		/// For detailed explanations of these tags and their respective
		/// values, please refer to PDF Reference 1.6, section 10.2.1,
		/// 'Document Information Dictionary'.
		/// </remarks>
		/// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference</seealso>
		/// <seealso cref="PdfDocument.GetMetaText(MetadataTags)"/>
		public static string FPDF_GetMetaText(FPDF_DOCUMENT document, string tag) => 
			GetUtf16String((ref byte buffer, int length) => (int)FPDF_GetMetaText(document, tag, ref buffer, (uint)length), sizeof(byte), true);

		/// <summary>
		/// Get meta-data <paramref name="tag"/> content from <paramref name="document"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="tag">The tag to retrieve.</param>
		/// <returns>The meta-data.</returns>
		/// <remarks>
		/// For detailed explanations of these tags and their respective
		/// values, please refer to PDF Reference 1.6, section 10.2.1,
		/// 'Document Information Dictionary'.
		/// </remarks>
		/// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference</seealso>
		/// <seealso cref="PdfDocument.GetMetaText(MetadataTags)"/>
		public static string FPDF_GetMetaText(FPDF_DOCUMENT document, MetadataTags tag) => 
			FPDF_GetMetaText(document, tag.ToString());

		/// <summary>
		/// Get the page label for <paramref name="pageIndex"/> from <paramref name="document"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="pageIndex">The zero-based index of the page.</param>
		/// <returns>The page label.</returns>
		/// <seealso cref="PdfPage.Label"/>
		public static string FPDF_GetPageLabel(FPDF_DOCUMENT document, int pageIndex) => 
			GetUtf16String((ref byte buffer, int length) => (int)FPDF_GetPageLabel(document, pageIndex, ref buffer, (uint)length), sizeof(byte), true);

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_edit.h

		/// <summary>
		/// Insert <paramref name="pageObj"/> into <paramref name="page"/>.
		/// </summary>
		/// <param name="page">Handle to a page.</param>
		/// <param name="pageObj">Handle to a page object. The <paramref name="pageObj"/> will be automatically freed.</param>
		public static void FPDFPage_InsertObject(FPDF_PAGE page, ref FPDF_PAGEOBJECT pageObj)
		{
			FPDFPage_InsertObject(page, pageObj);
			pageObj = FPDF_PAGEOBJECT.Null;
		}

		/// <summary>
		/// Load an image from a JPEG image file and then set it into <paramref name="imageObject"/>.
		/// </summary>
		/// <param name="loadedPages">All loaded pages, may be <c>null</c>.</param>
		/// <param name="imageObject">Handle to an image object.</param>
		/// <param name="stream">Stream which provides access to an JPEG image.</param>
		/// <param name="count">The number of bytes to read from <paramref name="stream"/> or 0 to read to the end.</param>
		/// <param name="inline">
		/// If <c>true</c>, this function loads the JPEG image inline, so the image
		/// content is copied to the file. This allows <paramref name="stream"/>
		/// to be closed after this function returns.
		/// </param>
		/// <returns><c>true</c> on success.</returns>
		/// <remarks>
		/// The image object might already have an associated image, which is shared and
		/// cached by the loaded pages. In that case, we need to clear the cached image
		/// for all the loaded pages. Pass <paramref name="loadedPages"/> to this API
		/// to clear the image cache. If the image is not previously shared, <c>null</c> is a
		/// valid <paramref name="loadedPages"/> value.
		/// </remarks>
		public static bool FPDFImageObj_LoadJpegFile(FPDF_PAGE[] loadedPages, FPDF_PAGEOBJECT imageObject, Stream stream, int count = 0, bool inline = true) => inline 
				? FPDFImageObj_LoadJpegFileInline(ref loadedPages[0], loadedPages.Length, imageObject, FPDF_FILEREAD.FromStream(stream)) 
				: FPDFImageObj_LoadJpegFile(ref loadedPages[0], loadedPages.Length, imageObject, FPDF_FILEREAD.FromStream(stream));

		/// <summary>
		/// Set <paramref name="bitmap"/> to <paramref name="imageObject"/>.
		/// </summary>
		/// <param name="loadedPages">All loaded pages, may be <c>null</c>.</param>
		/// <param name="imageObject">Handle to an image object.</param>
		/// <param name="bitmap">Handle of the bitmap.</param>
		/// <returns><c>true</c> on success.</returns>
		public static bool FPDFImageObj_SetBitmap(FPDF_PAGE[] loadedPages, FPDF_PAGEOBJECT imageObject, FPDF_BITMAP bitmap) => 
			FPDFImageObj_SetBitmap(ref loadedPages[0], loadedPages.Length, imageObject, bitmap);

		/// <summary>
		/// Returns a font object loaded from a stream of data. The font is loaded
		/// into the document. The caller does not need to free the returned object.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="fontType">The font type.</param>
		/// <param name="cid">A value specifying if the font is a CID font or not.</param>
		/// <param name="data">The data, which will be copied by the font object.</param>
		/// <param name="index">The index of the first byte to be copied from <paramref name="data"/>.</param>
		/// <param name="count">The number of bytes to copy from <paramref name="data"/> or a negative value to copy all bytes.</param>
		/// <returns>Returns NULL on failure.</returns>
		public static FPDF_FONT FPDFText_LoadFont(FPDF_DOCUMENT document, FontTypes fontType, bool cid, byte[] data, int index = -1, int count = 0)
		{
			if (count < 0)
			{
				count = data.Length - index;
			}
			return FPDFText_LoadFont(document, ref data[index], (uint)count, fontType, cid);
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_ppo.h

		/// <summary>
		/// Imports pages from <paramref name="srcDoc"/> to <paramref name="destDoc"/>
		/// </summary>
		/// <param name="destDoc"></param>
		/// <param name="srcDoc"></param>
		/// <param name="index">Zero-based index of where the imported pages should be inserted in the destination document.</param>
		/// <param name="srcPageIndices">Zero-based indices of the pages to import in the source document</param>
		public static bool FPDF_ImportPages(FPDF_DOCUMENT destDoc, FPDF_DOCUMENT srcDoc, int index, params int[] srcPageIndices)
		{
			string pageRange = null;
			if (srcPageIndices != null && srcPageIndices.Length > 0)
			{
				pageRange = string.Join(",", srcPageIndices.Select(p => (p + 1).ToString(CultureInfo.InvariantCulture)));
			}
			return FPDF_ImportPages(destDoc, srcDoc, pageRange, index);
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_save.h

		/// <summary>
		/// Saves a PDF document to a stream.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="stream"></param>
		/// <param name="flags"></param>
		/// <param name="version">
		/// The new PDF file version of the saved file.
		/// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
		/// </param>
		/// <seealso cref="Pdfium.FPDF_SaveAsCopy(FPDF_DOCUMENT, FPDF_FILEWRITE, SaveFlags)"/>
		/// <seealso cref="Pdfium.FPDF_SaveWithVersion(FPDF_DOCUMENT, FPDF_FILEWRITE, SaveFlags, int)"/>
		public static bool FPDF_SaveAsCopy(FPDF_DOCUMENT document, Stream stream, SaveFlags flags, int version = 0)
		{
			byte[] buffer = null;
			var fileWrite = new FPDF_FILEWRITE((ignore, data, size) =>
			{
				if (buffer == null || buffer.Length < size)
				{
					buffer = new byte[size];
				}
				Marshal.Copy(data, buffer, 0, size);
				stream.Write(buffer, 0, size);
				return true;
			});

			return version >= 10 
				? FPDF_SaveWithVersion(document, fileWrite, flags, version) 
				: FPDF_SaveAsCopy(document, fileWrite, flags);
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_structtree.h

		/// <summary>
		/// Get the alternative text for a given element.
		/// </summary>
		/// <param name="structElement">Handle to the struct element.</param>
		/// <returns>The alternative text for <paramref name="structElement"/>.</returns>
		public static string FPDF_StructElement_GetAltText(FPDF_STRUCTELEMENT structElement) => 
			GetUtf16String((ref byte buffer, int length) => (int)FPDF_StructElement_GetAltText(structElement, ref buffer, (uint)length), sizeof(byte), true);

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_text.h

		public static string FPDFText_GetText(FPDF_TEXTPAGE textPage, int startIndex, int count)
		{
			var buffer = new byte[2 * (count + 1)];
			return Encoding.Unicode.GetString(buffer, 0, (FPDFText_GetText(textPage, startIndex, count, ref buffer[0]) - 1) * 2);
		}

		public static string FPDFText_GetBoundedText(FPDF_TEXTPAGE textPage, double left, double top, double right, double bottom) => 
			GetUtf16String((ref byte buffer, int length) => FPDFText_GetBoundedText(textPage, left, top, right, bottom, ref buffer, length), sizeof(ushort), false);

		public static string FPDFLink_GetURL(FPDF_PAGELINK linkPage, int linkIndex) => 
			GetUtf16String((ref byte buffer, int length) => FPDFLink_GetURL(linkPage, linkIndex, ref buffer, length), sizeof(ushort), true);

		#endregion
		
		private static string GetUtf16String(GetStringHandler handler, int lengthUnit, bool lengthIncludesTerminator)
		{
			var buffer = GetBuffer(handler, out var length);
			length *= lengthUnit;
			if (lengthIncludesTerminator)
			{
				length -= 2;
			}
			return Encoding.Unicode.GetString(buffer, 0, length > 0 ? length : 0);
		}

		private static string GetAsciiString(GetStringHandler handler)
		{
			var buffer = GetBuffer(handler, out var length);
			return Encoding.ASCII.GetString(buffer, 0, length - 1);
		}

		private static string GetUtf8String(GetStringHandler handler)
		{
			var buffer = GetBuffer(handler, out var length);
			return Encoding.UTF8.GetString(buffer, 0, length - 1);
		}

		private static byte[] GetBuffer(GetStringHandler handler, out int length)
		{
			byte b = 0;
			length = handler(ref b, 0);
			if (length == 0)
			{
				return Array.Empty<byte>();
			}
			var buffer = new byte[length];
			handler(ref buffer[0], length);
			return buffer;
		}
    }
}