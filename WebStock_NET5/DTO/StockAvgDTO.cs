using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.DTO
{
    public class StockAvgDTO
    {
        public string code { get; set; }
        public double avgPrice { get; set; }
        public double highestPrice { get; set; }
        public double lowestPrice { get; set; }
        public int avgShares { get; set; }
        public double avgTurnover { get; set; }
        public int dataYear { get; set; }
    }
}
