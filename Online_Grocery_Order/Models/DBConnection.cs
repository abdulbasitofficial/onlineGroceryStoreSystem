using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Online_Grocery_Order.Models
{
    public class DBConnection
    {
        public static string constr = @"Data Source=DESKTOP-CL47E0L\SQLEXPRESS; Initial Catalog=grocery;Integrated Security=true";
       public  SqlConnection con = new SqlConnection(constr);
    }
}