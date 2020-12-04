using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Protocol
{
    /// <summary>
    /// 请求码 表明请求访问哪一个控制器
    /// </summary>
    public enum RequestCode : short
    {
        /// <summary>
        /// 默认值
        /// </summary>
        None,
        /// <summary>
        /// 登陆
        /// </summary>
        Login,
        /// <summary>
        /// 注册
        /// </summary>
        Register,
        /// <summary>
        /// 房间
        /// </summary>
        Room,
        /// <summary>
        /// 等待房间
        /// </summary>
        WaitingRoom,
    }
}
