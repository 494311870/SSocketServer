using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Protocol
{
    /// <summary>
    /// 请求的行为码
    /// 遵循REST规范
    /// </summary>
    public enum ActionCode : short
    {
        /// <summary>
        /// 默认值
        /// </summary>
        None,
        /// <summary>
        /// 查询 从服务器获取资源   
        /// </summary>
        Get,
        /// <summary>
        /// 添加 在服务器新增资源   
        /// </summary>
        Post,
        /// <summary>
        /// 删除 从服务器删除资源
        /// </summary>
        Delete,
        /// <summary>
        /// 修改 更新服务器资源
        /// </summary>
        Put,
        /// <summary>
        /// 局部修改 更新部分资源
        /// </summary>
        Patch,
    }
}
