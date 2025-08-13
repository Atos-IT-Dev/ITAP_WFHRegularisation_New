using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WFHRegularisationDAL;
using WFHRegularisationBLL;
using System.Configuration;
using System.Data;
using System.Drawing;
namespace WFHRegularisationUI
{
	public partial class TimeInTimeOutDetails : System.Web.UI.Page
	{
		#region Constants.

		private const string NOTIMEENTRYFOUND = "No time in time out entry found.";
		
		#endregion

		#region Events.

		protected void Page_Load(object sender, EventArgs e)
		{
			WFHRegularisationDAL.WFHRegularisation objWFHRegularisationBGSDAL = new WFHRegularisationDAL.WFHRegularisation();

			if (objWFHRegularisationBGSDAL.IsUserAssignedToBGS(HttpContext.Current.Session["DASID"].ToString()))
			{
				divMain.Visible = false;
                //lblAlertMessage.Text = ConfigurationManager.AppSettings["BenchShiftEmployeesMessage"].ToString();
                //lblAlertMessage.Type = BetiAlertMesageType.Error;
                //lblAlertMessage.Visible = true;
			}
			else
			// Added by Shardul S. Mahajan on 02-Apr-2024 ends here.
			{
				divMain.Visible = true;
				WFHRegularisationBLL.WFHRegularisation objWFHRegularisationBLL = new WFHRegularisationBLL.WFHRegularisation();
				WFHRegularisationDAL.WFHRegularisation objWFHRegularisationDAL = new WFHRegularisationDAL.WFHRegularisation();
				string strTodaysDate = DateTime.Today.ToString("dd-MM-yyyy");

				DataSet dsEmployeeDetails = objWFHRegularisationDAL.GetEmployeeDetails(strTodaysDate, HttpContext.Current.Session["DASID"].ToString());

				if (dsEmployeeDetails != null && dsEmployeeDetails.Tables[0] != null && dsEmployeeDetails.Tables[0].Rows.Count > 0)
				{
					bool blnCanAccess = objWFHRegularisationBLL.CanAccessTimeInTimeOut(dsEmployeeDetails.Tables[0].Rows[0]["Company"].ToString().Substring(0, 4));

					if (blnCanAccess)
					{
						divMain.Visible = true;
					}
					else
					{
                        //lblAlertMessage.Text = ConfigurationManager.AppSettings["NotAuthorisedToAccessThisPage"].ToString();
                        //lblAlertMessage.Type = BetiAlertMesageType.Error;
						divMain.Visible = false;
					}
				}
			}
		}

		protected void btnSearchTime_Click(object sender, EventArgs e)
		{
			getEmployeeTimeInTimeOutDetails(txtStartDate.Text,txtEndDate.Text);
		}

		protected void gvTimeInTimeOutDetails_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				DataRowView dr = (DataRowView)e.Row.DataItem;

