using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooz.Logging
{
    public class NLogLoggerProvider : ILoggerProvider
    {
        public void Log(LogEntry entry)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            NLog.LogLevel level = GetLogLevel(entry.Severity);

            logger.Log(level, entry.Exception, entry.Message, entry.Args);
        }

        private NLog.LogLevel GetLogLevel(LogLevel severity)
        {
            NLog.LogLevel retVal;

            switch (severity)
            {
                case LogLevel.Debug:
                    retVal = NLog.LogLevel.Debug;
                    break;
                case LogLevel.Information:
                    retVal = NLog.LogLevel.Info;
                    break;
                case LogLevel.Warning:
                    retVal = NLog.LogLevel.Warn;
                    break;
                case LogLevel.Error:
                    retVal = NLog.LogLevel.Error;
                    break;
                case LogLevel.Fatal:
                    retVal = NLog.LogLevel.Fatal;
                    break;
                default:
                    retVal = NLog.LogLevel.Info;
                    break;
            }

            return retVal;
        }
    }
}
