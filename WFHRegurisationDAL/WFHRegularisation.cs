using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using WFHRegularisationDAL;

namespace WFHRegularisationDAL
{
	public class WFHRegularisation
	{
		#region Constants.
		private const string GET_EMPLOYEE_DETAILS = "usp_GET_Employee_Details";
		private const string INSERT_EMPLOYEE_TIMEIN_TIMEOUT = "usp_INSERT_Employee_TimeIn_TimeOut";
		//private const string GET_EMPLOYEE_TIMEIN_TIMEOUT_DETAILS = "usp_GET_Employee_TimeIn_TimeOut_Details";
		private const string GET_EMPLOYEE_TIMEIN_TIMEOUT_DETAILS = "usp_GET_Employee_TimeIn_TimeOut_Details";
        private const string GET_EMPLOYEE_TIMEIN_TIMEOUT_DETAILS_ARCHIVED = "usp_GET_Employee_TimeIn_TimeOut_Details_Archived";
		private const string GET_TIMEIN_TIMEOUT_FOR_LOGGED_IN_EMPLOYEE = "usp_Get_TimeIn_TimeOut_For_Logged_In_Employee";
		private const string CHECK_BENCH_GENERAL_SHIFT_ALLOCATION = "usp_CHECK_BENCH_GENERAL_SHIFT_ALLOCATION";
		#endregion

		#region Methods.
		/// <summary>
		/// Get employee details.
		/// </summary>
		/// <returns>Datatable.</returns>
		public DataSet GetEmployeeDetails(string TodaysDate, string DASID)
		{
			string date = TodaysDate.Split('-').GetValue(0).ToString();
			string month = TodaysDate.Split('-').GetValue(1).ToString();
			string year = TodaysDate.Split('-').GetValue(2).ToString();

			DataConnection objDataConnection = new DataConnection(false);
			SqlCommand objSQLCommand = new SqlCommand();

			SqlParameter[] objSQLParameterItems = new SqlParameter[2];
			objSQLParameterItems[0] = new SqlParameter();
			objSQLParameterItems[0].Direction = ParameterDirection.Input;
			objSQLParameterItems[0].ParameterName = "@TodaysDate";
			objSQLParameterItems[0].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[0].SqlValue = (date + "-" + month + "-" + year);

			objSQLParameterItems[1] = new SqlParameter();
			objSQLParameterItems[1].Direction = ParameterDirection.Input;
			objSQLParameterItems[1].ParameterName = "@DASID";
			objSQLParameterItems[1].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[1].SqlValue = DASID;

			objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[1]);
			objSQLCommand.CommandText = GET_EMPLOYEE_DETAILS;

			DataSet dsGetEmployeeDetails = objDataConnection.GetDataSetFromParameterisedStoredProcedure(objSQLCommand);
			objDataConnection.closeConnection();
			return dsGetEmployeeDetails;
		}

