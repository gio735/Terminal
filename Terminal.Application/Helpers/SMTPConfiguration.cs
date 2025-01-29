using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Application.Helpers
{
    public class SMTPConfiguration
    {
        public string From { get; set; }
        public string Key { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
