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

namespace SSocketServer.Controller
{
    class RoomController : BaseController
    {
        public override RequestCode ProtocolCode => RequestCode.Room;

        [RequestMapping(ActionCode.Post)]
        public IResponse CreateRomm(Request request)
        {
            //var data = JsonMapper.ToObject<(int numberMax, string roomName, string password)>(request.Json);
            var data = request.GetData<(int numberMax, string roomName, string password)>();
            //var data = JsonMapper.ToObject(json);
            var room = Server.Instance.GetRoom();


            //room.Init(request.Client as Client, (int)request.Data["Number"], request.Data["NumberMax"], request.Data["IsLocked"], request.Data["RoomName"]);

            if (room != null)
            {
                //room.Create();
                room.Init(request.Client as Client, 1, data.numberMax, data.roomName, data.password);
                var (id, number, numberMax, isLocked, roomName) = room;

                return new Response(StatusCode.Created, (id, number, numberMax, isLocked, roomName)) { Message = "创建成功" };
                //return new Response(StatusCode.Created, new
                //{
                //    Message = "创建成功！",
                //    Room = (id, number, numberMax, isLocked, roomName),
                //});
            }

            return new Response(StatusCode.ServiceUnavailable, new { Message = "服务器房间数量已达到上限" });
        }
    }
}
