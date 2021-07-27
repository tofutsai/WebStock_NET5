using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class stockIndex
    {
        public int id { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string code { get; set; }
        public string company { get; set; }
        public DateTime dataDate { get; set; }
        public bool isEnable { get; set; }
    }
}
