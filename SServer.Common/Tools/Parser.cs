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
        //<RequestAndResponseCode, ActionCode, string, IRequest>
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
            var protocolCode = (RequestAndResponseCode)BitConverter.ToInt16(bytes, 5);
            string json = Encoding.UTF8.GetString(bytes, 9, bytes.Length - 9);
            Console.WriteLine(json);

            switch ((Protocol.TypeCode)bytes[0])
            {
                case Protocol.TypeCode.None:
                    break;
                case Protocol.TypeCode.Request:
                    var actionCode = (ActionCode)BitConverter.ToInt16(bytes, 7);

                    RequestHandle(new Request(id, protocolCode, actionCode, json), client);
                    break;
                case Protocol.TypeCode.Response:
                    var statusCode = (StatusCode)BitConverter.ToInt16(bytes, 7);
                    
                    ResponseHandle(new Response(id, protocolCode, statusCode, json), client);
                    break;
                default:
                    break;
            }
        }
    }


}
