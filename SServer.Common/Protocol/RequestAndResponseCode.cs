using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Protocol
{
    /// <summary>
    /// 请求和响应码
    /// 几乎所有的请求都希望得到一个响应，我们为什么不共用一套协议来区分是哪个请求/响应呢？
    /// </summary>
    public enum RequestAndResponseCode : short
    {
        None,
        Login,
    }
}
