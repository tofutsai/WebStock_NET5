using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class stockFavorite
    {
        public int id { get; set; }
        public int operId { get; set; }
        public string code { get; set; }
        public string memo { get; set; }
    }
}
