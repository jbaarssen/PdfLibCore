using PdfLibCore.Enums;
using PdfLibCore.Types;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace PdfLibCore
{
	public sealed class PdfPage : NativeWrapper<FPDF_PAGE>
	{
		/// <summary>
		/// Gets the page width (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public double Width => Pdfium.FPDF_GetPageWidth(Handle);

		/// <summary>
		/// Gets the page height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public double Height => Pdfium.FPDF_GetPageHeight(Handle);

		/// <summary>
		/// Gets the page width and height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public (double Width, double Height) Size => 
			Pdfium.FPDF_GetPageSizeByIndex(Document.Handle, Index, out var width, out var height) ? (width, height) : throw new PdfiumException();

		/// <summary>
		/// Gets the page orientation.
		/// </summary>
		public PageOrientations Orientation
		{
			get => Pdfium.FPDFPage_GetRotation(Handle);
			set => Pdfium.FPDFPage_SetRotation(Handle, value);
		}

		/// <summary>
		/// Get the transparency of the page
		/// </summary>
		public bool HasTransparency => Pdfium.FPDFPage_HasTransparency(Handle);

		/// <summary>
		/// Gets the zero-based index of the page in the <see cref="NativeWrapper{T}.Document"/>
		/// </summary>
		public int Index { get; internal set; }

		public string Label => Pdfium.FPDF_GetPageLabel(Document.Handle, Index);

		private PdfPage(PdfDocument doc, FPDF_PAGE page, int index)
			: base(doc, page)
		{
			Index = index;
		}

		internal static PdfPage Load(PdfDocument doc, int index) => new(doc, Pdfium.FPDF_LoadPage(doc.Handle, index), index);

		internal static PdfPage New(PdfDocument doc, int index, double width, double height) => new(doc, Pdfium.FPDFPage_New(doc.Handle, index, width, height), index);

		/// <summary>
		/// Renders the page to a <see cref="PdfiumBitmap"/>
		/// </summary>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public void Render(PdfiumBitmap renderTarget, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None) => 
			Render(renderTarget, (0, 0, renderTarget.Width, renderTarget.Height), orientation, flags);
		
		/// <summary>
		/// Renders the page to a <see cref="PdfiumBitmap"/>
		/// </summary>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="rectDest">The destination rectangle in <paramref name="renderTarget"/>.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public void Render(PdfiumBitmap renderTarget, (int left, int top, int width, int height) rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None) => 
			Pdfium.FPDF_RenderPageBitmap(renderTarget.Handle, Handle, rectDest.left, rectDest.top, rectDest.width, rectDest.height, orientation, flags);
		
		public (double X, double Y) DeviceToPage((int left, int top, int width, int height) displayArea, int deviceX, int deviceY, PageOrientations orientation = PageOrientations.Normal)
		{
			var (left, top, width, height) = displayArea;
			Pdfium.FPDF_DeviceToPage(Handle, left, top, width, height, orientation, deviceX, deviceY, out var x, out var y);
			return (x, y);
		}

		public (int X, int Y) PageToDevice((int left, int top, int width, int height) displayArea, double pageX, double pageY, PageOrientations orientation = PageOrientations.Normal)
		{
			var (left, top, width, height) = displayArea;
			Pdfium.FPDF_PageToDevice(Handle, left, top, width, height, orientation, pageX, pageY, out var x, out var y);
			return (x, y);
		}

		public FlattenResults Flatten(FlattenFlags flags) => 
			Pdfium.FPDFPage_Flatten(Handle, flags);

		protected override void Dispose(FPDF_PAGE handle) => 
			Pdfium.FPDF_ClosePage(handle);
	}
}