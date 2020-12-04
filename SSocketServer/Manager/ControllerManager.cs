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
using SServer.Common.Model;
using SSocketServer.Servers;
using SServer.Common.Tools;
using SSocketServer.Util.Log;
using SSocketServer.Util.Container;

namespace SSocketServer.Manager
{
    /// <summary>
    /// 相当于路由映射
    /// </summary>
    class ControllerManager
    {
        //private Dictionary<RequestCode, BaseController> ControllerDict { get; } = new Dictionary<RequestCode, BaseController>();
        // 请求码 + 行为码 映射到方法上
        // 这里有两种做法，一种是找不到的时候采取通过反射搜索，一种是初始化的时候全部加载
        // 为了避免客户端可能恶意发起不存在的请求来消耗服务器资源，我们使用第二种来实现
        // 如有需要，可以换成第一种
        private Dictionary<(RequestCode requestCode, ActionCode actionCode), (BaseController controller, MethodInfo method)> HandleDict { get; }
            = new Dictionary<(RequestCode, ActionCode), (BaseController, MethodInfo)>();


        public ControllerManager() => Init();

        private void Init()
        {
            //Add(new DefaultController());
            //Add(SContainer.Resolve<LoginController>());
            //Add(SContainer.Resolve<RegisterController>());
            //Add(SContainer.Resolve<RoomController>());
            Console.WriteLine("----------");
            var controllers = SContainer.Resolves<BaseController>();
            foreach (var controller in controllers)
            {
                FindMethod(controller.type, controller.instance);
            }
            // 搜索方法
            void FindMethod(Type type, BaseController controller)
            {
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RequestMappingAttribute), false);
                    if (attributes.Any())
                    {
                        var key = (controller.RequestCode, (attributes.First() as RequestMappingAttribute).ActionCode);
                        HandleDict.Add(key, (controller, method));
                    }
                }
            }
        }
        /// <summary>
        /// 添加一个控制器
        /// </summary>
        /// <param name="controller">控制器</param>
        //public void Add(BaseController controller) => ControllerDict.Add(controller.RequestCode, controller);

        /// <summary>
        /// 请求处理器：用于分发请求到对应的控制器上
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResponse RequestHandler(IRequest request)
        {
            IResponse response;
            if (HandleDict.TryGetValue((request.RequestCode, request.ActionCode), out var handel))
            {
                try
                {
                    response = handel.method.Invoke(handel.controller, new object[] { request }) as IResponse;
                    response.Id = request.Id;
                    return response;
                }
                catch (Exception e)
                {
                    // TODO 加一个异常处理中间件来统一处理异常 这样写属实憨憨
                    Server.Instance.Logger.Log($"在处理[{request.RequestCode}][{request.ActionCode}]时发生了错误:/n{e}", LogLevel.Error);
#if DEBUG
                    response = new Response(StatusCode.InternalServerError) { Id = request.Id, Message = $"在处理[{request.RequestCode}][{request.ActionCode}]时发生了错误" };
#endif
                }
            }


            if (Enum.IsDefined(typeof(RequestCode), request.RequestCode))
            {
                Server.Instance.Logger.Log($"在Controller[{handel.controller.ToString()}]中没有对应的处理方法：[{request.ActionCode}]", LogLevel.Warn);
#if DEBUG
                response = new Response(StatusCode.MethodNotAllowed) { Message = $"[{request.RequestCode}][{request.ActionCode}]是不被允许的请求" };
#endif
            }
            else
            {
                Server.Instance.Logger.Log($"[{request.RequestCode}]未找到对应的处理器！", LogLevel.Warn);
#if DEBUG
                response = new Response(StatusCode.BadRequest) { Message = $"[{request.RequestCode}][{request.ActionCode}]是错误的请求" };
#endif
            }
#if DEBUG
            return response;
#else
            return new Response(StatusCode.InternalServerError) { Id = request.Id, Message = $"服务器遇到错误，无法完成请求" };
#endif
        }

        public void ResponseHandler(IResponse response)
        {
            // TODO:对响应进行处理
        }
    }
}
