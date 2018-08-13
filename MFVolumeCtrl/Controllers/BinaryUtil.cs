using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MFVolumeCtrl.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public static class BinaryUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Task<byte[]> SerializeObject(object obj)
        {
            return Task.Run(() =>
            {
                var buffer = new byte[Marshal.SizeOf(obj)];
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                Marshal.StructureToPtr(obj, ptr, true);
                return buffer;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static Task<TType> DeserializeObject<TType>(byte[] binary)
        {
            return Task.Run(() =>
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(binary, 0);
                return Marshal.PtrToStructure<TType>(ptr);
            });
        }
    }
}
