using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SServer.Common.Protocol;

namespace SSocketServer.Controller
{
    class DefaultController : BaseController
    {
        public override RequestCode ProtocolCode => RequestCode.None;

        public override string ToString() => "Default";


    }
}
