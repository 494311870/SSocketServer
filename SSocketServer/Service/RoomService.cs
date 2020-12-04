using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSocketServer.Servers;
using SSocketServer.Attributes;

namespace SSocketServer.Service
{
    [Service]
    class RoomService : IRoomService
    {
        public Room CreateRoom() => Server.Instance.GetRoom();

        public IEnumerable<Room> AllRoom() => Server.Instance.AllRooms;
        

    }
}
