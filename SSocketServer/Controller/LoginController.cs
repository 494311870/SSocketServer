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
    [Controller]
    class LoginController : BaseController
    {
        public override RequestCode RequestCode => RequestCode.Login;

        public override string ToString() => "Login";

        [Autowired] private IUserDao UserDao { get; set; }

        [RequestMapping(ActionCode.Get)]
        public IResponse Login(Request request)
        {
            var data = request.GetData<(string username, string password)>();

            var user = UserDao.VerifyUser(data.username, data.password);

            if (user is null)
            {
                return new Response(StatusCode.NotFound) { Message = "用户名或密码错误!" };
            }
            else
            {
                return new Response(StatusCode.OK) { Message = "登录成功" };
            }
        }


    }
}
