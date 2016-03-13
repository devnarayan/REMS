using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access
{
    public class Connection
    {

        public string ConnectionString()
        {
            string StrConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
            return StrConnection;
        }
        public static void OpenConnection(SqlConnection con)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
        }
        public static void CloseConnection(SqlConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close(); con.Dispose();
            }
        }
    }
}
