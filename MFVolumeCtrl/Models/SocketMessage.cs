using MFVolumeCtrl.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MFVolumeCtrl.Models
{
    public enum MessageType : byte
    {
        ConfigMsg = 0,
        ServiceMsg = 1,
        ScriptMsg = 2,
        ImageMsg = 3,
        ResponseMsg = 4
    }

    [Serializable]
    public class SocketMessage<T> : IDisposable
    {
        private const int HeaderLengthIndex = 0;

        private const int BodyLengthIndex = 4;

        private const int HeaderIndex = 8;

        private int BodyIndex => HeaderIndex + HeaderLength;

        public int HeaderLength { get; set; }

        public int BodyLength { get; set; }

        public Dictionary<string, object> Headers { get; set; }

        public T Body { get; set; }

        public SocketMessage()
        {

        }

        public SocketMessage(byte[] binary)
        {
            ParseBinaryAsync(binary).GetAwaiter().GetResult();
        }

        public static async Task<SocketMessage<T>> CreateSocketMessageAsync(byte[] binary)
        {
            var msg = new SocketMessage<T>();
            await msg.ParseBinaryAsync(binary);
            return msg;
        }

        public void Dispose()
        {
        }

        public async Task ParseBinaryAsync(byte[] binary)
        {
            if (binary is null) throw new ArgumentNullException(nameof(binary));
            HeaderLength = BitConverter.ToInt32(binary, HeaderLengthIndex);
            BodyLength = BitConverter.ToInt32(binary, BodyLengthIndex);
            Headers = await BinaryUtil.DeserializeObject<Dictionary<string, object>>(binary, HeaderIndex, HeaderLength);
            Body = BodyLength == 0 ? default(T) : await BinaryUtil.DeserializeObject<T>(binary, BodyIndex, BodyLength);
        }
    }
}
