using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PdfLibCore.Generated;

namespace PdfLibCore;

public sealed class PdfDestinationCollection : IEnumerable<PdfDestination>
{
    private readonly PdfDocument _doc;

    public int Count => (int) Pdfium.FPDF_CountNamedDests(_doc.Handle);

    internal PdfDestinationCollection(PdfDocument doc) =>
        _doc = doc;

    public PdfDestination? this[string name]
    {
        get
        {
            var handle = Pdfium.FPDF_GetNamedDestByName(_doc.Handle, name);
            return handle.IsNull() ? null : new PdfDestination(_doc, handle, name);
        }
    }

    public PdfDestination? this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var size = 0;
            Pdfium.FPDF_GetNamedDest(_doc.Handle, index, IntPtr.Zero, ref size);
            var buffer = GCHandle.Alloc(new byte[size], GCHandleType.Pinned);
            try
            {
                var destination = Pdfium.FPDF_GetNamedDest(_doc.Handle, index, buffer.AddrOfPinnedObject(), ref size);
                var name =  Encoding.Unicode.GetString((byte[])buffer.Target, 0, size);
                return destination.IsNull() ? null : new PdfDestination(_doc, destination, name);
            }
            finally
            {
                buffer.Free();
            }
        }
    }

    IEnumerator<PdfDestination> IEnumerable<PdfDestination>.GetEnumerator()
    {
        var count = Count;
        for (var i = 0; i < count; i++)
        {
            if (this[i] is { } destination)
            {
                yield return destination;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        var count = Count;
        for (var i = 0; i < count; i++)
        {
            yield return this[i];
        }
    }
}