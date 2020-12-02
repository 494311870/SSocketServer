using SServer.Common.Protocol;

namespace SServer.Common.Specification
{
    /// <summary>
    /// 请求
    /// </summary>
    public interface IRequest : IInformation
    {
        /// <summary>
        /// 指定请求的目标
        /// </summary>
        RequestCode RequestCode { get; set; }
        /// <summary>
        /// 行为码
        /// </summary>
        ActionCode ActionCode { get; set; }
    }
}
