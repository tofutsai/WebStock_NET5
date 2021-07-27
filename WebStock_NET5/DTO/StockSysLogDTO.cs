using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.DTO
{
    public class StockSysLogDTO
    {
        public int id { get; set; }
        public string type { get; set; }
        public DateTime date { get; set; }
        public string message { get; set; }
        public int totalCount { get; set; }
    }
}
