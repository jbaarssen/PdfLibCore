using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CppSharp.Runtime;
using PdfLibCore.Generated;

namespace PdfLibCore;

public sealed class PdfDestinationCollection : IEnumerable<PdfDestination>
{
    private readonly PdfDocument _doc;

    public int Count => (int) Pdfium.FPDF_CountNamedDests(_doc.Handle);

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

            Pdfium.FPDF_GetNamedDest(_doc.Handle, index, IntPtr.Zero, out var size);
            return Unmanaged.WithHandle(new byte[size], (ptr, target) =>
            {
                var destination = Pdfium.FPDF_GetNamedDest(_doc.Handle, index, ptr, out size);
                return destination.IsNull()
                    ? null
                    : new PdfDestination(_doc, destination, MarshalUtil.GetString(target, size));
            });
        }
    }

    internal PdfDestinationCollection(PdfDocument doc)
    {
        _doc = doc;
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