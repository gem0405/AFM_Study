using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace AFM_Imput
{
    public class ADO
    {
        public SqlConnection conn;
        public SqlCommand cmd;

        public ADO()
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = conn;
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }
        public ADO(string contxt)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[contxt].ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = conn;
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }

        public DataSet toDataSet(string cmtxt)
        {
            try
            {
                conn.Open();
                //cmd.CommandText = cmtxt;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmtxt, conn);
                da.Fill(ds);

                return ds;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public SqlDataReader toDataReader(string cmtxt)
        {
            conn.Open();
            cmd.CommandText = cmtxt;
            SqlDataReader dr = cmd.ExecuteReader();
            //conn.Close();
            return dr;

        }

        public int DbNonQuery(string cmtxt)
        {
            try
            {
                conn.Open();
                cmd.CommandText = cmtxt;

                var count = cmd.ExecuteNonQuery();
                conn.Close();
                cmd.Dispose();
                return count;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
    }