		/// <summary>
		/// Saves employee time in / time out.
		/// </summary>
		/// <param name="objWFHRegularisationBLL"></param>
		/// <returns></returns>
		public int InsertEmployeeTimeInTimeOut(WFHRegularisationBLL.WFHRegularisation objWFHRegularisationBLL)
		{
			string date = objWFHRegularisationBLL.RegularisationDate.ToString().Split('-').GetValue(0).ToString();
			string month = objWFHRegularisationBLL.RegularisationDate.ToString().Split('-').GetValue(1).ToString();
			string year = objWFHRegularisationBLL.RegularisationDate.ToString().Split('-').GetValue(2).ToString();

			DataConnection objDataConnection = new DataConnection(false);
			SqlCommand objSQLCommand = new SqlCommand();

			SqlParameter[] objSQLParameterItems = new SqlParameter[13];

			objSQLParameterItems[0] = new SqlParameter();
			objSQLParameterItems[0].Direction = ParameterDirection.Input;
			objSQLParameterItems[0].ParameterName = "@ServiceLine";
			objSQLParameterItems[0].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[0].SqlValue = objWFHRegularisationBLL.ServiceLine;

			objSQLParameterItems[1] = new SqlParameter();
			objSQLParameterItems[1].Direction = ParameterDirection.Input;
			objSQLParameterItems[1].ParameterName = "@CompanyCode";
			objSQLParameterItems[1].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[1].SqlValue = objWFHRegularisationBLL.Company;

			objSQLParameterItems[2] = new SqlParameter();
			objSQLParameterItems[2].Direction = ParameterDirection.Input;
			objSQLParameterItems[2].ParameterName = "@DASID";
			objSQLParameterItems[2].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[2].SqlValue = objWFHRegularisationBLL.DASID;

			objSQLParameterItems[3] = new SqlParameter();
			objSQLParameterItems[3].Direction = ParameterDirection.Input;
			objSQLParameterItems[3].ParameterName = "@SAPNumber";
			objSQLParameterItems[3].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[3].SqlValue = objWFHRegularisationBLL.SAPNumber;

			objSQLParameterItems[4] = new SqlParameter();
			objSQLParameterItems[4].Direction = ParameterDirection.Input;
			objSQLParameterItems[4].ParameterName = "@Location";
			objSQLParameterItems[4].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[4].SqlValue = objWFHRegularisationBLL.Location;

			objSQLParameterItems[5] = new SqlParameter();
			objSQLParameterItems[5].Direction = ParameterDirection.Input;
			objSQLParameterItems[5].ParameterName = "@OrgUnit";
			objSQLParameterItems[5].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[5].SqlValue = objWFHRegularisationBLL.OrgUnit;

			objSQLParameterItems[6] = new SqlParameter();
			objSQLParameterItems[6].Direction = ParameterDirection.Input;
			objSQLParameterItems[6].ParameterName = "@RegularisationDate";
			objSQLParameterItems[6].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[6].SqlValue = (date + "-" + month + "-" + year);

			if (objWFHRegularisationBLL.TimeInTimeOutType.ToUpper() == "I")
			{
				objSQLParameterItems[7] = new SqlParameter();
				objSQLParameterItems[7].Direction = ParameterDirection.Input;
				objSQLParameterItems[7].ParameterName = "@TimeInOut";
				objSQLParameterItems[7].SqlDbType = SqlDbType.Time;
				objSQLParameterItems[7].SqlValue = objWFHRegularisationBLL.TimeIn;
			}
			else
			{
				objSQLParameterItems[7] = new SqlParameter();
				objSQLParameterItems[7].Direction = ParameterDirection.Input;
				objSQLParameterItems[7].ParameterName = "@TimeInOut";
				objSQLParameterItems[7].SqlDbType = SqlDbType.Time;
				objSQLParameterItems[7].SqlValue = objWFHRegularisationBLL.TimeOut;
			}

			objSQLParameterItems[8] = new SqlParameter();
			objSQLParameterItems[8].Direction = ParameterDirection.Input;
			//objSQLParameterItems[8].ParameterName = "@TimeOut";
			objSQLParameterItems[8].ParameterName = "@TimeInOutType";
			objSQLParameterItems[8].SqlDbType = SqlDbType.NVarChar;
			//objSQLParameterItems[8].SqlValue = objWFHRegularisationBLL.TimeOut;
			objSQLParameterItems[8].SqlValue = objWFHRegularisationBLL.TimeInTimeOutType;

			objSQLParameterItems[9] = new SqlParameter();
			objSQLParameterItems[9].Direction = ParameterDirection.Input;
			objSQLParameterItems[9].ParameterName = "@CreatedBy";
			objSQLParameterItems[9].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[9].SqlValue = HttpContext.Current.Session["DASID"].ToString();

			objSQLParameterItems[10] = new SqlParameter();
			objSQLParameterItems[10].Direction = ParameterDirection.Input;
			objSQLParameterItems[10].ParameterName = "@WFMEmployeeTimeInTimeOutID";
			objSQLParameterItems[10].SqlDbType = SqlDbType.Int;
			objSQLParameterItems[10].SqlValue = objWFHRegularisationBLL.WFMEmployeeTimeInTimeOutID;

			objSQLParameterItems[11] = new SqlParameter();
			objSQLParameterItems[11].Direction = ParameterDirection.Input;
			objSQLParameterItems[11].ParameterName = "@IsInShift";
			objSQLParameterItems[11].SqlDbType = SqlDbType.Bit;
			objSQLParameterItems[11].SqlValue = objWFHRegularisationBLL.IsInShift;

			objSQLParameterItems[12] = new SqlParameter();
			objSQLParameterItems[12].Direction = ParameterDirection.Input;
			objSQLParameterItems[12].ParameterName = "@IsTimeEntryComplete";
			objSQLParameterItems[12].SqlDbType = SqlDbType.Bit;
			objSQLParameterItems[12].SqlValue = objWFHRegularisationBLL.IsTimeEntryComplete;

			objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[1]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[2]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[3]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[4]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[5]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[6]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[7]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[8]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[9]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[10]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[11]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[12]);

