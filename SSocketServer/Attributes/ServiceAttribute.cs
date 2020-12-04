﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSocketServer.Attributes
{
    /// <summary>
    /// 标记为服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    class ServiceAttribute : Attribute
    {

    }
}
