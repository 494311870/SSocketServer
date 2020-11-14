using DataBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSocketServer.Dao
{
    interface IUserDao
    {
        bool Add(User user);

        User VerifyUser(string username, string password); // 验证用户名密码
    }
}
