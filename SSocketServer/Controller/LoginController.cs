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
using SSocketServer.Attributes;
using DataBase.Domain;

namespace SSocketServer.Controller
{
    class LoginController : BaseController
    {
        public UserDao UserDao { get; set; } = new UserDao();
        public override RequestCode ProtocolCode => RequestCode.Login;


        public override string ToString() => "Login";

        [RequestMapping(ActionCode.Get)]
        public IResponse Login(Request request)
        {
            var data = JsonMapper.ToObject<(string username, string password)>(request.Json);
            //var data = JsonMapper.ToObject(json);
            //var data = request.Data;
            //var username = data["Username"];
            //var password = data["Password"];
            //var user = UserDao.VerifyUser((string)username, (string)password);
            var user = UserDao.VerifyUser(data.username, data.password);

            Console.WriteLine(data.username);
            Console.WriteLine(data.password);
            Console.WriteLine(request.Client);
            if (user is null)
            {
                return new Response(StatusCode.NotFound, new { Message = "用户名或密码错误!" });
            }
            else
            {
                return new Response(StatusCode.OK, new { Message = "登录成功" });
            }
        }


    }
}
