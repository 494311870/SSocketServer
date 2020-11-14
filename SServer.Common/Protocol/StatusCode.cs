using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Protocol
{
    /// <summary>
    /// 响应的状态码
    /// </summary>
    public enum StatusCode : short
    {
        None,
        OK = 200,
        Created = 201,
        NotFound = 404,
        InternalServerError = 500,
    }
}
