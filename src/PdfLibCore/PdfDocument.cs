using System;
using System.Collections.Generic;
using System.IO;
using PdfLibCore.Enums;
using PdfLibCore.Types;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace PdfLibCore
{
    public class PdfDocument : NativeWrapper<FPDF_DOCUMENT>
    {
        /// <summary>
        /// Gets the pages in the current <see cref="PdfDocument"/>.
        /// </summary>
        public PdfPageCollection Pages { get; }

        public PdfDestinationCollection Destinations { get; }

        /// <summary>
        /// Gets the PDF file version. File version: 14 for 1.4, 15 for 1.5, ...
        /// </summary>
        public int FileVersion => 
            Pdfium.FPDF_GetFileVersion(Handle, out var fileVersion) ? fileVersion : 1;

        /// <summary>
        /// Gets the revision of the security handler.
        /// </summary>
        /// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference: Table 21</seealso>
        public int SecurityHandlerRevision => Pdfium.FPDF_GetSecurityHandlerRevision(Handle);

        public DocumentPermissions Permissions => Pdfium.FPDF_GetDocPermissions(Handle);

        public bool PrintPrefersScaling => Pdfium.FPDF_VIEWERREF_GetPrintScaling(Handle);
        
        public int PrintCopyCount => Pdfium.FPDF_VIEWERREF_GetNumCopies(Handle);

        public DuplexTypes DuplexType => Pdfium.FPDF_VIEWERREF_GetDuplex(Handle);

        public PdfPageRange PageRange => new(this, Pdfium.FPDF_VIEWERREF_GetPrintPageRange(Handle));

        public IEnumerable<PdfBookmark> Bookmarks
        {
            get
            {
                var handle = Pdfium.FPDFBookmark_GetFirstChild(Handle, FPDF_BOOKMARK.Null);
                while (!handle.IsNull)
                {
                    yield return new PdfBookmark(this, handle);
                    handle = Pdfium.FPDFBookmark_GetNextSibling(this.Handle, handle);
                }
            }
        }

        public PageModes PageMode => Pdfium.FPDFDoc_GetPageMode(Handle);

        private PdfDocument(FPDF_DOCUMENT doc)
            : base(doc)
        {
            Pages = new PdfPageCollection(this);
            Destinations = new PdfDestinationCollection(this);
        }

        /// <summary>
        /// Creates a new <see cref="PdfDocument"/>.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        public PdfDocument()
            : this(Pdfium.FPDF_CreateNewDocument())
        {
        }

        /// <summary>
        /// Loads a <see cref="PdfDocument"/> from the file system.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        /// <param name="fileName">Filepath of the PDF file to load.</param>
        /// <param name="password"></param>
        public PdfDocument(string fileName, string password = null)
            : this(Pdfium.FPDF_LoadDocument(fileName, password))
        {
        }

        /// <summary>
        /// Loads a <see cref="PdfDocument"/> from memory.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        /// <param name="data">Byte array containing the bytes of the PDF document to load.</param>
        /// <param name="index">The index of the first byte to be copied from <paramref name="data"/>.</param>
        /// <param name="count">The number of bytes to copy from <paramref name="data"/> or a negative value to copy all bytes.</param>
        /// <param name="password"></param>
        public PdfDocument(byte[] data, int index = 0, int count = -1, string password = null)
            : this(Pdfium.FPDF_LoadDocument(data, index, count, password))
        {
        }

        /// <summary>
        /// Loads a <see cref="PdfDocument"/> from a <paramref name="stream"/>.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="password"></param>
        public PdfDocument(Stream stream, string password = null)
            : this(Pdfium.FPDF_LoadDocument(GetBytes(stream), 0, -1, password))
        {
        }

        /// <summary>
        /// Closes the <see cref="PdfDocument"/> and frees unmanaged resources.
        /// </summary>
        public void Close() => ((IDisposable)this).Dispose();

        /// <summary>
        /// Saves the <see cref="PdfDocument"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="flags"></param>
        /// <param name="version">
        /// The new PDF file version of the saved file.
        /// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
        /// </param>
        public bool Save(Stream stream, SaveFlags flags = SaveFlags.None, int version = 0) => 
            Pdfium.FPDF_SaveAsCopy(Handle, stream, flags, version);

        /// <summary>
        /// Saves the <see cref="PdfDocument"/> to the file system.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="flags"></param>
        /// <param name="version">
        /// The new PDF file version of the saved file.
        /// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
        /// </param>
        public bool Save(string filename, SaveFlags flags = SaveFlags.None, int version = 0)
        {
            using var stream = new FileStream(filename, FileMode.Create);
            return Save(stream, flags, version);
        }

        public PdfBookmark FindBookmark(string title)
        {
            var handle = Pdfium.FPDFBookmark_Find(Handle, title);
            return handle.IsNull ? null : new PdfBookmark(this, handle);
        }

        public string GetMetaText(MetadataTags tag) => 
            Pdfium.FPDF_GetMetaText(Handle, tag);

        public void CopyViewerPreferencesFrom(PdfDocument srcDoc) => 
            Pdfium.FPDF_CopyViewerPreferences(Handle, srcDoc.Handle);

        protected override void Dispose(FPDF_DOCUMENT handle)
        {
            Pages.Dispose();
            Pdfium.FPDF_CloseDocument(handle);
        }
        
        private static byte[] GetBytes(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}