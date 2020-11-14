using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SServer.Common.Specification;
using SServer.Common.Tools;
using SSocketServer.Controller;
using SServer.Common.Protocol;
using SServer.Common.Model;

namespace SSocketServer.Servers
{

    class Server
    {
        public static Server Instance { get; private set; }
        public readonly int MaxConnection = 100;
        /// <summary>
        /// 连接池 
        /// </summary>
        private List<Client> ClientList { get; } = new List<Client>();
        private TcpListener Listener { get; }

        private ControllerManager ControllerManager { get; } = new ControllerManager();
        private Parser Parser { get; set; }

        //private Task task;

        /// <summary>
        /// 创建一个TcpListener，可以直接使用ip地址和端口号构造
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        public Server(string ip, int port) => Listener = new TcpListener(IPAddress.Parse(ip), port);
        public Server(IPAddress localaddr, int port) => Listener = new TcpListener(localaddr, port);
        public Server(IPEndPoint localEP) => Listener = new TcpListener(localEP);


        public async void Run(string message = "服务器运行起来了！")
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Console.WriteLine("[警告]只允许同时启动一个服务器！");
                return;
            }
            //
            Parser = new Parser(RequestHandleAsync, ResponseHandleAsync);
            // 初始化连接池 与最大连接数保持一致
            foreach (var i in Enumerable.Range(0, MaxConnection))
                ClientList.Add(new Client(this));

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:初始化了{ClientList.Count}个对等客户端");
            // 请求队列
            Listener.Start(MaxConnection);
            Console.WriteLine(message.PadLeft(15 + message.Length / 2, '-').PadRight(30, '-'));
            // 异步接受客户端连接请求
            await AcceptAsync();
        }

        private async Task AcceptAsync()
        {
            Socket socket = await Listener.AcceptSocketAsync();
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:一个客户端连接进来了！{socket.RemoteEndPoint.ToString()}");

            Client client = GetClient();
            if (client is null)
            {
                socket.Close();
                Console.WriteLine("无可用连接");
            }
            else
            {
                client.Use(socket);
            }
            //Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:剩余可用数量：{ClientList.Count(x => x.CanUse)}");
            await AcceptAsync();
        }

        private Client GetClient() => ClientList.FirstOrDefault(x => x.CanUse);

        private async Task RequestHandleAsync(IRequest request, IClient client)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("服务器处理了一条请求");
                var response = ControllerManager.RequestHandler(request);
                Console.WriteLine(response.Json);
                client.SendMessage(Message.GetBytes(response));
            });
        }

        private async Task ResponseHandleAsync(IResponse response, IClient client)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("服务器处理了一条响应");
                ControllerManager.ResponseHandler(response);
            });
        }


        public void Parse(byte[] bytes, IClient client)
        {
            Console.WriteLine("服务器解析了一条数据");
            Parser.Parse(bytes, client);
        }
    }
}
