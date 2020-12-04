using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Protocol;
using SSocketServer.Attributes;

namespace SSocketServer.Controller
{
    [Controller]
    class WaitingRoomController : BaseController
    {
        public override RequestCode RequestCode => RequestCode.WaitingRoom;
    }
}
