using System;
using System.Collections.Generic;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class member
    {
        public int id { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public bool providerFB { get; set; }
        public bool providerGoogle { get; set; }
        public string role { get; set; }
        public bool isAdmin { get; set; }
    }
}
