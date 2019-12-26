using System;
using System.Collections.Generic;
using System.Text;

namespace K5OrderBeta.Models
{
    public class GlobalReport
    {
        //OrderMain
        public string docNum { get; set; }
        public DateTime tDate { get; set; }
        public string custCode { get; set; }
        public string poNumber { get; set; }
        public string userCode { get; set; }
        public int orderStat { get; set; }

        //OrderSub
        public int rowNum { get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
        public string productCode { get; set; }
        public int IB { get; set; }
        public int PC { get; set; }
        public int CS { get; set; }

        //Customer
        public string customerCode { get; set; }
        public string customerName { get; set; }

        //User

        public string userName { get; set; }
        public string password { get; set; }

        //UserPrefs

        public int prefType { get; set; } //1 = Log in || 2 - Log Out || 3 - Import || 4 - Export
        public DateTime timeLog { get; set; }
    }
}
