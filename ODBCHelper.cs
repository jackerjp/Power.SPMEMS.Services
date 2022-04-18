using System;
using System.Data;
using System.Web.Services;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MySqlConnector;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Web;

namespace Power.SPMEMS.Services
{
    class ODBCHelper
    {
        /// <summary>
        /// MySQL数据库连接
        /// </summary>
        public class MySQLDataBase
        {
            private const int MaxPool = 30;//最大连接数
            private const int MinPool = 5;//最小连接数
            private const bool Asyn_Process = true;//设置异步访问数据库
                                                   //在单个连接上得到和管理多个、仅向前引用和只读的结果集(ADO.NET2.0)
            private const bool Mars = true;
            private const int Conn_Timeout = 10;//设置连接等待时间
            private const int Conn_Lifetime = 15;//设置连接的生命周期
            private MySqlConnection con = null;//连接对象
            private MySqlTransaction dbTran = null;
            public Dictionary<string, object> paramList = new Dictionary<string, object>();

            public MySQLDataBase()
            {
                //server=47.101.200.39;database=JJPowerPMDBTest;uid=sa;pwd=Power3506
                string sConnectionString = @"server=218.94.73.106;port=8306;user=JJSDBASE;password=JJSDBASE; database=jybim;";
                con = new MySqlConnection(sConnectionString);
            }

            public MySQLDataBase(string sLJCS)
            {
                string sConnectionString = sLJCS + ";";
                con = new MySqlConnection(sConnectionString);
            }

            public void beginTran()
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                dbTran = con.BeginTransaction();
            }

            public void commitTran()
            {
                try
                {
                    dbTran.Commit();
                }
                catch
                {
                    dbTran.Rollback();
                }
                finally
                {
                    con.Close();
                }
            }

            public void rollback()
            {
                dbTran.Rollback();
                con.Close();
            }

            public DataSet getDataSet(string sSQL)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                MySqlCommand cmd = new MySqlCommand(sSQL, con);
                cmd.Parameters.Clear();
                foreach (KeyValuePair<string, object> dic in paramList)
                    cmd.Parameters.AddWithValue(dic.Key, dic.Value);
                if (dbTran != null)
                    cmd.Transaction = dbTran;
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                clearParam();
                if (dbTran == null)
                    con.Close();
                return ds;
            }

