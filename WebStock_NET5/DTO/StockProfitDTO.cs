using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.DTO
{
    public class StockProfitDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public double buyPrice { get; set; }
        public int buyShares { get; set; }
        public double buyCost { get; set; }
        public double profit { get; set; }
        public double profitPercentage { get; set; }
        public string company { get; set; }
        public double position { get; set; }
        public double closePrice { get; set; }
        public int totalCount { get; set; }
        public string memo { get; set; }
    }
}
