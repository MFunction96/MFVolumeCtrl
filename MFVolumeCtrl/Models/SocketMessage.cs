using MFVolumeCtrl.Controllers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        private const int BodyLengthIndex = sizeof(int);

        private const int HeaderIndex = sizeof(int) << 1;

        private T _body;

        private int _headerlength;

        private int _bodylength;

        private int BodyIndex => HeaderIndex + HeaderLength;

        public int HeaderLength
        {
            get
            {
                _headerlength = Marshal.SizeOf(Headers);
                return _headerlength;
            }
            set => _headerlength = value;
        }

        public int BodyLength
        {
            get
            {
                _bodylength = Body == null ? 0 : Marshal.SizeOf(Body);
                return _bodylength;
            }
            set => _bodylength = value;
        }

        public Dictionary<string, object> Headers { get; set; }

        public T Body
        {
            get => _body;
            set
            {
                _body = value;
                _bodylength = value == null ? 0 : Marshal.SizeOf(value);
            }
        }

        public SocketMessage()
        {
            Headers = new Dictionary<string, object>();
        }

        public SocketMessage(byte[] binary)
        {
            Headers = new Dictionary<string, object>();
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
