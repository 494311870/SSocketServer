using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Protocol;
using SServer.Common.Specification;
using LitJson;
using SSocketServer.Servers;
using SSocketServer.Attributes;
using SServer.Common.Model;
using SSocketServer.Service;

namespace SSocketServer.Controller
{
    [Controller]
    class RoomController : BaseController
    {
        public override RequestCode RequestCode => RequestCode.Room;

        [Autowired] private IRoomService RoomService { get; set; }
        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestMapping(ActionCode.Post)]
        public IResponse CreateRoom(Request request)
        {
            var data = request.GetData<(int numberMax, string roomName, string password)>();
            var room = RoomService.CreateRoom();

            if (room != null)
            {
                room.Init(request.Client as Client, 1, data.numberMax, data.roomName, data.password);

                var (id, number, numberMax, isLocked, roomName) = room;

                return new Response(StatusCode.Created, (id, number, numberMax, isLocked, roomName)) { Message = "创建成功" };
            }

            return new Response(StatusCode.ServiceUnavailable) { Message = "服务器房间数量已达到上限" };
        }
        /// <summary>
        /// 获取房间列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // 这个可以做分页查询和条件过滤，暂时用不到就不做了
        [RequestMapping(ActionCode.Get)]
        public IResponse GetRoomList(Request request)
        {
            var roomList = new List<(int id, int number, int numberMax, bool isLocked, string roomName)>();
            int id;
            int number;
            int numberMax;
            bool isLocked;
            string roomName;

            foreach (var room in RoomService.AllRoom())
            {
                roomList.Add((id, number, numberMax, isLocked, roomName) = room);
            }

            return new Response(StatusCode.Created, roomList);
        }
    }
}
