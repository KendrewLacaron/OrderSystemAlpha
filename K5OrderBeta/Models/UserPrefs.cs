using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace K5OrderBeta.Models
{
    public class UserPrefs
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string userCode { get; set; }
        public int prefType { get; set; } //1 = Log in || 2 - Log Out || 3 - Import || 4 - Export
        public DateTime timeLog { get; set; }
        public int status { get; set; }
        public string customerCode { get; set; }
    }
}
