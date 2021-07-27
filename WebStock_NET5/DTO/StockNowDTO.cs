using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.DTO
{
    public class StockNowDTO
    {
        public string code { get; set; }
        public double closePrice { get; set; }
        public DateTime dataDate { get; set; }
        public double highestPrice { get; set; }
        public double lowestPrice { get; set; }
    }
}
