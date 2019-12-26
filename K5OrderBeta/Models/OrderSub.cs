using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K5OrderBeta.Models
{
    public class OrderSub
    {

        
        public int rowNum { get; set; }
        public string docNum { get; set; }
        public string productImage { get; set; }
        public string productName { get; set; }
        public string productCode { get; set; }
        public int IB { get; set; }
        public int PC { get; set; }
        public int CS { get; set; }
    }
}
