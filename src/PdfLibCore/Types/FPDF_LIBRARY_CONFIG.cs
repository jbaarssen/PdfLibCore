using System;
using System.Runtime.InteropServices;

namespace PdfLibCore.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct FPDF_LIBRARY_CONFIG
    {
        private readonly int _version;
        private readonly IntPtr _userFontPaths;
        private readonly IntPtr _v8Isolate;
        private readonly uint _v8EmbedderSlot;

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once ConvertToAutoProperty
        public int Version => _version;
    }
}