            public void doSQL(string sSQL)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                MySqlCommand cmd = new MySqlCommand(sSQL, con);
                cmd.Parameters.Clear();
                foreach (KeyValuePair<string, object> dic in paramList)
                    cmd.Parameters.AddWithValue(dic.Key, dic.Value);
                if (dbTran != null)
                    cmd.Transaction = dbTran;
                cmd.ExecuteNonQuery();
                clearParam();
                if (dbTran == null)
                    con.Close();
            }

            public void addParam(string param, object value)
            {
                paramList.Add(param, value);
            }

            public void clearParam()
            {
                paramList.Clear();
            }
        }
        /// <summary>
        /// oracle数据库连接
        /// </summary>
        public class OraDataBase
        {
            private OracleConnection con = null;//连接对象
            private OracleTransaction dbTran = null;
            public Dictionary<string, object> paramList = new Dictionary<string, object>();

            public OraDataBase()
            {
                string connStr = "User Id=JJBIM;Password=JJBIM;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=222.190.136.142)(PORT=1221)))(CONNECT_DATA=(SERVICE_NAME=orcl)))";
                con = new OracleConnection(connStr);
            }

            public OraDataBase(string sLJCS)
            {
                string connStr = "User Id=JJBIM;Password=JJBIM;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=222.190.136.142)(PORT=1221)))(CONNECT_DATA=(SERVICE_NAME=orcl)))";
                con = new OracleConnection(connStr);
            }

            public void beginTran()
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                dbTran = con.BeginTransaction();
            }

            public void commitTran()
            {
                try
                {
                    dbTran.Commit();
                }
                catch
                {
                    dbTran.Rollback();
                }
                finally
                {
                    con.Close();
                }
            }

            public void rollback()
            {
                dbTran.Rollback();
                con.Close();
            }

            public DataSet getDataSet(string sSQL)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                OracleCommand cmd = new OracleCommand(sSQL, con);
                cmd.Parameters.Clear();
                foreach (KeyValuePair<string, object> dic in paramList)
                    cmd.Parameters.Add(dic.Key, dic.Value);
                if (dbTran != null)
                    cmd.Transaction = dbTran;
                OracleDataAdapter sda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                clearParam();
                if (dbTran == null)
                    con.Close();
                return ds;
            }

            public void doSQL(string sSQL)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                OracleCommand cmd = new OracleCommand(sSQL, con);
                cmd.Parameters.Clear();
                foreach (KeyValuePair<string, object> dic in paramList)
                    cmd.Parameters.Add(dic.Key, dic.Value);
                if (dbTran != null)
                    cmd.Transaction = dbTran;
                cmd.ExecuteNonQuery();
                clearParam();
                if (dbTran == null)
                    con.Close();
            }

            public void addParam(string param, object value)
            {
                paramList.Add(param, value);
            }

            public void clearParam()
            {
                paramList.Clear();
            }
        }
        /// <summary>
        /// sqlserver数据库连接
        /// </summary>
        public class SqlDataBase
        {
            private const int MaxPool = 512;//最大连接数
            private const int MinPool = 5;//最小连接数
            private const bool Asyn_Process = true;//设置异步访问数据库
                                                   //在单个连接上得到和管理多个、仅向前引用和只读的结果集(ADO.NET2.0)
            private const bool Mars = true;
            private const int Conn_Timeout = 10;//设置连接等待时间
            private const int Conn_Lifetime = 15;//设置连接的生命周期
            private SqlConnection con = null;//连接对象
            private SqlTransaction dbTran = null;
            public Dictionary<string, object> paramList = new Dictionary<string, object>();

            public SqlDataBase()
            {
                //server=47.101.200.39;database=JJPowerPMDBTest;uid=sa;pwd=Power3506
                string sConnectionString = @"server=47.101.200.39;database=JJPowerPMDBTest;uid=sa;pwd=Power3506;"
                                         + "Max Pool Size=" + MaxPool + ";"
                                         + "Min Pool Size=" + MinPool + ";"
                                         + "Connect Timeout=" + Conn_Timeout + ";"
                                         + "Connection Lifetime=" + Conn_Lifetime + ";"
                                         + "Asynchronous Processing=" + Asyn_Process + ";";
                con = new SqlConnection(sConnectionString);
            }

            public SqlDataBase(string sLJCS)
            {
                string sConnectionString = sLJCS + ";";
                sConnectionString += @"Max Pool Size=" + MaxPool + ";"
                                   + "Min Pool Size=" + MinPool + ";"
                                   + "Connect Timeout=" + Conn_Timeout + ";"
                                   + "Connection Lifetime=" + Conn_Lifetime + ";"
                                   + "Asynchronous Processing=" + Asyn_Process + ";";
                con = new SqlConnection(sConnectionString);
            }

            public void beginTran()
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                dbTran = con.BeginTransaction();
            }

            public void commitTran()
            {
                try
                {
                    dbTran.Commit();
                }
                catch
                {
                    dbTran.Rollback();
                }
                finally
                {
                    con.Close();
                }
            }

            public void rollback()
            {
                dbTran.Rollback();
                con.Close();
            }

            public DataSet getDataSet(string sSQL)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand(sSQL, con);
                cmd.Parameters.Clear();
                foreach (KeyValuePair<string, object> dic in paramList)
                    cmd.Parameters.AddWithValue(dic.Key, dic.Value);
                if (dbTran != null)
                    cmd.Transaction = dbTran;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                clearParam();
                if (dbTran == null)
                    if (dbTran == null)
                        con.Close();
                return ds;
            }

            public void doSQL(string sSQL)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand(sSQL, con);
                cmd.Parameters.Clear();
                foreach (KeyValuePair<string, object> dic in paramList)
                    cmd.Parameters.AddWithValue(dic.Key, dic.Value);
                if (dbTran != null)
                    cmd.Transaction = dbTran;
                cmd.ExecuteNonQuery();
                clearParam();
                if (dbTran == null)
                    con.Close();
            }

            public void addParam(string param, object value)
            {
                paramList.Add(param, value);
            }

            public void clearParam()
            {
                paramList.Clear();
            }
        }
    }
}
