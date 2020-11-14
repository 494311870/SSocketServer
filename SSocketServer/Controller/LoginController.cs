using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Protocol;
using SServer.Common.Model;
using LitJson;
using SServer.Common.Specification;
using SSocketServer.Dao;

namespace SSocketServer.Controller
{
    class LoginController : BaseController
    {
        public UserDao UserDao { get; set; } = new UserDao();

        public LoginController()
        {
            ProtocolCode = RequestAndResponseCode.Login;
        }
        public override string ToString() => "Login";

        [RequestMapping(ActionCode.Get)]
        public IResponse Login(string json)
        {
            var data = JsonMapper.ToObject(json);
            var username = data["Username"];
            var password = data["Password"];
            var user = UserDao.VerifyUser((string)username, (string)password);
            Console.WriteLine("UserDao.VerifyUser");
            if (user is null)
            {
                return new Response(ProtocolCode, StatusCode.NotFound, JsonMapper.ToJson(new { Message = "用户名或密码错误!" }));
            }
            else
            {
                return new Response(ProtocolCode, StatusCode.OK, JsonMapper.ToJson(new { Message = "登录成功" }));
            }
            return new Response(ProtocolCode, StatusCode.OK, JsonMapper.ToJson(new { Message = "登录成功" }));
        }
    }
}
