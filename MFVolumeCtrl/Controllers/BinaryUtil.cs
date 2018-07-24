using System.Runtime.InteropServices;

namespace MFVolumeCtrl.Controllers
{
    public static class BinaryUtil
    {
        public static byte[] SerializeObject(object obj)
        {
            var buffer = new byte[Marshal.SizeOf(obj)];
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            Marshal.StructureToPtr(obj, ptr, true);
            return buffer;
        }

        public static TType DeserializeObject<TType>(byte[] binary)
        {
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(binary, 0);
            return Marshal.PtrToStructure<TType>(ptr);
        }
    }
}
