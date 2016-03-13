using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access
{
    public class DataFunctions
    {
        //SqlConnection cn_connection;
        Connection obj_connectionstr = new Connection();
        ILog logger = log4net.LogManager.GetLogger(typeof(DataFunctions));
        public DataFunctions()
        {
            //
            // TODO: Add constructor logic here
            //
        }
       
        public Boolean ExecuteNonQuery(String Query)
        {
            Boolean ReturnBoolean;
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand(Query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                cmd.Dispose();
                ReturnBoolean = true;
            }
            catch (Exception ex)
            {
                ReturnBoolean = false;
            }
            return ReturnBoolean;
        }
        public string DB_IndianDateFormat(string strDate)
        {
            return Convert.ToDateTime(strDate).Date.ToString("dd/MM/yyyy");
        }

        public string Text_IndianDateFormat(string strDate)
        {
            try
            {
                return Convert.ToDateTime(strDate).Date.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                string[] strCustome = strDate.Split('/');
                string newDate = null;
                newDate = strCustome[1] + "/" + strCustome[0] + "/" + strCustome[2];
                return Convert.ToDateTime(newDate).Date.ToString("MM/dd/yyyy");
            }
        }
        public string ExecuteNonQuery1(String Query)
        {
            Boolean ReturnBoolean;
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand(Query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                cmd.Dispose();
                return "Executed Successfully.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public Boolean IsDuplicate(String Value, String FieldName, String TableName)
        {
            Boolean ReturnBoolean;
            String Query;
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            Query = "SELECT COUNT(*) as 'no' from " + TableName + " WHERE " + FieldName + " = " + Value;
            SqlCommand cmd = new SqlCommand(Query, con);
            try
            {
                con.Open();
                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                {
                    ReturnBoolean = true;
                }
                else
                {
                    ReturnBoolean = false;
                }
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                ReturnBoolean = true;
            }
            return ReturnBoolean;
        }

        public DataTable GetDataTable(String Query)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            DataTable ReturnTable = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(Query, con);
            DataSet ds = new DataSet();
            try
            {
                adp.Fill(ds);
                adp.Dispose();
                ReturnTable = ds.Tables[0];
            }
            catch (Exception ex)
            {
            }
            return ReturnTable;
        }
        public DataTable spGetDatatable(string spName, Hashtable HT)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            using (SqlCommand _cmd = new SqlCommand(spName, con))
            {
                try
                {

                    string key = null;
                    _cmd.CommandType = CommandType.StoredProcedure;
                    foreach (string key_loopVariable in HT.Keys)
                    {
                        key = key_loopVariable;
                        _cmd.Parameters.AddWithValue("@" + key, HT[key]);
                    }

                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    DataTable dt = new DataTable();
                    _dap.Fill(dt);
                    return dt;

                }
                catch (SqlException ex)
                {
                    logger.Error("SP=" + spName + " SqlException: " + ex.ToString());
                    return null;
                }

            }
        }

        public DataSet GetDataSet(String Query)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);

            SqlDataAdapter adp = new SqlDataAdapter(Query, con);
            DataSet ds = new DataSet();
            try
            {
                adp.Fill(ds);
                adp.Dispose();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet GetDataSETFromProcedure(string Procedure, Hashtable HT)
        {
            string key = null;
            DataSet ds = new DataSet();
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand(Procedure, con);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                foreach (string key_loopVariable in HT.Keys)
                {
                    key = key_loopVariable;
                    cmd.Parameters.AddWithValue("@" + key, HT[key]);
                }
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                adp.Dispose();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Dispose();
                cmd.Dispose();
            }
            return ds;

        }
        public DataTable GetDataTableFromProcedure(string Procedure, Hashtable HT)
        {
            string key = null;
            DataTable dt = new DataTable();
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 200000;
            cmd = new SqlCommand(Procedure, con);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                foreach (string key_loopVariable in HT.Keys)
                {
                    key = key_loopVariable;
                    cmd.Parameters.AddWithValue("@" + key, HT[key]);
                }
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                adp.Dispose();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Dispose();
                cmd.Dispose();
            }
            return dt;

        }
        ///////////////%%%%%%%%5
        public string GetMax(string fld, string tbl)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cc = new SqlCommand("select Isnull(max(" + fld + "),0) from " + tbl, con);
            con.Open();
            string a = cc.ExecuteScalar().ToString();
            con.Close();
            con.Dispose();
            cc.Dispose();
            return a;
        }

        public DataTable SelREC(string Query)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlDataAdapter adp = new SqlDataAdapter(Query, con);
            DataTable d = new DataTable();
            adp.Fill(d);
            adp.Dispose();
            con.Close();
            con.Dispose();
            return d;
        }

        public Boolean ExecuteProcedure(String Procedure, Hashtable HT)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand(Procedure, con);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                foreach (String key in HT.Keys)
                {
                    cmd.Parameters.AddWithValue("@" + key, HT[key]);
                }
                con.Open();
                int i = cmd.ExecuteNonQuery();

                con.Close();
                con.Dispose();
                cmd.Dispose();
                if (i > 0) return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                //Helper h = new Helper();
                //h.LogException(ex);
                return false;
            }
        }
        public int ExecuteSP(String Procedure, Hashtable HT)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand(Procedure, con);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                foreach (String key in HT.Keys)
                {
                    cmd.Parameters.AddWithValue("@" + key, HT[key]);
                }
                con.Open();
                int i = cmd.ExecuteNonQuery();

                con.Close();
                con.Dispose();
                cmd.Dispose();
                return i;
            }
            catch (Exception ex)
            {
                logger.Error("SP=" + Procedure + " Exception= " + ex.ToString());
                return 0;
            }
        }
        public async Task<int> ExecuteSPAsyn(String Procedure, Hashtable HT)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand(Procedure, con);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                foreach (String key in HT.Keys)
                {
                    cmd.Parameters.AddWithValue("@" + key, HT[key]);
                }
                await con.OpenAsync();
                int i = await cmd.ExecuteNonQueryAsync();

                con.Close();
                con.Dispose();
                cmd.Dispose();
                return i;
            }
            catch (Exception ex)
            {
                logger.Error("SP(Async)=" + Procedure + " Exception= " + ex.ToString());
                return 0;
            }
        }
        public Boolean ExecuteProcedure(String Procedure)
        {
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand(Procedure, con);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();

                con.Close();
                con.Dispose();
                cmd.Dispose();
                if (i > 0) return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error("SP=" + Procedure + " Exception= " + ex.ToString());
                return false;
            }
        }
        public string GetStringValueFromProcedure(string Procedure, Hashtable HT)
        {
            string key = null;
            string ReturnValue = "";
            string str_connection = Convert.ToString(obj_connectionstr.ConnectionString());
            SqlConnection con = new SqlConnection(str_connection);
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand(Procedure, con);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                foreach (string test in HT.Keys)
                {
                    key = test;
                    cmd.Parameters.AddWithValue("@" + key, HT[key]);
                }
                SqlParameter prm = new SqlParameter();
                prm.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(prm);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ReturnValue = prm.Value.ToString();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Dispose();
                cmd.Dispose();
            }
            return ReturnValue;
        }
    }
}
