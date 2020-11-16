using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Protocol;
using SServer.Common.Specification;
using System.Reflection;
using SSocketServer.Controller;
using SSocketServer.Attributes;

namespace SSocketServer.Manager
{
    /// <summary>
    /// 相当于路由映射
    /// </summary>
    class ControllerManager
    {

        private Dictionary<RequestCode, BaseController> ControllerDict { get; } = new Dictionary<RequestCode, BaseController>();
        public Type RequestMappingAttributeType { get; } = typeof(RequestMappingAttribute);

        public ControllerManager() => Init();

        private void Init()
        {
            // TODO
            Add(new DefaultController());
            Add(new LoginController());
            foreach (var item in ControllerDict.Keys)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// 添加一个控制器
        /// </summary>
        /// <param name="controller">控制器</param>
        public void Add(BaseController controller) => ControllerDict.Add(controller.ProtocolCode, controller);

        /// <summary>
        /// 请求处理器：用于分发请求到对应的控制器上
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResponse RequestHandler(IRequest request)
        {
            Console.WriteLine(request.RequestCode);
            if (ControllerDict.TryGetValue(request.RequestCode, out BaseController controller))
            {
                var controllerType = controller.GetType();
                var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                // 找到行为映射的方法
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(RequestMappingAttributeType, false);
                    if (attributes.Any())
                    {
                        if ((attributes.First() as RequestMappingAttribute).ActionCode.Equals(request.ActionCode))
                        {
                            var response = method.Invoke(controller, new object[] { request.Json }) as IResponse;
                            response.Id = request.Id;
                            return response;
                        }
                    }
                }
                Console.WriteLine($"[警告]在Controller[{controller.ToString()}]中没有对应的处理方法：[{request.ActionCode}]");
            }
            else
            {
                Console.WriteLine($"[警告]请求[{request.RequestCode}]未找到请求对应的处理器！");
            }

            return null;

        }

        public void ResponseHandler(IResponse response)
        {
            Console.WriteLine("处理器进行了处理！");
            // TODO:对响应进行处理,然后再由服务器负责发送

        }
    }
}
