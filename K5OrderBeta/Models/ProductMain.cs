using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K5OrderBeta.Models
{
    public class ProductMain
    {
        [PrimaryKey, AutoIncrement]
        public int productID { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productDesc { get; set; }
        public string productCat { get; set; }
        public int productIB { get; set; }
        public int productPC { get; set; }
        public int productCS { get; set; }
        public int productStat { get; set; }
        public string productImage { get; set; }
        public bool isVisible { get; set; }
    }
}
