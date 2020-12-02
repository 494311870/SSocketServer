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
    /// 响应中间件 用于在请求前后进行处理 
    /// </summary>
    class ResponseMiddleware
    {
        public Server Server { get; set; }

        public ResponseMiddleware(Server server)
        {
            Server = server;
        }

        /// <summary>
        /// Before方法用在响应发送给服务器之前
        /// </summary>
        /// <param name="response"></param>
        public void BeforeWrapper(IResponse response)
        {
            Before(response);
        }
        /// <summary>
        /// After方法用在处理器生成响应之后
        /// </summary>
        /// <param name="response"></param>
        public void AfterWrapper(IResponse response)
        {
            After(response);
        }

        protected virtual void Before(IResponse response)
        {

        }
        protected virtual void After(IResponse response)
        {
            Server.Logger.Log("服务器生成了一条响应");
        }
    }
}
