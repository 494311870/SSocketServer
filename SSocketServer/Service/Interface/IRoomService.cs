﻿using SSocketServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSocketServer.Service
{
    interface IRoomService
    {
        Room CreateRoom();
        IEnumerable<Room> AllRoom();

    }
}
