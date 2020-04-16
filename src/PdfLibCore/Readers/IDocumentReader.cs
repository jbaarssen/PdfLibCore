using System;
using PdfLibCore.Models;

namespace PdfLibCore.Readers
{
    public interface IDocumentReader : IDisposable
    {
        /// <summary>
        /// PDF document version e.g. 1.7
        /// </summary>
        /// <returns>Version</returns>
        PdfVersion GetPdfVersion();

        /// <summary>
        /// Reads page count
        /// </summary>
        /// <returns>Page count</returns>
        int GetPageCount();

        /// <summary>
        /// Get page reader
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <returns>Page reader</returns>
        IPageReader GetPageReader(int pageIndex);
    }
}