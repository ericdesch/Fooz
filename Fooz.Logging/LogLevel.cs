using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooz.Logging
{
    // Severity levels supported by both NLog and log4net.
    // So you can write your own ILogger for either and it will work with these.
    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    };
}
