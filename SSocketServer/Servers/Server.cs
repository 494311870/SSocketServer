using SServer.Common.Specification;
using SServer.Common.Tools;
using SSocketServer.Manager;
using SSocketServer.Middleware;
using SSocketServer.Util.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SSocketServer.Servers
{

    class Server
    {
        // 这里在考虑要不要做成单例模式，随后会修改
        public static Server Instance { get; private set; }
        public readonly int MaxConnection = 5;
        public readonly int MaxRoomNumbers = 4;
        public Logger Logger { get; set; } = new Logger();

        /// <summary>
        /// 连接池 
        /// 使用列表和表标记的好处是可以在快速使用和释放连接，但查找一个可用连接的时间是 O(n)
        /// 使用队列 + 集合 可以在 O(1) 时间里获取获取和释放
        /// </summary>
        //private List<Client> ClientList { get; } = new List<Client>();
        /// <summary>
        /// 空闲连接队列
        /// </summary>
        protected Queue<Client> FreeClients { get; set; } = new Queue<Client>(100);
        /// <summary>
        /// 使用中的连接集合
        /// </summary>
        protected HashSet<Client> UsingClients { get; set; } = new HashSet<Client>();

        protected TcpListener Listener { get; }
        /// <summary>
        /// 这样子的话 房间也是一个对象池，也可以只用一个HashSet替代，
        /// </summary>
        protected Queue<Room> FreeRooms { get; set; } = new Queue<Room>();
        protected HashSet<Room> UsingRooms { get; set; } = new HashSet<Room>();
        public IEnumerable<Room> AllRooms => UsingRooms.AsEnumerable();

        protected ControllerManager ControllerManager { get; } = new ControllerManager();
        protected Parser Parser { get; set; }
        protected RequestMiddleware RequestMiddleware { get; set; }
        protected ResponseMiddleware ResponseMiddleware { get; set; }

        //private Task task;

        /// <summary>
        /// 创建一个TcpListener，可以直接使用ip地址和端口号构造
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        public Server(string ip, int port) : this(IPAddress.Parse(ip), port) { }
        public Server(IPAddress localaddr, int port) => Listener = new TcpListener(localaddr, port);
        public Server(IPEndPoint localEP) => Listener = new TcpListener(localEP);

        /// <summary>
        /// 启动服务器!
        /// </summary>
        /// <param name="message"></param>
        public async void Run(string message = "服务器运行起来了！")
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Logger.Log("只允许同时启动一个服务器！", LogLevel.Warn);
                return;
            }
            // 处理未观察到的异常
            TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
            {
                eventArgs.SetObserved();
                (eventArgs.Exception).Handle(e =>
                {
                    Logger.Log($"Exception type: {e.GetType()}\n{e}", LogLevel.Error);
                    return true;
                });
            };
            // 加载解析器
            Parser = new Parser(RequestHandleAsync, ResponseHandleAsync);
            // 加载中间件
            RequestMiddleware = new RequestMiddleware(this);
            ResponseMiddleware = new ResponseMiddleware(this);
            // 初始化连接池 与最大连接数保持一致
            foreach (var i in Enumerable.Range(0, MaxConnection))
                FreeClients.Enqueue(new Client(this));
            // 初始化房间
            foreach (var i in Enumerable.Range(0, MaxRoomNumbers))
                FreeRooms.Enqueue(new Room(i));

            Logger.Log($"初始化了{FreeClients.Count}个对等客户端");
            // 设置请求队列的最大值
            Listener.Start(MaxConnection);
            Logger.Log(message);
            // Test
            for (int i = 1; i < 3; i++)
            {
                var room = GetRoom();
                room.Init(GetClient(), i, i * 2, $"TestRoom{i}");
            }

            //
            // 异步接受客户端连接请求
            await AcceptAsync();
        }

        private async Task AcceptAsync()
        {
            Socket socket = await Listener.AcceptSocketAsync();
            Logger.Log($"一个客户端连接进来了！{socket.RemoteEndPoint}");

            Client client = GetClient();
            if (client is null)
            {
                socket.Close();
                Logger.Log("无可用连接", LogLevel.Warn);
            }
            else
            {
                client.Use(socket);
            }
            Logger.Log($"剩余可用数量:{FreeClients.Count}");
            await AcceptAsync();
        }
        #region 管理客户端
        private Client GetClient()
        {
            Client client;
            lock (FreeClients)
            {
                client = FreeClients.Any() ? FreeClients.Dequeue() : null;
            }

            if (client != null)
            {
                lock (UsingClients) { UsingClients.Add(client); }
            }

            return client;
        }

        public void FreeClient(Client client)
        {
            lock (UsingClients) { UsingClients.Remove(client); }

            lock (FreeClients) { FreeClients.Enqueue(client); }
        }



        #endregion

        #region 房间
        public Room GetRoom()
        {
            Room room;
            lock (FreeRooms)
            {
                room = FreeRooms.Any() ? FreeRooms.Dequeue() : null;
            }

            if (room != null)
            {
                lock (UsingRooms) { UsingRooms.Add(room); }
            }

            return room;
        }

        public void FreeRoom(Room room)
        {
            lock (UsingRooms) { UsingRooms.Remove(room); }

            lock (FreeRooms) { FreeRooms.Enqueue(room); }
        }


        #endregion


        #region 请求和响应
        private async Task RequestHandleAsync(IRequest request)
        {
            await Task.Run(() => RequestHandle());

            void RequestHandle()
            {
                // 请求中间件
                RequestMiddleware.BeforeWrapper(request);
                // 让控制器管理者去处理请求
                var response = ControllerManager.RequestHandler(request);
                // 响应中间件
                ResponseMiddleware.AfterWrapper(response);
                // 向客户端发送响应
                request.Client.SendMessage(Message.GetBytes(response));
            }
        }

        private async Task ResponseHandleAsync(IResponse response)
        {
            // TODO 
            await Task.Run(() =>
            {
                ControllerManager.ResponseHandler(response);
            });
        }

        public void Parse(byte[] bytes, IClient client)
        {
            Logger.Log("服务器解析了一条数据");
            Parser.Parse(bytes, client);
        }
        #endregion
    }
}
