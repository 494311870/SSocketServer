using LitJson;
using SServer.Common.Protocol;
using SServer.Common.Specification;
using System;

namespace SServer.Common.Model
{
    /// <summary>
    /// 网络请求
    /// </summary>
    [Serializable]
    public class Request :Information, IRequest
    {
        /// <summary>
        /// 信息的类别
        /// </summary>
        public override Protocol.TypeCode TypeCode => Protocol.TypeCode.Request;
        /// <summary>
        /// 请求码
        /// </summary>
        public RequestCode RequestCode { get; set; }
        /// <summary>
        /// 请求的行为码
        /// </summary>
        public ActionCode ActionCode { get; set; }
        /// <summary>
        /// 创建一个Request对象
        /// </summary>
        public Request(RequestCode requestCode, ActionCode actionCode, object data) 
            : this(requestCode, actionCode) 
            => Json = JsonMapper.ToJson(data);
        /// <summary>
        /// 创建一个Request对象
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        public Request(RequestCode requestCode, ActionCode actionCode)
        {
            RequestCode = requestCode;
            ActionCode = actionCode;
        }
    }
}
