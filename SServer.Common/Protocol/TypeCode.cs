using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Protocol
{
    /// <summary>
    /// 区分消息的种类
    /// </summary>
    public enum TypeCode : byte
    {
        None,
        /// <summary>
        /// 请求
        /// </summary>
        Request,
        /// <summary>
        /// 响应
        /// </summary>
        Response,
    }
}
