using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Specification;
using SServer.Common.Protocol;
using LitJson;

namespace SServer.Common.Model
{
    /// <summary>
    /// 网络响应
    /// </summary>
    [Serializable]
    public class Response : Information, IResponse
    {
        /// <summary>
        /// 信息的类别
        /// </summary>
        public override Protocol.TypeCode TypeCode => Protocol.TypeCode.Response;
        /// <summary>
        /// 响应的状态码
        /// </summary>
        public StatusCode StatusCode { get; set; }
        /// <summary>
        /// 创建一个Response对象
        /// </summary>
        public Response(StatusCode statusCode, object data) : this(statusCode) 
            => Json = JsonMapper.ToJson(data);
        /// <summary>
        /// 创建一个Response对象
        /// </summary>
        public Response(StatusCode statusCode) => StatusCode = statusCode;
    }
}
