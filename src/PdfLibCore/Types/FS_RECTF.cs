using System.Runtime.InteropServices;

namespace PdfLibCore.Types
{
    /// <summary>
    /// Rectangle area(float) in device or page coordinate system.
    /// </summary>
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct FS_RECTF
    {
        /// <summary>
        /// The x-coordinate of the left-top corner.
        /// </summary>
        public float Left { get; }

        /// <summary>
        /// The y-coordinate of the left-top corner.
        /// </summary>
        public float Top { get; }

        /// <summary>
        /// The x-coordinate of the right-bottom corner.
        /// </summary>
        public float Right { get; }

        /// <summary>
        /// The y-coordinate of the right-bottom corner.
        /// </summary>
        public float Bottom { get; }

        public FS_RECTF(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}