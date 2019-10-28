using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CGateService
{
    class myConn
    {
        public OdbcConnection conn;
        public string host = "10.0.5.11";
        public string server = "knit";
        public string db = "dftShop";
        public string uid = "itpro";
        public string pass = "123456";
        public myConn()
        {
            string connString = "Driver={SQL Anywhere 16};host=" + host + ";server=" + server + ";db=" + db + ";uid=" + uid + ";pwd=" + pass + ";Named Parameters=false";
            conn = new OdbcConnection(connString);
        }
        public myConn(string host, string server, string db, string uid, string pass)
        {
            string connString = "Driver={SQL Anywhere 16};host=" + host + ";server=" + server + ";db=" + db + ";uid=" + uid + ";pwd=" + pass;
            conn = new OdbcConnection(connString);
        }
    }
}
