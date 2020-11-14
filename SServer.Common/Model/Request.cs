using System.Collections.Generic;

using SServer.Common.Protocol;
using SServer.Common.Specification;
using System;
using System.Runtime.Serialization;
using LitJson;

namespace SServer.Common.Model
{
    [Serializable]
    public class Request : IRequest
    {
        public int Id { get; set; }
        public RequestAndResponseCode RequestCode { get; set; }
        public ActionCode ActionCode { get; set; }
        //public object[] Data { get; set; }
        public string Json { get; set; }

        public Request(int id, RequestAndResponseCode requestCode, ActionCode actionCode, string json)
        {
            Id = id;
            RequestCode = requestCode;
            ActionCode = actionCode;
            //Data = data;
            Json = json;
        }

        /// <summary>
        /// 提供一个无需id的构造方法，id会由处理器进行处理，无需控制器处理
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="json"></param>
        public Request(RequestAndResponseCode requestCode, ActionCode actionCode, string json)
        {
            RequestCode = requestCode;
            ActionCode = actionCode;
            Json = json;
        }



        //public IRequest Create(RequestAndResponseCode requestCode, ActionCode actionCode, string json)
        //    => new Request(requestCode, actionCode, json);
    }
}
