using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.DTO
{
    public class StockSysConfigDTO
    {
        public int id { get; set; }
        public DateTime stockUpdate { get; set; }
        public DateTime otcUpdate { get; set; }
        public DateTime nowDate { get; set; }
        public DateTime avgStartDate { get; set; }
        public DateTime avgEndDate { get; set; }
        public int totalCount { get; set; }
    }
}
