using Microsoft.Extensions.Logging;
using System;

namespace WarcraftGuild.WoW.Handlers
{
    public class LogEvent
    {
        public string Message { get; set; }
        public string Trace { get; set; }
        public LogLevel Severity { get; set; }
        public DateTime LogDate { get; set; }
    }
}