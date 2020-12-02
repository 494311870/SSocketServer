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
using SSocketServer.Attributes;

namespace SSocketServer.Controller
{
    class RegisterController : BaseController
    {
        public override RequestCode ProtocolCode => RequestCode.Register;

        public UserDao UserDao { get; set; } = new UserDao();

        

        [RequestMapping(ActionCode.Post)]
        public IResponse Register(string json)
        {
            var data = JsonMapper.ToObject(json);

            var username = (string)data["Username"];
            var password = (string)data["Password"];
            var user = new User { Username = username, Password = password };

            if (UserDao.Add(user))
            {
                return new Response(StatusCode.OK, new { Message = "注册成功！" });
            }
            else
            {
                return new Response(StatusCode.InternalServerError, new { Message = "注册失败！" });
            }
        }

        [RequestMapping(ActionCode.Get)]
        public IResponse CheckUsername(string json)
        {
            var data = JsonMapper.ToObject(json);

            var username = (string)data["Username"];



            if (UserDao.CheckUsername(username))
            {
                return new Response(StatusCode.OK, new { Message = "用户名可用！", Result = true });
            }
            else
            {
                return new Response(StatusCode.OK, new { Message = "用户名重复！", Result = false });
            }
        }
    }
}
