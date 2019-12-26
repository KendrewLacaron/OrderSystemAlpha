using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace K5OrderBeta.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int userID { get; set; }
        public string userCode { get; set; }
        public string userName { get; set; }
        public string loginName { get; set; }
        public string password { get; set; }

    }
}
