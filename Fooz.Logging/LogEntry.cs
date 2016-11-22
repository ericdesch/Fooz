using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CuttingEdge.Conditions;

namespace Fooz.Logging
{
    // Immutable DTO that contains the log information.
    public class LogEntry
    {
        public readonly LogLevel Severity;
        public readonly string Message;
        public readonly Exception Exception;
        public readonly object[] Args;

        public LogEntry(LogLevel severity, string message, Exception exception = null, params object[] args)
        {
            //CuttingEdge.Conditions
            Condition.Requires(message, "message").IsNotNullOrEmpty();
            this.Severity = severity;
            this.Message = message;
            this.Exception = exception;
            this.Args = args;
        }
    }
}
