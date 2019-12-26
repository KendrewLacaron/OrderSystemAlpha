using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K5OrderBeta.Models
{
    public class IPConfig
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ConfigIP { get; set; }
        public string ConfigPort { get; set; }
    }
}
