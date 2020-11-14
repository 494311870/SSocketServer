using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Specification
{
    public interface IClient
    {
        void SendMessage(byte[] message);
    }
}
