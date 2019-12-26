using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K5OrderBeta.Models
{
    public class OrderMain
    {

        [PrimaryKey]
        public string docNum { get; set; }
        public DateTime tDate { get; set; }
        public string custCode { get; set; }
        public string poNumber { get; set; }
        public string userCode { get; set; }
        public int orderStat { get; set; }
        public byte[] signature { get; set; }
        public bool pending { get; set; }
    }
}
