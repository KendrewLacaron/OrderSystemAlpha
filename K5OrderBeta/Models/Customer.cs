using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace K5OrderBeta.Models
{
    public class Customer
    {
        [PrimaryKey,AutoIncrement]
        public int customerID { get; set; }
        public string customerCode { get; set; }
        public string customerName { get; set; }
    }
}
