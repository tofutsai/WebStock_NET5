using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class sysConfig
    {
        public int id { get; set; }
        public DateTime stockUpdate { get; set; }
        public DateTime otcUpdate { get; set; }
        public DateTime nowDate { get; set; }
        public DateTime avgStartDate { get; set; }
        public DateTime avgEndDate { get; set; }
    }
}
