using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Log
    {
        public long ID { get; set; }
        public System.DateTime LogTime { get; set; }
        public string LogSource { get; set; }
        public string LogMessage { get; set; }
        public KoalaBlog.Entity.Models.Enums.LogLevel LogLevel { get; set; }
    }
}
