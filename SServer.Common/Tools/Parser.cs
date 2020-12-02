using System;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Specification;
using SServer.Common.Protocol;
using SServer.Common.Model;
namespace SServer.Common.Tools
{
    /// <summary>
    /// 解析器，把字节流转换为Request/Response
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// 如何处理请求
        /// </summary>
        public Func<IRequest, Task> RequestHandle { get; }
        /// <summary>
        /// 如何处理响应
        /// </summary>
        public Func<IResponse, Task> ResponseHandle { get; }
        /// <summary>
        /// 创建一个解析器
        /// </summary>
        /// <param name="requestHandle"></param>
        /// <param name="responseHandle"></param>
        public Parser(Func<IRequest, Task> requestHandle, Func<IResponse, Task> responseHandle)
        {
            RequestHandle = requestHandle;
            ResponseHandle = responseHandle;
        }

        /// <summary>
        /// 解析 相当于我们自己进行反序列化啦
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="client"></param>
        public void Parse(byte[] bytes, IClient client)
        {
            var typeCode = (Protocol.TypeCode)bytes[0];
            var id = BitConverter.ToInt32(bytes, 1);
            (string message, string json) data;

            switch (typeCode)
            {
                case Protocol.TypeCode.None:
                    break;
                case Protocol.TypeCode.Request:
                    var requestCode = (RequestCode)BitConverter.ToInt16(bytes, 5);
                    var actionCode = (ActionCode)BitConverter.ToInt16(bytes, 7);
                    data = GetData(9);
                    IRequest request = new Request(requestCode, actionCode)
                    { Id = id, Message = data.message, Json = data.json, Client = client };
                    RequestHandle?.Invoke(request);
                    break;
                case Protocol.TypeCode.Response:
                    var statusCode = (StatusCode)BitConverter.ToInt16(bytes, 5);
                    data = GetData(7);
                    IResponse response = new Response(statusCode)
                    { Id = id, Message = data.message, Json = data.json, Client = client };
                    ResponseHandle?.Invoke(response);
                    break;
                default:
                    break;
            }
            // 解析数据
            (string message, string json) GetData(int start)
            {
                var messageLength = BitConverter.ToInt16(bytes, start);
                start += 4;
                var message = Encoding.UTF8.GetString(bytes, start, messageLength);
                var json = Encoding.UTF8.GetString(bytes, start + messageLength, bytes.Length - start - messageLength);
                return (message, json);
            }
        }
    }


}
