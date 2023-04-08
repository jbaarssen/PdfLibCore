using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
namespace PdfLibCore
{
    public sealed class PdfPageCollection : List<PdfPage>, IDisposable
	{
		private readonly PdfDocument _doc;
		private readonly object _lock = new();

		public new int Count => Pdfium.FPDF_GetPageCount(_doc.Handle);
		
		internal PdfPageCollection(PdfDocument doc)
		{
			lock (_lock)
			{
				_doc = doc;
				for (var index = 0; index < Count; index++)
				{
					Add(PdfPage.Load(doc, index));
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="PdfPage"/> at the zero-based <paramref name="index"/> in the <see cref="PdfDocument"/>.
		/// </summary>
		public new PdfPage this[int index]
		{
			get
			{
				if (index >= Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}
				
				if (base[index] == null || base[index].IsDisposed)
				{
					base[index] = PdfPage.Load(_doc, index);
				}

				return base[index];
			}
		}

		public void Dispose()
		{
			foreach (IDisposable page in this)
			{
				page?.Dispose();
			}
			Clear();
		}

		/// <summary>
		/// Imports pages of <paramref name="sourceDocument"/> into the current <see cref="PdfDocument"/>.
		/// </summary>
		/// <seealso cref="Pdfium.FPDF_ImportPages(Types.FPDF_DOCUMENT, Types.FPDF_DOCUMENT, int, int[])"/>
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
		/// <seealso cref="Pdfium.FPDF_ImportPages(Types.FPDF_DOCUMENT, Types.FPDF_DOCUMENT, int, int[])"/>
		public bool Insert(int index, PdfDocument sourceDocument, params int[] srcPageIndices)
		{
			if (index <= Count)
            {
                var result = Pdfium.FPDF_ImportPages(_doc.Handle, sourceDocument.Handle, index, srcPageIndices);
                if (!result)
                {
	                return false;
                }
                InsertRange(index, Enumerable.Repeat<PdfPage>(null, srcPageIndices.Length));
                for (var i = index; i < Count; i++)
                {
	                if (this[i] != null)
	                {
		                this[i].Index = i;
	                }
                }
            }
            else
            {
	            throw new ArgumentOutOfRangeException(nameof(index));
            }

            return true;
		}

		/// <summary>
		/// Inserts a new page at <paramref name="index"/>.
		/// </summary>
		public PdfPage Insert(int index, double width, double height)
		{
            PdfPage page;
            if (index <= Count)
			{
                page = PdfPage.New(_doc, index, width, height);
                Insert(index, page);
				for (var i = index; i < Count; i++)
				{
					if (this[i] != null)
					{
						this[i].Index = i;
					}
				}
			}
            else
            {
	            throw new ArgumentOutOfRangeException(nameof(index));
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
				((IDisposable)this[index])?.Dispose();
				for (var i = index; i < Count; i++)
				{
					if (this[i] != null)
					{
						this[i].Index = i;
					}
				}
			}
			Pdfium.FPDFPage_Delete(_doc.Handle, index);
			base.RemoveAt(index);
		}
	}
}