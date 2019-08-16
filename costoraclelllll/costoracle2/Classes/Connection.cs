using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Classes
{
    public class Connection
    {
        private string constr;
        public Connection()
        {
            constr =ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        }
        public string DbConnectionString
        {
            get
            {
                return constr;
            }
        }
    }
}