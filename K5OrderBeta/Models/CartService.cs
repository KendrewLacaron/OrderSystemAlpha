using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K5OrderBeta.Models
{
    public class CartService
    {
        [AutoIncrement,PrimaryKey]
        public int productRowCart { get; set; }
        public string productCode { get; set; }
        public string productImage { get; set; }
        public string productName { get; set; }
        public int IB { get; set; }
        public int PC { get; set; }
        public int CS { get; set; }
        public string orderUM { get; set; }
       
    }
}
