using System;
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
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Task<TType> DeserializeObject<TType>(byte[] binary, int startIndex = 0, int length = 0)
        {
            return Task.Run(() =>
            {
                byte[] tmp;
                if (length == 0)
                {
                    tmp = binary;
                }
                else
                {
                    tmp = new byte[length];
                    Array.Copy(binary, startIndex, tmp, 0, length);
                }
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(tmp, 0);
                return Marshal.PtrToStructure<TType>(ptr);
            });
        }
    }
}
