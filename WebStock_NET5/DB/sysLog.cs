using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class sysLog
    {
        public int id { get; set; }
        public string type { get; set; }
        public DateTime date { get; set; }
        public string message { get; set; }
    }
}
