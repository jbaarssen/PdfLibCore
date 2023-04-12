using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
namespace PdfLibCore
{
    public struct PinnedGcHandle : IDisposable
    {
        private GCHandle _handle;

        public IntPtr Pointer => _handle.AddrOfPinnedObject();
        public bool IsAllocated => _handle.IsAllocated;
        public object Target => _handle.Target;        
        
        private PinnedGcHandle(GCHandle handle)
        {
            _handle = handle;
        }

        public static PinnedGcHandle Pin(object obj) => new(GCHandle.Alloc(obj, GCHandleType.Pinned));

        public void Free() => _handle.Free();

        void IDisposable.Dispose()
        {
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }
        }

        public override string ToString() => _handle.ToString();
    }
}