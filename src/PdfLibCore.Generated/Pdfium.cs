// ReSharper disable once CheckNamespace
namespace PdfLibCore.Generated;

public static partial class Pdfium
{
    public static bool IsAvailable { get; }

	static Pdfium()
	{
		IsAvailable = Initialize();
	}

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
}