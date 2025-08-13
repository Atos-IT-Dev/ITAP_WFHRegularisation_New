using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace WFHRegularisationDAL
{
    public static class EmployeeData
    {
        public static string GetAppId(int ScreenId)
        {
            DataConnection objDataConnection = new DataConnection(true);
            SqlCommand objSQLCommand = new SqlCommand();
            SqlParameter[] objSQLParameterItems = new SqlParameter[1];
            objSQLParameterItems[0] = new SqlParameter();
            objSQLParameterItems[0].Direction = ParameterDirection.Input;
            objSQLParameterItems[0].ParameterName = "@BETIScreenID";
            objSQLParameterItems[0].SqlDbType = SqlDbType.Int;
            objSQLParameterItems[0].SqlValue = ScreenId;
            objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
            objSQLCommand.CommandText = "USP_GetAppId";
            DataTable dt = objDataConnection.GetDataTableFromParameterisedStoredProcedure(objSQLCommand);
            objDataConnection.closeConnection();
            string AppID = "";
            if (dt.Rows.Count > 0)
            {
                AppID = dt.Rows[0][0].ToString();
            }
            return AppID;
        }

        public static void SetEmpoyeeSession( )
        {
            string dasid = HttpContext.Current.Session["DASID"].ToString().ToUpper();
            string AppId = ConfigurationManager.AppSettings["APP_ID"].ToString();
            DataConnection objDataConnection = new DataConnection(true);
            SqlCommand objSQLCommand = new SqlCommand();
            SqlParameter[] objSQLParameterItems = new SqlParameter[3];
            //DAS ID
            objSQLParameterItems[0] = new SqlParameter();
            objSQLParameterItems[0].Direction = ParameterDirection.Input;
            objSQLParameterItems[0].ParameterName = "@IN_DASID";
            objSQLParameterItems[0].SqlDbType = SqlDbType.VarChar;
            objSQLParameterItems[0].SqlValue = dasid;

            //IN_APP ID
            objSQLParameterItems[1] = new SqlParameter();
            objSQLParameterItems[1].Direction = ParameterDirection.Input;
            objSQLParameterItems[1].ParameterName = "@IN_APPID";
            objSQLParameterItems[1].SqlDbType = SqlDbType.VarChar;
            objSQLParameterItems[1].SqlValue = AppId;

            //IN_PAGENAME
            objSQLParameterItems[2] = new SqlParameter();
            objSQLParameterItems[2].Direction = ParameterDirection.Input;
            objSQLParameterItems[2].ParameterName = "@IN_PAGENAME";
            objSQLParameterItems[2].SqlDbType = SqlDbType.VarChar;
            objSQLParameterItems[2].SqlValue = "";

            objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
            objSQLCommand.Parameters.Add(objSQLParameterItems[1]);
            objSQLCommand.Parameters.Add(objSQLParameterItems[2]);
            objSQLCommand.CommandText = "BETI_SP_GETROLE";
            DataSet ds = objDataConnection.GetDataSetFromParameterisedStoredProcedure(objSQLCommand);
            objDataConnection.closeConnection();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session.Add("USER_ROLES", ds.Tables[0].Rows[0][0]);
            }

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                HttpContext.Current.Session.Add("APP_VERSION", ds.Tables[1].Rows[0][0]);
                HttpContext.Current.Session.Add("APP_TITLE", ds.Tables[1].Rows[0][1]);
            }
            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                HttpContext.Current.Session.Add("APP_NOTE", ds.Tables[2].Rows[0][0]);
            }

            if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
            {
                HttpContext.Current.Session.Add("SERVER_TYPE", ds.Tables[3].Rows[0][0]);
            }
        }

        public static Boolean CheckEmpExistsInZTEmpMas(string dasid)
        {
            DataConnection objDataConnection = new DataConnection(true);
            SqlCommand objSQLCommand = new SqlCommand();
            SqlParameter[] objSQLParameterItems = new SqlParameter[3];
            //DAS ID
            objSQLParameterItems[0] = new SqlParameter();
            objSQLParameterItems[0].Direction = ParameterDirection.Input;
            objSQLParameterItems[0].ParameterName = "@IN_DASID";
            objSQLParameterItems[0].SqlDbType = SqlDbType.VarChar;
            objSQLParameterItems[0].SqlValue = dasid;
            objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
            objSQLCommand.CommandText = "SP_IS_EXISTS_ZT_EMP_MAS";
            DataTable dt = objDataConnection.GetDataTableFromParameterisedStoredProcedure(objSQLCommand);
            objDataConnection.closeConnection();
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;

        }
    }
}