using SServer.Common.Protocol;

namespace SServer.Common.Specification
{
    /// <summary>
    /// 响应
    /// </summary>
    public interface IResponse : IInformation
    {
        /// <summary>
        /// 响应的状态码
        /// </summary>
        StatusCode StatusCode { get; set; }
    }
}
