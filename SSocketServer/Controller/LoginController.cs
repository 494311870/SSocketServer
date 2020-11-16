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

        public LoginController()
        {
            ProtocolCode = RequestCode.Login;
        }
        public override string ToString() => "Login";

        [RequestMapping(ActionCode.Get)]
        public IResponse Login(string json)
        {
            var data = JsonMapper.ToObject(json);
            var username = data["Username"];
            var password = data["Password"];
            var user = UserDao.VerifyUser((string)username, (string)password);

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
