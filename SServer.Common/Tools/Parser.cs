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
        public Func<IRequest, IClient, Task> RequestHandle { get; }
        public Func<IResponse, IClient, Task> ResponseHandle { get; }

        public Parser(Func<IRequest, IClient, Task> requestHandle, Func<IResponse, IClient, Task> responseHandle)
        {
            RequestHandle = requestHandle;
            ResponseHandle = responseHandle;
        }

        // 相当于我们自己进行反序列化啦
        public void Parse(byte[] bytes, IClient client)
        {
            var id = BitConverter.ToInt32(bytes, 1);
            string json;

            switch ((Protocol.TypeCode)bytes[0])
            {
                case Protocol.TypeCode.None:
                    break;
                case Protocol.TypeCode.Request:
                    var requestCode = (RequestCode)BitConverter.ToInt16(bytes, 5);
                    var actionCode = (ActionCode)BitConverter.ToInt16(bytes, 7);
                    json = Encoding.UTF8.GetString(bytes, 9, bytes.Length - 9);

                    RequestHandle(new Request(id, requestCode, actionCode, json), client);
                    break;
                case Protocol.TypeCode.Response:
                    var statusCode = (StatusCode)BitConverter.ToInt16(bytes, 5);
                    json = Encoding.UTF8.GetString(bytes, 7, bytes.Length - 7);

                    ResponseHandle(new Response(id, statusCode, json), client);
                    break;
                default:
                    break;
            }
        }
    }


}
