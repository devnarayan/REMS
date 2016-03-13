using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access
{
   public class BaseActivity
    {
        #region Private Fields
        private readonly REMSDBEntities dbContext;
        private readonly DataFunctions context;
        #endregion
        public BaseActivity()
        {
            dbContext = new REMSDBEntities();
            context = new DataFunctions();
        }
        public int AddActivity(string userName,string Message, int flatID)
        {
            Hashtable hash = new Hashtable();
            hash.Add("UserName", userName);
            hash.Add("FlatID", flatID);
            hash.Add("Message", Message);
            int i=  context.ExecuteSP("spActivityLog", hash);
            return i;
        }
        public DataTable GetActityList(int flatID)
        {
            var dt = context.GetDataTable("select * from ActivityLog where FlatID='" + flatID + "'");
            return dt;
        }
        public bool UpdateFlatSaleStatus(int flatid, string username, string status)
        {
            bool bl = context.ExecuteNonQuery("update SaleFlat set Status='" + status + "', UpdateBy='" + username + "', UpdateDate='" + DateTime.Now + "' where FlatID='" + flatid + "'");
            bool b11 = context.ExecuteNonQuery("update Flat set Status='" + status + "' where FlatID='" + flatid + "'");
            return bl;
        }
    }
}
