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
        public RequestCode RequestCode { get; set; }
        public ActionCode ActionCode { get; set; }
        //public object[] Data { get; set; }
        public string Json { get; set; }

        public Request(int id, RequestCode requestCode, ActionCode actionCode, string json) : this(requestCode, actionCode, json) => Id = id;
        public Request(RequestCode requestCode, ActionCode actionCode, string json) : this(requestCode, actionCode) => Json = json;
        public Request(RequestCode requestCode, ActionCode actionCode, object data) : this(requestCode, actionCode) => Json = JsonMapper.ToJson(data);

        public Request(RequestCode requestCode, ActionCode actionCode)
        {
            RequestCode = requestCode;
            ActionCode = actionCode;
        }

        //public IRequest Create(RequestAndResponseCode requestCode, ActionCode actionCode, string json)
        //    => new Request(requestCode, actionCode, json);
    }
}
