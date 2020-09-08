using System;
using System.Collections;
using System.Collections.Generic;

namespace PdfLibCore
{
    public sealed class PdfDestinationCollection : IEnumerable<PdfDestination>
    {
        readonly PdfDocument _doc;

        /// 
        public int Count => Pdfium.FPDF_CountNamedDests(_doc.Handle);

        internal PdfDestinationCollection(PdfDocument doc)
        {
            _doc = doc;
        }

        public PdfDestination this[string name]
        {
            get
            {
                var handle = Pdfium.FPDF_GetNamedDestByName(_doc.Handle, name);
                return handle.IsNull ? null : new PdfDestination(_doc, handle, name);
            }
        }

        public PdfDestination this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                (var handle, var name) = Pdfium.FPDF_GetNamedDest(_doc.Handle, index);
                return handle.IsNull ? null : new PdfDestination(_doc, handle, name);
            }
        }

        IEnumerator<PdfDestination> IEnumerable<PdfDestination>.GetEnumerator()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
                yield return this[i];
        }
    }
}