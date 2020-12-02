using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSocketServer.Attributes
{
    /// <summary>
    /// 用于自动装配需要的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    class AutowiredAttribute : Attribute
    {

    }
}
