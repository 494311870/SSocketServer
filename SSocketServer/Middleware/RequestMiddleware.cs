using Microsoft.Extensions.Logging;
using SServer.Common.Model;
using SServer.Common.Specification;
using SSocketServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSocketServer.Middleware
{
    /// <summary>
    /// 请求中间件 用于在请求前后进行处理
    /// </summary>
    class RequestMiddleware
    {
        public Server Server { get; set; }

        public RequestMiddleware(Server server)
        {
            Server = server;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void BeforeWrapper(IRequest request)
        {
            Before(request);
        }

        public void AfterWrapper(Request request)
        {
            After(request);
        }

        protected virtual void Before(IRequest request) { Server.Logger.Log("服务器处理了一条请求"); }
        protected virtual void After(IRequest request) { }
    }
}
