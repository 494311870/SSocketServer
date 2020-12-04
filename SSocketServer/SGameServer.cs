using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSocketServer.Servers;
using LitJson;
using System.Configuration;
using SSocketServer.Util.Container;

namespace SSocketServer
{
    public class SGameServer
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 9966);
            Console.WriteLine("??");
            server.Run("服务器运行起来了！");


            while (true)
            {
                switch (Console.ReadLine().ToLower())
                {
                    case "quit":
                        return;
                    default:
                        break;
                }
            }
            //SContainer.Test();

        }
    }

    class MyClass
    {

    }
}
