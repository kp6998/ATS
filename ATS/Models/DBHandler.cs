using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace ATS.Models
{
    public class DBHandler
    {
        public static string Connection(string strType)
        {
            string strtype = string.Empty;
            string strConnectionString = string.Empty;
            switch (strType)
            {
                case "A":
                    strConnectionString = ConfigurationManager.AppSettings["ApplicationConnectionString"].ToString() != null ? ConfigurationManager.AppSettings["ApplicationConnectionString"].ToString() : "";
                    break;
                case "L":
                    strConnectionString = ConfigurationManager.AppSettings["LogConnectionString"].ToString() != null ? ConfigurationManager.AppSettings["LogConnectionString"].ToString() : "";
                    break;
            }
            return strConnectionString;
        }
        public static DataSet ExecProcedureReturnsDataset(string ProcedureName, Hashtable Parameters, ref string strErrorMsg)
        {
            SqlConnection my_connection = null;
            DataSet my_dataset = new DataSet();
            string my_procedure = ProcedureName;
            try
            {
                my_connection = new SqlConnection(Connection("A"));
                SqlCommand my_command = my_connection.CreateCommand();
                my_command.CommandText = ProcedureName;
                my_command.CommandType = CommandType.StoredProcedure;
                my_command.CommandTimeout = 0;
                AssignParameters(my_command, Parameters);
                SqlDataAdapter my_adapter = new SqlDataAdapter();
                my_adapter.SelectCommand = my_command;
                my_adapter.Fill(my_dataset, "Temp");
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
            finally
            {
                if (my_connection != null)
                {
                    my_connection.Close();
                }
            }
            return (my_dataset);
        }
        public static bool InsertUpdateDelete(string ProcedureName, Hashtable Parameters, ref string strErrorMsg, ref int ResultCount)
        {
            SqlConnection my_connection = null;
            try
            {
                my_connection = new SqlConnection(Connection("A"));
                SqlCommand my_command = my_connection.CreateCommand();
                my_command.CommandText = ProcedureName;
                my_command.CommandType = CommandType.StoredProcedure;
                my_command.CommandTimeout = 0;
                AssignParameters(my_command, Parameters, ResultCount);
                my_connection.Open();
                ResultCount = my_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ResultCount = 0;
                strErrorMsg = ex.Message.ToString();
                return false;
            }
            finally
            {
                if (my_connection != null)
                {
                    my_connection.Close();
                }
            }
            return true;
        }
        public static void AssignParameters(SqlCommand Command, Hashtable Parameters)
        {
            if (Parameters == null) return;
            foreach (object key in Parameters.Keys)
            {
                Command.Parameters.Add(("@" + key.ToString().ToUpper()), Parameters[key]);
            }
        }
        public static void AssignParameters(SqlCommand Command, Hashtable Parameters, int ResultCount)
        {
            if (Parameters == null) return;
            foreach (object key in Parameters.Keys)
            {
                if (Parameters[key] != "CV_BYTES")
                {
                    Command.Parameters.Add(("@" + key.ToString().ToUpper()), Parameters[key]);
                }
                else
                {
                    Command.Parameters.Add(("@" + key.ToString().ToUpper()), SqlDbType.VarBinary, ResultCount);
                }
            }
        }
        public class StoreProcedure
        {
            public static string CANDIDATES_DETAILS_SERVICE = "CANDIDATES_DETAILS_SERVICE";
            public static string CHECK_DUPLICATE_SERVICE = "CHECK_DUPLICATE_SERVICE";
            public static string CANDIDATES_LIST = "CANDIDATES_LIST";
            public static string UPDATE_STATUS = "UPDATE_STATUS";
        }
    }
}