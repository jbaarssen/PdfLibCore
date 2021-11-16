using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PdfLibCore.Types
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool FileReadBlockHandler(IntPtr ignore, int position, IntPtr buffer, int size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool FileWriteBlockHandler(IntPtr ignore, IntPtr data, int size);

    [StructLayout(LayoutKind.Sequential)]
    public class FPDF_FILEREAD
    {
        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once NotAccessedField.Local
        [MarshalAs(UnmanagedType.FunctionPtr)] readonly FileReadBlockHandler _readBlock;

        private FPDF_FILEREAD(FileReadBlockHandler readBlock)
        {
            _readBlock = readBlock;
        }

        public static FPDF_FILEREAD FromStream(Stream stream)
        {
            var start = stream.Position;
            byte[] data = null;
            var fileread = new FPDF_FILEREAD((ignore, position, buffer, size) =>
            {
                stream.Position = start + position;
                if (data == null || data.Length < size)
                {
                    data = new byte[size];
                }
                if (stream.Read(data, 0, size) != size)
                {
                    return false;
                }
                Marshal.Copy(data, 0, buffer, size);
                return true;
            });
            return fileread;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FPDF_FILEWRITE
    {
        private int Version = 1;
        
        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once NotAccessedField.Local
        // ReSharper disable once MemberCanBePrivate.Global
        [MarshalAs(UnmanagedType.FunctionPtr)] public FileWriteBlockHandler WriteBlock;

        public FPDF_FILEWRITE(FileWriteBlockHandler writeBlock)
        {
            WriteBlock = writeBlock;
        }
    }
}