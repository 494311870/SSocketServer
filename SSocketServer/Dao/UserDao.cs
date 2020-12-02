using System.Linq;
using DataBase.Data;
using DataBase.Domain;

namespace SSocketServer.Dao
{
    public class UserDao : IUserDao
    {
        public bool Add(User user)
        {
            using (var context = new DataContext())
            {
                context.Users.Add(user);
                return context.SaveChanges() > 0 ? true : false; // 提交
            }
        }

        public bool CheckUsername(string username)
        {
            using (var context = new DataContext())
            {
                var user = context.Users
                    .Where(x => x.Username.Equals(username))
                    .SingleOrDefault();
                return user is null; 
            }
        }

        public User VerifyUser(string username, string password)
        {
            using (var context = new DataContext())
            {
                var user = context.Users
                    .Where(x => x.Username.Equals(username) && x.Password.Equals(password))
                    .SingleOrDefault();

                return user;
            }
        }
    }
}
