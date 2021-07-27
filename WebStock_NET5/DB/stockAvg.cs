using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class stockAvg
    {
        public int id { get; set; }
        public string code { get; set; }
        public double avgPrice { get; set; }
        public double highestPrice { get; set; }
        public double lowestPrice { get; set; }
        public int avgShares { get; set; }
        public double avgTurnover { get; set; }
    }
}
