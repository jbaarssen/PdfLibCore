using System;
using PdfLibCore.Readers;

namespace PdfLibCore
{
    /// <inheritdoc />
    /// <summary>
    /// DocNet library object.
    /// Should be long lived and only
    /// disposed once.
    /// </summary>
    public interface IPdfLib : IDisposable
    {
        /// <summary>
        /// Get document reader for this particular document.
        /// dimOne x dimTwo represents a viewport to which
        /// the document gets scaled to fit without modifying
        /// it's aspect ratio.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <returns>Document reader object</returns>
        IDocumentReader GetDocumentReader(string filePath);

        /// <summary>
        /// Get document reader for this particular document.
        /// dimOne x dimTwo represents a viewport to which
        /// the document gets scaled to fit without modifying
        /// it's aspect ratio.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="password">File password</param>
        /// <returns>Document reader object</returns>
        IDocumentReader GetDocumentReader(string filePath, string password);

        /// <summary>
        /// Get document reader for this particular document.
        /// dimOne x dimTwo represents a viewport to which
        /// the document gets scaled to fit without modifying
        /// it's aspect ratio.
        /// </summary>
        /// <param name="bytes">File bytes</param>
        /// <returns>Document reader object</returns>
        IDocumentReader GetDocumentReader(byte[] bytes);

        /// <summary>
        /// Get document reader for this particular document.
        /// dimOne x dimTwo represents a viewport to which
        /// the document gets scaled to fit without modifying
        /// it's aspect ratio.
        /// </summary>
        /// <param name="bytes">File bytes</param>
        /// <param name="password">File password</param>
        /// <returns>Document reader object</returns>
        IDocumentReader GetDocumentReader(byte[] bytes, string password);

        /// <summary>
        /// Get a description of the last error
        /// that has occured.
        /// </summary>
        /// <returns>Error message</returns>
        string GetLastError();
    }
}