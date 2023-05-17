using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using PdfLibCore.Enums;
using PdfLibCore.Generated;
using PdfLibCore.Helpers;
using PdfLibCore.Types;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace PdfLibCore;

public class PdfDocument : NativeWrapper<FPDF_Document>
{
    /// <summary>
    /// Gets the pages in the current <see cref="PdfDocument"/>.
    /// </summary>
    public PdfPageCollection Pages { get; }

    public PdfDestinationCollection Destinations { get; }

    /// <summary>
    /// Gets the PDF file version. File version: 14 for 1.4, 15 for 1.5, ...
    /// </summary>
    public int FileVersion
    {
        get
        {
            var fileVersion = 1;
            return Pdfium.FPDF_GetFileVersion(Handle, ref fileVersion) ? fileVersion : 1;
        }
    }

    /// <summary>
    /// Gets the revision of the security handler.
    /// </summary>
    /// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference: Table 21</seealso>
    public int SecurityHandlerRevision => Pdfium.FPDF_GetSecurityHandlerRevision(Handle);

    public DocumentPermissions Permissions => (DocumentPermissions) Pdfium.FPDF_GetDocPermissions(Handle);

    public bool PrintPrefersScaling => Pdfium.FPDF_VIEWERREF_GetPrintScaling(Handle);

    public int PrintCopyCount => Pdfium.FPDF_VIEWERREF_GetNumCopies(Handle);

    public DuplexTypes DuplexType => (DuplexTypes) Enum.Parse(typeof(DuplexTypes), Pdfium.FPDF_VIEWERREF_GetDuplex(Handle).ToString());

    public PdfPageRange PageRange => new(this, Pdfium.FPDF_VIEWERREF_GetPrintPageRange(Handle));

    public IEnumerable<PdfBookmark> Bookmarks
    {
        get
        {
            var handle = Pdfium.FPDFBookmark_GetFirstChild(Handle, null);
            while (!handle.IsNull())
            {
                yield return new PdfBookmark(this, handle);
                handle = Pdfium.FPDFBookmark_GetNextSibling(Handle, handle);
            }
        }
    }

    public PageModes PageMode => (PageModes) Pdfium.FPDFDoc_GetPageMode(Handle);

    private PdfDocument(FPDF_Document doc)
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
    public PdfDocument(string fileName, string? password = null)
        : this(Pdfium.FPDF_LoadDocument(fileName, password))
    {
    }

    /// <summary>
    /// Loads a <see cref="PdfDocument"/> from memory.
    /// <see cref="Close"/> must be called in order to free unmanaged resources.
    /// </summary>
    /// <param name="data">Byte array containing the bytes of the PDF document to load.</param>
    /// <param name="password"></param>
    public PdfDocument(byte[] data, string? password = null)
        : this(Pointer(data, ptr => Pdfium.FPDF_LoadMemDocument64(ptr, (ulong) data.Length, password)))
    {
    }

    /// <summary>
    /// Loads a <see cref="PdfDocument"/> from a <paramref name="stream"/>.
    /// <see cref="Close"/> must be called in order to free unmanaged resources.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="password"></param>
    public PdfDocument(Stream stream, string? password = null)
        : this(BinaryData.FromStream(stream).ToArray(), password)
    {
    }

    /// <summary>
    /// Closes the <see cref="PdfDocument"/> and frees unmanaged resources.
    /// </summary>
    public void Close() => ((IDisposable) this).Dispose();

    /// <summary>
    /// Saves the <see cref="PdfDocument"/> to a <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="flags"></param>
    /// <param name="version">
    /// The new PDF file version of the saved file.
    /// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
    /// </param>
    public bool Save(Stream stream, SaveFlags flags = SaveFlags.None, int version = 0)
    {
        byte[]? buffer = null;
        var writeHandle = new FPDF_FILEWRITE_
        {
            Version = version,
            WriteBlock = (_, data, size) =>
            {
                if (buffer == null || buffer.Length < size)
                {
                    buffer = new byte[size];
                }
                Marshal.Copy(data, buffer, 0, (int)size);
                stream.Write(buffer, 0, (int)size);
                return FPDF_BOOL.True;
            }
        };
        return Pdfium.FPDF_SaveWithVersion(Handle, writeHandle, (long) flags, version);
    }

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

    public PdfBookmark? FindBookmark(string title)
    {
        var handle = Pdfium.FPDFBookmark_Find(Handle, ref title);
        return handle.IsNull() ? null : new PdfBookmark(this, handle);
    }

    public string GetMetaText(MetadataTags tag) =>
        Helper.GetString((ptr, len) => Pdfium.FPDF_GetMetaText(Handle, tag.ToString(), ptr, len));

    public void CopyViewerPreferencesFrom(PdfDocument srcDoc) =>
        Pdfium.FPDF_CopyViewerPreferences(Handle, srcDoc.Handle);

    protected override void OnDispose(FPDF_Document handle)
    {
        Pages.Dispose();
        Pdfium.FPDF_CloseDocument(handle);
    }
}