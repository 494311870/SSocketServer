using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer.Common.Tools
{
    /// <summary>
    /// 日志级别
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// 不显示任何日志
        /// </summary>
        None = 0,
        /// <summary>
        /// 显示Debug信息
        /// </summary>
        Debug = 1,
        /// <summary>
        /// 显示应用程序的运行过程
        /// </summary>
        Info = 2,
        /// <summary>
        /// 潜在的错误
        /// </summary>
        Warn = 4,
        /// <summary>
        /// 发生错误但程序仍能运行
        /// </summary>
        Error = 8,
        /// <summary>
        /// 严重的错误
        /// </summary>
        Fatal = 16,
        /// <summary>
        /// 显示所有级别的日志
        /// </summary>
        All = 31,
    }

    /// <summary>
    /// 日志记录器 
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 日志显示级别
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.All;
        /// <summary>
        /// 如何打印日志
        /// </summary>
        public Action<string> LogAction { get; set; } = ConsoleLog;
        /// <summary>
        /// 是否显示时间戳
        /// </summary>
        public bool ShowTime { get; set; } = true;
        /// <summary>
        /// 时间戳格式
        /// </summary>
        public string TimeFormat { get; set; } = "u";


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="log"></param>
        public void Log(string log, LogLevel level = LogLevel.Info)
        {
            if ((level & LogLevel) == level)
            {
                var s = $"{(ShowTime ? $"[{ DateTime.Now.ToString(TimeFormat)}]" : string.Empty)}[{level}]{log}";
                LogAction?.Invoke(s);
            }
        }

        /// <summary>
        /// 在控制台输出日志
        /// </summary>
        /// <param name="log"></param>
        public static void ConsoleLog(string log)
        {
            Console.WriteLine(log);
        }
    }
}
