using SServer.Common.Protocol;
using SServer.Common.Specification;
using SSocketServer.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using DataBase.Domain;
using SServer.Common.Model;

namespace SSocketServer.Controller
{
    class RegisterController : BaseController
    {
        public RegisterController()
        {
            ProtocolCode = RequestCode.Register;
        }

        public UserDao UserDao { get; set; } = new UserDao();


        public IResponse Register(string json)
        {
            var data = JsonMapper.ToObject(json);

            var username = (string)data["Username"];
            var password = (string)data["Password"];
            var user = new User { Username = username, Password = password };
            UserDao.Add(user);
            return new Response(StatusCode.OK, new { Message = "注册成功！" });

        }

    }
}
