using System;

namespace PdfLibCore.Readers
{
    public interface IPageReader : IDisposable
    {
        /// <summary>
        /// Page index.
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Get scaled page width.
        /// </summary>
        double GetPageWidth();

        /// <summary>
        /// Get scaled page high.
        /// </summary>
        double GetPageHeight();
        
        /// <summary>
        /// Return a byte representation
        /// of the page image.
        /// Byte array is formatted as
        /// B-G-R-A ordered list.
        /// </summary>
        /// <returns></returns>
        byte[] GetImage();
    }
}