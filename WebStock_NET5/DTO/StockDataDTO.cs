using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.DTO
{
    public class StockDataDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public DateTime dataDate { get; set; }
        public int shares { get; set; }
        public double turnover { get; set; }
        public double openPrice { get; set; }
        public double highestPrice { get; set; }
        public double lowestPrice { get; set; }
        public double closePrice { get; set; }
        public string company { get; set; }
        public int totalCount { get; set; }
    }
}