				if (dr != null)
				{
					Label lblDASID = (Label)e.Row.FindControl("lblDASID");

					if (lblDASID != null)
						lblDASID.Text = dr["DASID"].ToString();

					Label lblEmployeeName = (Label)e.Row.FindControl("lblEmployeeName");

					if (lblEmployeeName != null)
						lblEmployeeName.Text = dr["EmployeeName"].ToString();

					Label lblSAPNumber = (Label)e.Row.FindControl("lblSAPNumber");

					if (lblSAPNumber != null)
						lblSAPNumber.Text = dr["SAPnumber"].ToString();

					Label lblServiceLine = (Label)e.Row.FindControl("lblServiceLine");

					if (lblServiceLine != null)
						lblServiceLine.Text = dr["ServiceLine"].ToString();

					Label lblOrgUnitnumber = (Label)e.Row.FindControl("lblOrgUnitnumber");

					if (lblOrgUnitnumber != null)
						lblOrgUnitnumber.Text = dr["OrgUnitnumber"].ToString();

					Label lblCompany = (Label)e.Row.FindControl("lblCompany");

					if (lblCompany != null)
						lblCompany.Text = dr["Company"].ToString();

					Label lblLocation = (Label)e.Row.FindControl("lblLocation");

					if (lblLocation != null)
						lblLocation.Text = dr["Location"].ToString();

					Label lblTimeIn = (Label)e.Row.FindControl("lblTimeIn");

					if (lblTimeIn != null)
						lblTimeIn.Text = dr["InTime"].ToString();

					Label lblTimeout = (Label)e.Row.FindControl("lblTimeout");

					if (lblTimeout != null)
						lblTimeout.Text = dr["OutTime"].ToString();

					Label lblTotalTime = (Label)e.Row.FindControl("lblTotalTime");

					if (lblTotalTime != null)
						lblTotalTime.Text = dr["TotalTime"].ToString();
				}
			}
		}

		protected void btnExportToExcel_Click(object sender, EventArgs e)
		{
			try
			{
				DataTable seatDisplayDataTable = getEmployeeTimeInTimeOutDetails(txtStartDate.Text, txtEndDate.Text);

				if (seatDisplayDataTable != null && seatDisplayDataTable.Rows.Count > 0)
				{
					HttpContext currentContext = HttpContext.Current;
					currentContext.Response.Clear();
					currentContext.Response.Charset = "";
					string attachment = "attachment; filename=TimeInTimeOutReport.xls";
					currentContext.Response.ClearContent();
					currentContext.Response.AddHeader("content-disposition", attachment);
					currentContext.Response.ContentType = "application/vnd.ms-excel";
					currentContext.Response.Write("<table border=1>");
					currentContext.Response.Write("<tr>");

					foreach (DataColumn field in seatDisplayDataTable.Columns)
					{
						currentContext.Response.Write("<td style='background-color:#d6e8ff'><b>");
						currentContext.Response.Write(field.ColumnName);
						currentContext.Response.Write("</b></td>");
					}
					currentContext.Response.Write("</tr>");

					foreach (DataRow dr in seatDisplayDataTable.Rows)
					{
						currentContext.Response.Write("<tr>");
						currentContext.Response.Write("<td>" + dr["DASID"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["SAPNumber"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["EmployeeName"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["GCMCode"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["OrgUnitNumber"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["OrgUnitManager"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["ServiceLine"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["Location"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["Company"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["IsInShift"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["InTime"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["OutTime"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["TotalTime"].ToString() + "</td>");
						currentContext.Response.Write("</tr>");
					}
					currentContext.Response.Write("</table>");
					currentContext.Response.End();
				}
			}

			catch (Exception ex)
			{
				string str = ex.Message;
			}
		}

		protected void gvTimeInTimeOutDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvTimeInTimeOutDetails.PageIndex = e.NewPageIndex;

			if (ViewState["dtTimeInTimOutDetails"] != null)
			{
				DataTable dtTimeInTimOutDetails = (DataTable)ViewState["dtTimeInTimOutDetails"];
				gvTimeInTimeOutDetails.DataSource = dtTimeInTimOutDetails;
				gvTimeInTimeOutDetails.DataBind();
			}
		}

		#endregion

		#region MyRegion

		private DataTable getEmployeeTimeInTimeOutDetails(string StartDate,string EndDate)
		{
			WFHRegularisationBLL.WFHRegularisation objWFHRegularisation = new WFHRegularisationBLL.WFHRegularisation();

			string strDateIn = txtStartDate.Text.Split('-').GetValue(0).ToString();
			//string strMonthIn = txtStartDate.Text.Split('-').GetValue(1).ToString();
			string strMonthIn = objWFHRegularisation.GetMonthNamefromMonthNumber(int.Parse(txtStartDate.Text.Split('-').GetValue(1).ToString()));
			string strYearIn = txtStartDate.Text.Split('-').GetValue(2).ToString();
			//string strDateTimeIn = strDateIn + "-" + strMonthIn + "-" + strYearIn;
			string strDateTimeIn = strDateIn + " " + strMonthIn + " " + strYearIn;

			string strDateOut = txtEndDate.Text.Split('-').GetValue(0).ToString();
			//string strMonthOut = txtEndDate.Text.Split('-').GetValue(1).ToString();
			string strMonthOut = objWFHRegularisation.GetMonthNamefromMonthNumber(int.Parse(txtEndDate.Text.Split('-').GetValue(1).ToString()));
			string strYearOut = txtEndDate.Text.Split('-').GetValue(2).ToString();
			//string strDateTimeOut = strDateOut + "-" + strMonthOut + "-" + strYearOut;
			string strDateTimeOut = strDateOut + " " + strMonthOut + " " + strYearOut;

			WFHRegularisationDAL.WFHRegularisation objWFHRegularisationDAL = new WFHRegularisationDAL.WFHRegularisation();

			DataTable dtTimeInTimOutDetails = objWFHRegularisationDAL.GetEmployeeTimeIntimeOutDetails(HttpContext.Current.Session["DASID"].ToString(),StartDate,EndDate);
			//DataTable dtTimeInTimOutDetails = objWFHRegularisationDAL.GetEmployeeTimeIntimeOutDetails(HttpContext.Current.Session["DASID"].ToString(), strDateTimeIn, strDateTimeOut);
			ViewState["dtTimeInTimOutDetails"] = dtTimeInTimOutDetails;

			if (dtTimeInTimOutDetails != null && dtTimeInTimOutDetails.Rows.Count > 0)
			{
				gvTimeInTimeOutDetails.Visible = true;
				gvTimeInTimeOutDetails.DataSource = dtTimeInTimOutDetails;
				gvTimeInTimeOutDetails.DataBind();
				ViewState["EmployeeTimeInTimeOut"] = dtTimeInTimOutDetails;
			}
			else
			{
				gvTimeInTimeOutDetails.Visible = false;
				ViewState["EmployeeTimeInTimeOut"] = null;
                //lblAlertMessage.Text = NOTIMEENTRYFOUND;
                //lblAlertMessage.Type = BetiAlertMesageType.Info;
			}
			return dtTimeInTimOutDetails;
		}

		#endregion

		#region "Archived Records Report: Added by Shraddha Pawar"

		private DataTable getEmployeeTimeInTimeOutDetailsArchived(string StartDate, string EndDate)
		{
			WFHRegularisationBLL.WFHRegularisation objWFHRegularisation = new WFHRegularisationBLL.WFHRegularisation();

			string strDateIn = txtStartDateArchived.Text.Split('-').GetValue(0).ToString();
			string strMonthIn = objWFHRegularisation.GetMonthNamefromMonthNumber(int.Parse(txtStartDateArchived.Text.Split('-').GetValue(1).ToString()));
			string strYearIn = txtStartDateArchived.Text.Split('-').GetValue(2).ToString();
			string strDateTimeIn = strDateIn + " " + strMonthIn + " " + strYearIn;

			string strDateOut = txtEndDateArchived.Text.Split('-').GetValue(0).ToString();
			string strMonthOut = objWFHRegularisation.GetMonthNamefromMonthNumber(int.Parse(txtEndDateArchived.Text.Split('-').GetValue(1).ToString()));
			string strYearOut = txtEndDateArchived.Text.Split('-').GetValue(2).ToString();
			string strDateTimeOut = strDateOut + " " + strMonthOut + " " + strYearOut;

			WFHRegularisationDAL.WFHRegularisation objWFHRegularisationDAL = new WFHRegularisationDAL.WFHRegularisation();

			DataTable dtTimeInTimOutDetailsArchived = objWFHRegularisationDAL.GetEmployeeTimeIntimeOutDetailsArchived(HttpContext.Current.Session["DASID"].ToString(), StartDate, EndDate);
			ViewState["dtTimeInTimOutDetailsArchived"] = dtTimeInTimOutDetailsArchived;

			if (dtTimeInTimOutDetailsArchived != null && dtTimeInTimOutDetailsArchived.Rows.Count > 0)
			{
				gvTimeInTimeOutDetailsArchived.Visible = true;
				gvTimeInTimeOutDetailsArchived.DataSource = dtTimeInTimOutDetailsArchived;
				gvTimeInTimeOutDetailsArchived.DataBind();
				ViewState["EmployeeTimeInTimeOutArchived"] = dtTimeInTimOutDetailsArchived;
			}
			else
			{
				gvTimeInTimeOutDetailsArchived.Visible = false;
				ViewState["EmployeeTimeInTimeOutArchived"] = null;
                //lblAlertMessage.Text = NOTIMEENTRYFOUND;
                //lblAlertMessage.Type = BetiAlertMesageType.Info;
			}
			return dtTimeInTimOutDetailsArchived;
		}

		protected void btnSearchTimeArchived_Click(object sender, EventArgs e)
		{
			getEmployeeTimeInTimeOutDetailsArchived(txtStartDateArchived.Text, txtEndDateArchived.Text);
		}

		protected void btnExportToExcelArchived_Click(object sender, EventArgs e)
		{
			try
			{
				DataTable seatDisplayDataTableArchived = getEmployeeTimeInTimeOutDetailsArchived(txtStartDateArchived.Text, txtEndDateArchived.Text);

				if (seatDisplayDataTableArchived != null && seatDisplayDataTableArchived.Rows.Count > 0)
				{
					HttpContext currentContext = HttpContext.Current;
					currentContext.Response.Clear();
					currentContext.Response.Charset = "";
					string attachment = "attachment; filename=TimeInTimeOutReport.xls";
					currentContext.Response.ClearContent();
					currentContext.Response.AddHeader("content-disposition", attachment);
					currentContext.Response.ContentType = "application/vnd.ms-excel";
					currentContext.Response.Write("<table border=1>");
					currentContext.Response.Write("<tr>");

					foreach (DataColumn field in seatDisplayDataTableArchived.Columns)
					{
						currentContext.Response.Write("<td style='background-color:#d6e8ff'><b>");
						currentContext.Response.Write(field.ColumnName);
						currentContext.Response.Write("</b></td>");
					}
					currentContext.Response.Write("</tr>");

					foreach (DataRow dr in seatDisplayDataTableArchived.Rows)
					{
						currentContext.Response.Write("<tr>");
						currentContext.Response.Write("<td>" + dr["DASID"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["SAPNumber"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["EmployeeName"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["GCMCode"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["OrgUnitNumber"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["OrgUnitManager"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["ServiceLine"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["Location"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["Company"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["IsInShift"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["InTime"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["OutTime"].ToString() + "</td>");
						currentContext.Response.Write("<td>" + dr["TotalTime"].ToString() + "</td>");
						currentContext.Response.Write("</tr>");
					}
					currentContext.Response.Write("</table>");
					currentContext.Response.End();
				}
			}

			catch (Exception ex)
			{
				string str = ex.Message;
			}
		}

		protected void gvTimeInTimeOutDetailsArchived_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvTimeInTimeOutDetailsArchived.PageIndex = e.NewPageIndex;

			if (ViewState["dtTimeInTimOutDetailsArchived"] != null)
			{
				DataTable dtTimeInTimOutDetailsArchived = (DataTable)ViewState["dtTimeInTimOutDetailsArchived"];
				gvTimeInTimeOutDetailsArchived.DataSource = dtTimeInTimOutDetailsArchived;
				gvTimeInTimeOutDetailsArchived.DataBind();
			}
		}

		protected void gvTimeInTimeOutDetailsArchived_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				DataRowView dr = (DataRowView)e.Row.DataItem;

				if (dr != null)
				{
					Label lblDASID = (Label)e.Row.FindControl("lblDASIDArchived");

					if (lblDASID != null)
						lblDASID.Text = dr["DASID"].ToString();

					Label lblEmployeeName = (Label)e.Row.FindControl("lblEmployeeNameArchived");

					if (lblEmployeeName != null)
						lblEmployeeName.Text = dr["EmployeeName"].ToString();

					Label lblSAPNumber = (Label)e.Row.FindControl("lblSAPNumberArchived");

					if (lblSAPNumber != null)
						lblSAPNumber.Text = dr["SAPnumber"].ToString();

					Label lblServiceLine = (Label)e.Row.FindControl("lblServiceLineArchived");

					if (lblServiceLine != null)
						lblServiceLine.Text = dr["ServiceLine"].ToString();

					Label lblOrgUnitnumber = (Label)e.Row.FindControl("lblOrgUnitnumberArchived");

					if (lblOrgUnitnumber != null)
						lblOrgUnitnumber.Text = dr["OrgUnitnumber"].ToString();

					Label lblCompany = (Label)e.Row.FindControl("lblCompanyArchived");

					if (lblCompany != null)
						lblCompany.Text = dr["Company"].ToString();

					Label lblLocation = (Label)e.Row.FindControl("lblLocationArchived");

					if (lblLocation != null)
						lblLocation.Text = dr["Location"].ToString();

					Label lblTimeIn = (Label)e.Row.FindControl("lblTimeInArchived");

					if (lblTimeIn != null)
						lblTimeIn.Text = dr["InTime"].ToString();

					Label lblTimeout = (Label)e.Row.FindControl("lblTimeoutArchived");

					if (lblTimeout != null)
						lblTimeout.Text = dr["OutTime"].ToString();

					Label lblTotalTime = (Label)e.Row.FindControl("lblTotalTimeArchived");

					if (lblTotalTime != null)
						lblTotalTime.Text = dr["TotalTime"].ToString();
				}
			}
		}

		#endregion

	}
}