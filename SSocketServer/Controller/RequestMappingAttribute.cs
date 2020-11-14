using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Protocol;
namespace SSocketServer.Controller
{
    /// <summary>
    /// 用于映射行为到自定义的方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    class RequestMappingAttribute : Attribute
    {
        public ActionCode ActionCode { get; set; }
        public RequestMappingAttribute(ActionCode actionCode)
        {
            ActionCode = actionCode;
        }
    }
}
