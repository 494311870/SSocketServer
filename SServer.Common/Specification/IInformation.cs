using SServer.Common.Protocol;

namespace SServer.Common.Specification
{
    /// <summary>
    /// 网络信息
    /// </summary>
    public interface IInformation
    {
        /// <summary>
        /// 信息的类别
        /// </summary>
        TypeCode TypeCode { get; }
        /// <summary>
        /// 用于标识消息
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 用于提示的信息
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// 携带的Json数据
        /// </summary>
        string Json { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        IClient Client { get; set; }

    }
}
