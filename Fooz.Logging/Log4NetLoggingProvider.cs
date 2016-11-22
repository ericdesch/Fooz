using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooz.Logging
{
    public class Log4NetLoggingProvider : ILoggerProvider
    {
        public void Log(LogEntry entry)
        {
            // Need to get the calling method's type when instantiating an ILog object.
            var method = new StackFrame(1).GetMethod();
            var type = method.DeclaringType;

            log4net.ILog log = log4net.LogManager.GetLogger(type);

            switch (entry.Severity)
            {
                case LogLevel.Debug:
                    if (entry.Exception == null)
                        log.Debug(entry.Message);
                    else
                        log.Debug(entry.Message, entry.Exception);
                    break;
                case LogLevel.Information:
                    if (entry.Exception == null)
                        log.Info(entry.Message);
                    else
                        log.Info(entry.Message, entry.Exception);
                    break;
                case LogLevel.Warning:
                    if (entry.Exception == null)
                        log.Warn(entry.Message);
                    else
                        log.Warn(entry.Message, entry.Exception);
                    break;
                case LogLevel.Error:
                    if (entry.Exception == null)
                        log.Error(entry.Message);
                    else
                        log.Error(entry.Message, entry.Exception);
                    break;
                case LogLevel.Fatal:
                    if (entry.Exception == null)
                        log.Fatal(entry.Message);
                    else
                        log.Fatal(entry.Message, entry.Exception);
                    break;
                default:
                    if (entry.Exception == null)
                        log.Info(entry.Message);
                    else
                        log.Info(entry.Message, entry.Exception);
                    break;
            }
        }
    }
}
