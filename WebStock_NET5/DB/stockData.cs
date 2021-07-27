using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class stockData
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
        public double spread { get; set; }
    }
}
