using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.DTO
{
    public class StockFavoriteDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public string memo { get; set; }
        public string type { get; set; }
        public string company { get; set; }
        public string category { get; set; }
        public double position { get; set; }
        public double closePrice { get; set; }
        public string selfmemo { get; set; }
    }
}
