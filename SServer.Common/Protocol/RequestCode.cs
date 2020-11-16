using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Protocol
{
    /// <summary>
    /// 请求码 表面请求访问哪一个控制器
    /// </summary>
    public enum RequestCode : short
    {
        None,
        Login,
        Register,
    }
}
