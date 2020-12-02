using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SServer.Common.Specification;
using System.Threading.Tasks;
using LitJson;
namespace SServer.Common.Tools
{
    /// <summary>
    /// 用于读取、打包 网络消息
    /// </summary>
    public class Message
    {
        private static readonly int BufferSize = 1024;
        /// <summary>
        /// 缓冲器
        /// </summary>
        public byte[] Buffer { get; } = new byte[BufferSize];
        /// <summary>
        /// 指针：指向缓冲器当前写入的位置
        /// </summary>
        public int Current { get; set; }
        /// <summary>
        /// 计算属性:缓冲器剩余空间
        /// </summary>
        public int BufferRemain => BufferSize - Current;
        // 数据的真实长度
        private int DataLength { get; set; }
        // 用于解析较长的消息
        private List<byte> ByteList { get; } = new List<byte>();
        /// <summary>
        /// 得到若干个完整的包，可以处理分包和粘包的情况
        /// </summary>
        /// <param name="length"></param>
        /// <param name="parse">解析方法</param>
        public void Read(int length, Func<byte[], Task> parse)
        {
            Current += length;
            //解析多次，用于处理tcp粘包情况
            while (true)
            {
                // 消息长度不足一个包头，不处理
                if (Current <= 4) break;
                // 已有缓存的数据，继续解析
                if (ByteList.Count <= 0) DataLength = BitConverter.ToInt32(Buffer, 0);
                //模拟服务器处理缓慢
                //Thread.Sleep(1000);
                //读取到了一条完整的消息，用于处理tcp分包情况
                if (Current >= DataLength + 4)
                {
                    ByteList.AddRange(Buffer.Skip(4).Take(DataLength));

                    Current -= DataLength + 4;
                    // 手动移动缓冲区的数据
                    Array.Copy(Buffer, DataLength + 4, Buffer, 0, Current);
                    // 调用解析方法处理读到消息
                    parse(ByteList.ToArray());

                    ByteList.Clear();
                }
                else
                {
                    // 消息不够完整，可能被tcp分包了，也可能是我们缓冲区大小不够
                    // 虽然没有接收到全部的数据，但是由于缓冲区满了，无法接受更多的数据，此时应该处理一下;
                    if (BufferRemain <= 0)
                    {
                        // 从缓冲区中读取全部的数据 短的消息总是会被先处理，所以这里整个缓冲区的数据都是一个包的
                        ByteList.AddRange(Buffer.Skip(4));
                        Current = 4;
                        DataLength -= BufferSize - 4;
                    }
                    break;
                }
            }
        }

        private static byte[] GetBytes(IInformation information, params byte[][] other)
        {
            var byteList = new List<byte>();
            var typeCode = (byte)information.TypeCode;
            var id = BitConverter.GetBytes(information.Id);
            var message = Encoding.UTF8.GetBytes(information.Message ?? "");
            var messageLength = BitConverter.GetBytes(message.Length);
            var data = Encoding.UTF8.GetBytes(information.Json ?? "");
            var length = 1 + id.Length + message.Length + messageLength.Length + data.Length;
            // 处理传入的参数
            length += other.Sum(x => x.Length);

            byteList.AddRange(BitConverter.GetBytes(length));

            byteList.Add(typeCode); // 1
            byteList.AddRange(id); // 4

            foreach (var item in other) { byteList.AddRange(item); }

            byteList.AddRange(messageLength);
            byteList.AddRange(message);
            byteList.AddRange(data);

            return byteList.ToArray();
        }


        /// <summary>
        /// 这里的话我们自己去处理request,也可以用json进行序列化反序列化，但是枚举类型需要自定义序列化，索性就自己写了。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static byte[] GetBytes(IRequest request)
        {
            var requestCode = BitConverter.GetBytes((short)request.RequestCode);
            var actionCode = BitConverter.GetBytes((short)request.ActionCode);

            return GetBytes(request, requestCode, actionCode);
        }

        /// <summary>
        /// 将response转换为字节数组
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static byte[] GetBytes(IResponse response)
        {
            var statusCode = BitConverter.GetBytes((short)response.StatusCode);

            return GetBytes(response, statusCode);
        }
    }
}
