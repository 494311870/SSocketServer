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

namespace SSocketServer.Manager
{
    /// <summary>
    /// 相当于路由映射
    /// </summary>
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> ControllerDict { get; } = new Dictionary<RequestCode, BaseController>();


        public ControllerManager() => Init();

        private void Init()
        {
            // TODO
            Add(new DefaultController());
            Add(new LoginController());
            Add(new RegisterController());
            Add(new RoomController());

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
            if (ControllerDict.TryGetValue(request.RequestCode, out BaseController controller))
            {
                var controllerType = controller.GetType();
                var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                // 找到行为映射的方法
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RequestMappingAttribute), false);
                    if (attributes.Any())
                    {
                        if ((attributes.First() as RequestMappingAttribute).ActionCode.Equals(request.ActionCode))
                        {
                            try
                            {
                                var response = method.Invoke(controller, new object[] { request }) as IResponse;
                                response.Id = request.Id;
                                return response;
                            }
                            catch (Exception e)
                            {
                                Server.Instance.Logger.Log($"在处理[{request.RequestCode}][{request.ActionCode}]时发生了错误:/n{e}", LogLevel.Error);
#if DEBUG
                                return new Response(StatusCode.InternalServerError) { Id = request.Id, Message = $"在处理[{request.RequestCode}][{request.ActionCode}]时发生了错误" };
#else
                                return new Response(StatusCode.InternalServerError) { Id = request.Id, Message = $"服务器遇到错误，无法完成请求" };
#endif
                            }
                            finally
                            {

                            }
                        }
                    }
                }
                Server.Instance.Logger.Log($"在Controller[{controller.ToString()}]中没有对应的处理方法：[{request.ActionCode}]", LogLevel.Warn);
                return new Response(StatusCode.MethodNotAllowed, new { Message = $"[{request.RequestCode}][{request.ActionCode}]是不被允许的请求" });


            }
            else
            {
                Server.Instance.Logger.Log($"[警告][{request.RequestCode}]未找到对应的处理器！", LogLevel.Warn);
                return new Response(StatusCode.BadRequest, new { Message = $"[{request.RequestCode}][{request.ActionCode}]是错误的请求" });
            }


        }



        public void ResponseHandler(IResponse response)
        {
            // TODO:对响应进行处理
        }
    }
}
