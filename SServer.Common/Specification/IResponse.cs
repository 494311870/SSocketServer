using SServer.Common.Protocol;

namespace SServer.Common.Specification
{
    /// <summary>
    /// 响应
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// 用于标识响应
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 指定响应的接受者
        /// </summary>
        RequestAndResponseCode ResponseCode { get; set; }
        /// <summary>
        /// 响应的状态码
        /// </summary>
        StatusCode StatusCode { get; set; }
        /// <summary>
        /// 携带的数据
        /// </summary>
        //object[] Data { get; set; }
        /// <summary>
        /// 携带的Json数据
        /// </summary>
        string Json { get; set; }

        //IResponse Create(RequestAndResponseCode responseCode, StatusCode statusCode, string json);
    }
}
