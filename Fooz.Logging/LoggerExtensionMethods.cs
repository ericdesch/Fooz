using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooz.Logging
{
    public static class LoggerExtensionMethods
    {
        public static void Log(this ILoggerProvider logger, string message)
        {
            logger.Log(new LogEntry(LogLevel.Information, message, null, null));
        }

        public static void Log(this ILoggerProvider logger, string message, params object[] args)
        {
            logger.Log(new LogEntry(LogLevel.Information, message, null, args));
        }

        public static void Log(this ILoggerProvider logger, Exception exception)
        {
            logger.Log(new LogEntry(LogLevel.Error, exception.Message, exception, null));
        }

        public static void Log(this ILoggerProvider logger, LogLevel level, string message)
        {
            logger.Log(new LogEntry(level, message, null, null));
        }

        public static void Log(this ILoggerProvider logger, LogLevel level, string message, params object[] args)
        {
            logger.Log(new LogEntry(level, message, null, args));
        }

        public static void Log(this ILoggerProvider logger, LogLevel level, string message, Exception exception)
        {
            logger.Log(new LogEntry(level, message, exception, null));
        }

        public static void Log(this ILoggerProvider logger, LogLevel level, Exception exception, string message, params object[] args)
        {
            logger.Log(new LogEntry(level, message, exception, args));
        }
    }
}
