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
        /// <summary>
        /// 默认值
        /// </summary>
        None,
        /// <summary>
        /// 成功 服务器已成功处理了请求。
        /// </summary>
        OK = 200,
        /// <summary>
        /// 已创建 请求成功并且服务器创建了新的资源。
        /// </summary>
        Created = 201,
        /// <summary>
        /// 无内容 服务器成功处理了请求，但没有返回任何内容。
        /// </summary>
        NoContent = 204,
        /// <summary>
        /// 重置内容 服务器成功处理了请求，但没有返回任何内容。 此响应要求请求者重置内容。
        /// </summary>
        ResetContent = 205,
        /// <summary>
        /// 错误请求 服务器不理解请求的语法。
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// 未找到 服务器找不到请求的资源
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// 方法禁用 禁用请求中指定的方法。
        /// </summary>
        MethodNotAllowed = 405,
        /// <summary>
        /// 请求超时 
        /// </summary>
        RequestTimeout = 408,
        /// <summary>
        /// 服务器内部错误 服务器遇到错误，无法完成请求。
        /// </summary>
        InternalServerError = 500,
        /// <summary>
        /// 服务不可用 服务器目前无法使用
        /// </summary>
        ServiceUnavailable = 503,
    }
}
