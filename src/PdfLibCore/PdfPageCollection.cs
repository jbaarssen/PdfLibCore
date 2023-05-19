using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PdfLibCore.Generated;
using PdfLibCore.Types;


// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
namespace PdfLibCore;

public sealed class PdfPageCollection : List<PdfPage?>, IDisposable
{
    private readonly PdfDocument _doc;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public new int Count => Pdfium.FPDF_GetPageCount(_doc.Handle);

    internal PdfPageCollection(PdfDocument doc)
    {
        _semaphore.Wait();
        try
        {
            _doc = doc;
            for (var index = 0; index < Count; index++)
            {
                Add(PdfPage.Load(doc, index));
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Gets the <see cref="PdfPage"/> at the zero-based <paramref name="index"/> in the <see cref="PdfDocument"/>.
    /// </summary>
    public new PdfPage? this[int index]
    {
        get
        {
            if (index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (base[index] == null || base[index]!.IsDisposed)
            {
                base[index] = PdfPage.Load(_doc, index);
            }

            return base[index];
        }
    }

    public void Dispose()
    {
        foreach (var page in this)
        {
            page?.Dispose();
        }
        Clear();
    }

    /// <summary>
    /// Imports pages of <paramref name="sourceDocument"/> into the current <see cref="PdfDocument"/>.
    /// </summary>
    /// <seealso cref="Generated.Pdfium.FPDF_ImportPages(FPDF_Document, FPDF_Document, string, int)"/>
    public bool Add(PdfDocument sourceDocument, params int[] srcPageIndices) =>
        Insert(Count, sourceDocument, srcPageIndices);

    /// <summary>
    /// Adds a new page to the end of the document.
    /// </summary>
    public PdfPage Add(double width, double height) =>
        Insert(Count, width, height);

    /// <summary>
    /// Imports pages of <paramref name="sourceDocument"/> into the current <see cref="PdfDocument"/>.
    /// </summary>
    /// <seealso cref="Generated.Pdfium.FPDF_ImportPages(FPDF_Document, FPDF_Document, string, int)"/>
    public bool Insert(int index, PdfDocument sourceDocument, params int[] srcPageIndices)
    {
        if (index > Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (Pdfium.FPDF_ImportPages(_doc.Handle, sourceDocument.Handle, string.Join(",", srcPageIndices), index) != FPDF_BOOL.True)
        {
            return false;
        }
        InsertRange(index, Enumerable.Repeat<PdfPage?>(null, srcPageIndices.Length));
        for (var i = index; i < Count; i++)
        {
            if (this[i] != null)
            {
                this[i]!.Index = i;
            }
        }
        return true;
    }

    /// <summary>
    /// Inserts a new page at <paramref name="index"/>.
    /// </summary>
    public PdfPage Insert(int index, double width, double height)
    {
        if (index > Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var page = PdfPage.New(_doc, index, width, height);
        Insert(index, page);
        for (var i = index; i < Count; i++)
        {
            if (this[i] != null)
            {
                this[i]!.Index = i;
            }
        }
        return page;
    }

    /// <summary>
    /// Removes the <paramref name="page"/> from the document.
    /// </summary>
    public new void Remove(PdfPage page) => RemoveAt(page.Index);

    /// <summary>
    /// Removes the page at <paramref name="index"/>.
    /// </summary>
    public new void RemoveAt(int index)
    {
        if (index < Count)
        {
            ((IDisposable?) this[index])?.Dispose();
            for (var i = index; i < Count; i++)
            {
                if (this[i] != null)
                {
                    this[i]!.Index = i;
                }
            }
        }
        Pdfium.FPDFPage_Delete(_doc.Handle, index);
        base.RemoveAt(index);
    }
}