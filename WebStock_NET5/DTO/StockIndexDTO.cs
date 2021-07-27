using System;

namespace WebStock_NET5.DTO
{
    public class StockIndexDTO
    {
        public int id { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string code { get; set; }
        public string company { get; set; }
        public DateTime? dataDate { get; set; }
        public bool? isEnable { get; set; }
        public int totalCount { get; set; }
    }
}
