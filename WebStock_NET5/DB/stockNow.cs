using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class stockNow
    {
        public int id { get; set; }
        public string code { get; set; }
        public double closePrice { get; set; }
        public double position { get; set; }
        public DateTime dataDate { get; set; }
    }
}
