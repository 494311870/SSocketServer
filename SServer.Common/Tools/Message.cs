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
    /// 用于发送 请求和响应 
    /// 0-3 标记内容长度 int32 4字节
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
        /// <param name="length">写入缓冲区的长度</param>
        /// <returns></returns>
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
                    Array.Copy(Buffer, DataLength + 4, Buffer, 0, Current); // 手动移动缓冲区的数据
                    // 调用解析方法处理读到消息
                    Console.WriteLine("读到了一条完整的数据");
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

        // 将要发送的数据打包成Byte数组
        public static byte[] GetBytes(string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var lengthBytes = BitConverter.GetBytes(dataBytes.Length);
            return lengthBytes.Concat(dataBytes).ToArray();
        }

        /// <summary>
        /// 这里的话我们自己去处理request,也可以用json进行序列化反序列化，但是枚举类型需要自定义序列化，索性就自己写了。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static byte[] GetBytes(IRequest request)
        {
            var byteList = new List<byte>();

            var typeCode = (byte)Protocol.TypeCode.Request;
            var id = BitConverter.GetBytes(request.Id);
            var requestCode = BitConverter.GetBytes((short)request.RequestCode);
            var actionCode = BitConverter.GetBytes((short)request.ActionCode);
            //var dataBytes = Encoding.UTF8.GetBytes(JsonMapper.ToJson(request.Data));
            var dataBytes = Encoding.UTF8.GetBytes(request.Json);

            var lengthBytes = BitConverter.GetBytes(1 + id.Length + requestCode.Length + actionCode.Length + dataBytes.Length);
            //Console.WriteLine("正在打包request");
            byteList.AddRange(lengthBytes);

            byteList.Add(typeCode); // 1
            byteList.AddRange(id); // 4
            byteList.AddRange(requestCode); // 2
            byteList.AddRange(actionCode); // 2
            byteList.AddRange(dataBytes);

            return byteList.ToArray();

            
        }


        public static byte[] GetBytes(IResponse response)
        {
            var byteList = new List<byte>();

            var typeCode = (byte)Protocol.TypeCode.Response;
            var id = BitConverter.GetBytes(response.Id);
            //var responseCode = BitConverter.GetBytes((short)response.ResponseCode);
            var statusCode = BitConverter.GetBytes((short)response.StatusCode);
            //var dataBytes = Encoding.UTF8.GetBytes(JsonMapper.ToJson(response.Data));
            var dataBytes = Encoding.UTF8.GetBytes(response.Json);

            var lengthBytes = BitConverter.GetBytes(1 + id.Length + statusCode.Length + dataBytes.Length);
            //Console.WriteLine("正在打包response");
            byteList.AddRange(lengthBytes);

            byteList.Add(typeCode); // 1
            byteList.AddRange(id); // 4
            //byteList.AddRange(responseCode); // 2
            byteList.AddRange(statusCode); // 2
            byteList.AddRange(dataBytes);

            return byteList.ToArray();
        }


    }
}
