using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStock_NET5.DB;

namespace WebStock_NET5.DTO
{
    public class StockStatisticsDTO 
    {
        public string code { get; set; }
        public double highestPrice { get; set; }
        public double lowestPrice { get; set; }
        public int avgShares { get; set; }
        public double avgTurnover { get; set; }
        public string type { get; set; }
        public string company { get; set; }
        public string category { get; set; }
        public double position { get; set; }
        public double closePrice { get; set; }
        public DateTime dataDate { get; set; }
        public string memo { get; set; }
        public int totalCount { get; set; }

    }
}
