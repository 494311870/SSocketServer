using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Protocol;
using SServer.Common.Specification;

namespace SSocketServer.Controller
{
    abstract class BaseController
    {
        public RequestCode ProtocolCode { get; set; }
        /// <summary>
        /// 提供一个默认的虚方法
        /// </summary>
        public virtual IResponse DefaultHandle(string json) => null;

        

    }
}
