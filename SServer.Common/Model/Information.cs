using LitJson;
using SServer.Common.Protocol;
using SServer.Common.Specification;

namespace SServer.Common.Model
{
    /// <summary>
    /// 用于传输网络信息的基类
    /// </summary>
    public abstract class Information : IInformation
    {
        /// <summary>
        /// 信息的类别
        /// </summary>
        public abstract TypeCode TypeCode { get; }
        /// <summary>
        /// 用于标识响应
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用于提示的信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 携带的Json数据
        /// </summary>
        public string Json { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        public IClient Client { get; set; }

        private JsonData data;
        /// <summary>
        /// JsonData
        /// </summary>
        public JsonData Data => data ?? (data = JsonMapper.ToObject(Json));
        /// <summary>
        /// 将Json转换为自定义类型
        /// </summary>
        /// <typeparam name="T">期望类型</typeparam>
        /// <returns></returns>
        public T GetData<T>() => JsonMapper.ToObject<T>(Json);

    }
}
