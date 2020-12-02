using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SServer.Common.Tools;
using SServer.Common.Specification;

namespace SSocketServer.Servers
{
    /// <summary>
    /// 对等客户端
    /// 用来保持和客户端建立的连接
    /// </summary>
    class Client : IClient
    {
        //public bool CanUse { get; private set; } = true;
        public TcpClient TcpClient { get; } = new TcpClient();

        public Server Server { get; }
        private Message Message { get; } = new Message();
        private NetworkStream Stream { get; set; }

        public Client(Server server)
        {
            Server = server;
        }

        public void Use(Socket socket)
        {
            TcpClient.Client = socket;
            //TcpClient.Client.ReceiveAsync
            //CanUse = false;
            // 获取一个新的网络流 
            Stream = new NetworkStream(socket, true);
            // 异步接受消息
            ReceiveMessage();
        }
        public void Release()
        {
            try
            {
                TcpClient.Client?.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                TcpClient.Client.Close();
                TcpClient.Client = null;
            }

            Stream?.Close();
            Stream = null; // 释放指针

            //CanUse = true;
            Server.FreeClient(this);
            Console.WriteLine("关闭了一个客户端连接");

        }

        // 接受客户端发送的消息
        public async void ReceiveMessage()
        {
            try
            {
                // 接收到断开连接消息时，释放连接
                if (TcpClient.Client.Poll(10, SelectMode.SelectRead))
                {
                    Console.WriteLine("难以置信,客户端居然无情的断开了连接");
                    Release();
                    return;
                }

                var length = await Stream.ReadAsync(Message.Buffer, Message.Current, Message.BufferRemain);
                Console.WriteLine($"从网络流中读取到长度为{length}的消息！");
                // 读取数据，此操作是一个同步操作,但解析操作是异步的，因为同时操作缓冲区存在线程安全问题。
                Message.Read(length, ParseAsync);
                ReceiveMessage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Release();
            }
        }

        public void SendMessage(byte[] message)
        {
            Console.WriteLine("正在向客户端发送消息");
            TcpClient.Client.Send(message);
        }

        async Task ParseAsync(byte[] bytes) => await Task.Run(() => Server.Parse(bytes, this));
    }
}
