using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class stockProfit
    {
        public int id { get; set; }
        public string code { get; set; }
        public int operId { get; set; }
        public double buyPrice { get; set; }
        public int buyShares { get; set; }
        public double buyCost { get; set; }
        public double profit { get; set; }
        public double profitPercentage { get; set; }
    }
}
