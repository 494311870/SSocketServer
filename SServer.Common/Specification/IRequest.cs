using SServer.Common.Protocol;

namespace SServer.Common.Specification
{
    /// <summary>
    /// 请求
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// 用于标识发起的请求
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 指定请求的目标
        /// </summary>
        RequestAndResponseCode RequestCode { get; set; }
        /// <summary>
        /// 行为码
        /// </summary>
        ActionCode ActionCode { get; set; }
        /// <summary>
        /// 携带的数据
        /// </summary>
        //object[] Data { get; set; }
        /// <summary>
        /// 携带的Json数据
        /// </summary>
        string Json { get; set; }

        //IRequest Create(RequestAndResponseCode requestCode, ActionCode actionCode, string json);
    }
}
