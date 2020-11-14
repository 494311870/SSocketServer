using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Specification;
using SServer.Common.Protocol;

namespace SServer.Common.Model
{
    [Serializable]
    public class Response : IResponse
    {
        public int Id { get; set; }
        public RequestAndResponseCode ResponseCode { get; set; }
        public StatusCode StatusCode { get; set; }
        //public object[] Data { get; set; }
        public string Json { get; set; }

        public Response(int id, RequestAndResponseCode responseCode, StatusCode statusCode, string json)
        {
            Id = id;
            ResponseCode = responseCode;
            StatusCode = statusCode;
            //Data = data;
            Json = json;
        }
        public Response(RequestAndResponseCode responseCode, StatusCode statusCode, string json)
        {
            ResponseCode = responseCode;
            StatusCode = statusCode;
            Json = json;
        }

        //public IResponse Create(RequestAndResponseCode responseCode, StatusCode statusCode, string json)
        //    => new Response(responseCode, statusCode, json);
    }
}
