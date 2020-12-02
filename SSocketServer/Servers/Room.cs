using SServer.Common.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSocketServer.Servers
{
    enum RoomState
    {
        None,
        WaitingJoin,
        waitingBattle,
        Battle,
        End
    }
    class Room
    {
        // 房间号
        public int Id { get; set; }

        public RoomState RoomState { get; private set; } = RoomState.WaitingJoin;

        public int Number { get; set; }

        private int numberMax;
        public int NumberMax
        {
            get => numberMax;
            set => numberMax = value < Number ? Number : value; //房间人数上限不能小于当前房间人数
        }


        public string RoomName { get; set; }
        public bool IsLocked => !string.IsNullOrEmpty(Password);
        public string Password { get; set; }


        private HashSet<Client> Clients { get; } = new HashSet<Client>();
        public Client Host { get; set; }

        public Room(int id)
        {
            Id = id;
        }

        public void Deconstruct(out int id, out int number, out int numberMax, out bool isLocked, out string roomName)
        {
            id = Id;
            number = Number;
            numberMax = NumberMax;
            isLocked = IsLocked;
            roomName = RoomName;
        }

        public void Init(Client client, int number, int numberMax, string roomName, string password)
        {
            Clients.Clear();
            Host = client;
            AddClient(Host);
            RoomState = RoomState.WaitingJoin;
            NumberMax = numberMax;
            Number = number;
            RoomName = roomName;
            Password = password;
        }

        void AddClient(Client client)
        {
            Clients.Add(client);
            // TODO 向房间内的玩家进行一次广播，刷新他们的面板以显示新玩家

        }

        void RemoveClient(Client client)
        {
            Clients.Remove(client);
            // TODO 向房间内的玩家进行一次广播，刷新他们的面板以显示新玩家
        }

        // 获取房间内的玩家列表
        public void ShowClients()
        {

        }

    }
}
