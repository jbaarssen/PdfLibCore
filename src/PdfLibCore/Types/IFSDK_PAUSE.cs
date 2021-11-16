using System;
using System.Runtime.InteropServices;

// ReSharper disable NotAccessedField.Local
namespace PdfLibCore.Types
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    [StructLayout(LayoutKind.Sequential)]
    public abstract class IFSDK_PAUSE
    {
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly Func<IntPtr, bool> _needToPauseCore;
        private readonly IntPtr _userData;
        private readonly Func<bool> _needToPause;

        protected IFSDK_PAUSE(Func<bool> needToPause)
        {
            _needToPause = needToPause ?? throw new ArgumentNullException(nameof(needToPause));
            _needToPauseCore = ignore => needToPause();
            _userData = IntPtr.Zero;
        }
    }
}