			objSQLCommand.CommandText = INSERT_EMPLOYEE_TIMEIN_TIMEOUT;

			int intCount = objDataConnection.SaveDataFromStoredProcedure(objSQLCommand);
			objDataConnection.closeConnection();
			return intCount;
		}

		public DataTable GetEmployeeTimeIntimeOutDetails(string DASID, string StartDate, string EndDate)
		{
			DataConnection objDataConnection = new DataConnection(false);
			SqlCommand objSQLCommand = new SqlCommand();
			objSQLCommand.CommandTimeout = 7200;

			SqlParameter[] objSQLParameterItems = new SqlParameter[3];
			objSQLParameterItems[0] = new SqlParameter();
			objSQLParameterItems[0].Direction = ParameterDirection.Input;
			objSQLParameterItems[0].ParameterName = "@FromDate";
			objSQLParameterItems[0].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[0].SqlValue = StartDate;

			objSQLParameterItems[1] = new SqlParameter();
			objSQLParameterItems[1].Direction = ParameterDirection.Input;
			objSQLParameterItems[1].ParameterName = "@ToDate";
			objSQLParameterItems[1].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[1].SqlValue = EndDate;

			objSQLParameterItems[2] = new SqlParameter();
			objSQLParameterItems[2].Direction = ParameterDirection.Input;
			objSQLParameterItems[2].ParameterName = "@DASID";
			objSQLParameterItems[2].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[2].SqlValue = DASID;

			objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[1]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[2]);
			objSQLCommand.CommandText = GET_EMPLOYEE_TIMEIN_TIMEOUT_DETAILS;

			DataTable dtGetEmployeeDetails = objDataConnection.GetDataTableFromParameterisedStoredProcedure(objSQLCommand);
			objDataConnection.closeConnection();
			return dtGetEmployeeDetails;
		}

        //Added by Shraddha: GetEmployeeTimeIntimeOutDetailsArchived for Archived Details
        public DataTable GetEmployeeTimeIntimeOutDetailsArchived(string DASID, string StartDate, string EndDate)
        {
            DataConnection objDataConnection = new DataConnection(false);
            SqlCommand objSQLCommand = new SqlCommand();
            objSQLCommand.CommandTimeout = 7200;

            SqlParameter[] objSQLParameterItems = new SqlParameter[3];
            objSQLParameterItems[0] = new SqlParameter();
            objSQLParameterItems[0].Direction = ParameterDirection.Input;
            objSQLParameterItems[0].ParameterName = "@FromDate";
            objSQLParameterItems[0].SqlDbType = SqlDbType.NVarChar;
            objSQLParameterItems[0].SqlValue = StartDate;

            objSQLParameterItems[1] = new SqlParameter();
            objSQLParameterItems[1].Direction = ParameterDirection.Input;
            objSQLParameterItems[1].ParameterName = "@ToDate";
            objSQLParameterItems[1].SqlDbType = SqlDbType.NVarChar;
            objSQLParameterItems[1].SqlValue = EndDate;

            objSQLParameterItems[2] = new SqlParameter();
            objSQLParameterItems[2].Direction = ParameterDirection.Input;
            objSQLParameterItems[2].ParameterName = "@DASID";
            objSQLParameterItems[2].SqlDbType = SqlDbType.NVarChar;
            objSQLParameterItems[2].SqlValue = DASID;

            objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
            objSQLCommand.Parameters.Add(objSQLParameterItems[1]);
            objSQLCommand.Parameters.Add(objSQLParameterItems[2]);
            objSQLCommand.CommandText = GET_EMPLOYEE_TIMEIN_TIMEOUT_DETAILS_ARCHIVED;

            DataTable dtGetEmployeeDetails = objDataConnection.GetDataTableFromParameterisedStoredProcedure(objSQLCommand);
            objDataConnection.closeConnection();
            return dtGetEmployeeDetails;
        }

		public DataTable GetEmployeeTimeIntimeOutDetails(string StartDate)
		{
			DataConnection objDataConnection = new DataConnection(false);
			SqlCommand objSQLCommand = new SqlCommand();

			SqlParameter[] objSQLParameterItems = new SqlParameter[2];
			objSQLParameterItems[0] = new SqlParameter();
			objSQLParameterItems[0].Direction = ParameterDirection.Input;
			objSQLParameterItems[0].ParameterName = "@FromDate";
			objSQLParameterItems[0].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[0].SqlValue = StartDate;

			objSQLParameterItems[1] = new SqlParameter();
			objSQLParameterItems[1].Direction = ParameterDirection.Input;
			objSQLParameterItems[1].ParameterName = "@DASID";
			objSQLParameterItems[1].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[1].SqlValue = HttpContext.Current.Session["DASID"].ToString();

			objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
			objSQLCommand.Parameters.Add(objSQLParameterItems[1]);
			objSQLCommand.CommandText = GET_TIMEIN_TIMEOUT_FOR_LOGGED_IN_EMPLOYEE;

			DataTable dtGetEmployeeDetails = objDataConnection.GetDataTableFromParameterisedStoredProcedure(objSQLCommand);
			objDataConnection.closeConnection();
			return dtGetEmployeeDetails;
		}

		public bool IsUserAssignedToBGS(string DASID)
		{
			bool blnIsUserAssignedToBGS = false;

			DataConnection objDataConnection = new DataConnection(false);
			SqlCommand objSQLCommand = new SqlCommand();

			SqlParameter[] objSQLParameterItems = new SqlParameter[1];
			objSQLParameterItems[0] = new SqlParameter();
			objSQLParameterItems[0].Direction = ParameterDirection.Input;
			objSQLParameterItems[0].ParameterName = "@DASID";
			objSQLParameterItems[0].SqlDbType = SqlDbType.NVarChar;
			objSQLParameterItems[0].SqlValue = DASID;

			objSQLCommand.Parameters.Add(objSQLParameterItems[0]);
			objSQLCommand.CommandText = CHECK_BENCH_GENERAL_SHIFT_ALLOCATION;

			DataTable dtIsUserAssignedToBGS = objDataConnection.GetDataTableFromParameterisedStoredProcedure(objSQLCommand);
			objDataConnection.closeConnection();

			if (dtIsUserAssignedToBGS != null && dtIsUserAssignedToBGS.Rows.Count > 0)
				blnIsUserAssignedToBGS = true;
			return blnIsUserAssignedToBGS;
		}
		#endregion
	}
}
