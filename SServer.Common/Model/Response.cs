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
    [Serializable]
    public class Response : IResponse
    {
        public int Id { get; set; }
        //public RequestCode ResponseCode { get; set; }
        public StatusCode StatusCode { get; set; }
        //public object[] Data { get; set; }
        public string Json { get; set; }

        public Response(int id, StatusCode statusCode, string json) : this(statusCode, json) => Id = id;
        public Response(StatusCode statusCode, string json) : this(statusCode) => Json = json;
        public Response(StatusCode statusCode, object data) : this(statusCode) => Json = JsonMapper.ToJson(data);

        public Response(StatusCode statusCode) => StatusCode = statusCode;

        //public IResponse Create(RequestAndResponseCode responseCode, StatusCode statusCode, string json)
        //    => new Response(responseCode, statusCode, json);
    }
}
