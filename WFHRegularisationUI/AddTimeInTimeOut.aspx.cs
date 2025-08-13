using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WFHRegularisationUI
{
    public partial class AddTimeInTimeOut : System.Web.UI.Page
    {
        #region Constants.
        private const string SUCCESSFULLYINSERTEDTIMEINTIMEOUT = "Successfully updated the time. Please close the browser / tab.";
        private const string FAILEDTOADDTIMEINTIMEOUT = "Failed to add time.";
        private const string NOTIMEINTIMEOUTDATA = "No time in / time out data. Please enter time in 24 Hours format.";
        private const string INVALIDTIMEINTIMEOUT = "Invalid In Time / Out Time. Please enter time in 24 Hours format.";
        private const string TIMEINFOUNDTIMEOUTNOTFOUND = "Please enter out time first and then click on date change";
        private const string INOUTTIMEALREADYEXISTS = "In Time / Out Time already recorded.";
        private const string TIMEIN = "TimeIn";
        private const string TIMEOUT = "Timeout";
        #endregion

        #region Events.
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["USER_ROLES"] == null)
            //	BETIDataObject.GetDASID_Details(HttpContext.Current.Session["DASID"].ToString(), ConfigurationManager.AppSettings["APP_ID"].ToString());
            // Added by Shardul S. Mahajan on 02-Apr-2024 starts here.
            // Check if the logged in user is assigned to Bench General Shift.

            WFHRegularisationDAL.WFHRegularisation objWFHRegularisationDAL = new WFHRegularisationDAL.WFHRegularisation();

            if (objWFHRegularisationDAL.IsUserAssignedToBGS(HttpContext.Current.Session["DASID"].ToString()))
            {
                divMain.Visible = false;

                string Message = ConfigurationManager.AppSettings["BenchShiftEmployeesMessage"].ToString();
                ShowAlert("Error!", Message, "danger");
                //lblAlertMessage.Text = ConfigurationManager.AppSettings["BenchShiftEmployeesMessage"].ToString();
                //lblAlertMessage.Type = BetiAlertMesageType.Error;
                //lblAlertMessage.Visible = true;
            }
            else
            // Added by Shardul S. Mahajan on 02-Apr-2024 ends here.
            {
                divMain.Visible = true;
                RemoveClass(pageAlert, "show");
                //lblAlertMessage.Visible = false;

                if (!Page.IsPostBack)
                {
                    txtTimeInDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                    getEmployeeDetails(txtTimeInDate.Text);
                }
            }
        }



        protected void btnSaveTimeIn_Click(object sender, EventArgs e)
        {
            btnSaveTimeIn.CausesValidation = false;
            btnSaveTimeIn.Enabled = false;
            saveTimeRegularisation(TIMEIN);
        }

        protected void btnSaveTimeOut_Click(object sender, EventArgs e)
        {
            btnSaveTimeOut.Enabled = false;
            saveTimeRegularisation(TIMEOUT);
        }

        protected void chkIsInShift_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsInShift.Checked)
            {
                getEmployeeDetails(DateTime.Today.ToString("dd-MM-yyyy"));
            }
            else
            {
                if ((txtTimeInDate.Text.Length > 0) && (txtTimeInDate.Text != txtTimeOutDate.Text))
                {
                    lblIsInShift.Visible = true;
                    lblNotInShift.Visible = false;
                }
                else
                {
                    lblIsInShift.Visible = false;
                    lblNotInShift.Visible = true;
                }
                txtTimeInDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                txtTimeIn.Text = DateTime.Now.ToString("HH:mm");
                txtTimeIn.Enabled = txtTimeInDate.Enabled = false;

                btnSaveTimeIn.Enabled = true;

                txtTimeOutDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                txtTimeOut.Text = DateTime.Now.ToString("HH:mm");
                txtTimeOut.Enabled = txtTimeInDate.Enabled = false;

                btnSaveTimeOut.Enabled = true;
            }
        }

        protected void btnViewTimeInTimeOutDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("TimeInTimeOutDetails.aspx");
        }
        #endregion

        #region Methods.
        private bool isDataValid()
        {

            //if (txtTimeOut.Text.Length > 0)
            //{
            //	if (int.Parse(txtTimeIn.Text.Substring(0, 2)) > int.Parse(txtTimeOut.Text.Substring(0, 2)))
            //		blnIsDataValid = false;
            //	else if (int.Parse(txtTimeIn.Text.Substring(0, 2)) == int.Parse(txtTimeOut.Text.Substring(0, 2)))
            //	{
            //		if (int.Parse(txtTimeIn.Text.Substring(3, 2)) > int.Parse(txtTimeOut.Text.Substring(3, 2)))
            //			blnIsDataValid = false;
            //	}
            //}
            //else if (txtTimeOut.Text.Length == 0 && int.Parse(lblWFHEmployeeTimeID.Text) > 0)
            //{
            //	btnSaveTimeIn.CausesValidation = true;
            //	blnIsDataValid = false;
            //}
            return true;
        }

        private void getEmployeeDetails(string TodayDate)
        {
            WFHRegularisationBLL.WFHRegularisation objWFHRegularisationBLL = new WFHRegularisationBLL.WFHRegularisation();
            WFHRegularisationDAL.WFHRegularisation objWFHRegularisationDAL = new WFHRegularisationDAL.WFHRegularisation();

            DataSet dsEmployeeDetails = objWFHRegularisationDAL.GetEmployeeDetails(TodayDate, HttpContext.Current.Session["DASID"].ToString());

            /*
				Table[0] - Employee Details
				Table[1] - Time-In Details
				Table[2] - Time-Out Details
			*/
            if (dsEmployeeDetails != null && dsEmployeeDetails.Tables[0] != null && dsEmployeeDetails.Tables[0].Rows.Count > 0)
            {
                bool blnCanAccess = objWFHRegularisationBLL.CanAccessTimeInTimeOut(dsEmployeeDetails.Tables[0].Rows[0]["Company"].ToString().Substring(0, 4));

                if (blnCanAccess)
                {
                    txtName.Text = dsEmployeeDetails.Tables[0].Rows[0]["Name"].ToString();
                    txtCompany.Text = dsEmployeeDetails.Tables[0].Rows[0]["Company"].ToString();
                    txtDASID.Text = dsEmployeeDetails.Tables[0].Rows[0]["DASID"].ToString();
                    txtSAPNumber.Text = dsEmployeeDetails.Tables[0].Rows[0]["SAPnumber"].ToString();
                    txtLocation.Text = dsEmployeeDetails.Tables[0].Rows[0]["Location"].ToString();
                    txtOrgUnit.Text = dsEmployeeDetails.Tables[0].Rows[0]["OrgUnit"].ToString();
                    txtServiceLine.Text = dsEmployeeDetails.Tables[0].Rows[0]["ServiceLine"].ToString();
                    //chkIsInShift.Checked = bool.Parse(dsEmployeeDetails.Tables[0].Rows[0]["IsInShift"].ToString());
                    //lblTimeInTimeOutType.Text = dsEmployeeDetails.Tables[0].Rows[0]["TimeInTimeOutType"].ToString().ToUpper();

                    //if (dsEmployeeDetails != null && (dsEmployeeDetails.Tables[1] != null && dsEmployeeDetails.Tables[1].Rows.Count == 0) && (dsEmployeeDetails.Tables[2] != null && dsEmployeeDetails.Tables[2].Rows.Count == 0))
                    //{
                    txtTimeIn.Text = DateTime.Now.ToString("HH:mm");
                    txtTimeInDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                    txtTimeIn.Enabled = txtTimeInDate.Enabled = false;

                    btnSaveTimeIn.Enabled = true;

                    if (!chkIsInShift.Checked)
                    {
                        txtTimeOut.Text = DateTime.Now.ToString("HH:mm");
                        txtTimeOutDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                        txtTimeOut.Enabled = txtTimeOutDate.Enabled = false;

                        btnSaveTimeOut.Enabled = true;
                    }
                    else
                    {
                        txtTimeOut.Text = DateTime.Now.ToString("HH:mm");
                        txtTimeOutDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                        txtTimeOut.Enabled = txtTimeOutDate.Enabled = false;

                        btnSaveTimeOut.Enabled = false;
                    }
                    //}
                    // Time in details.
                    if (dsEmployeeDetails != null && dsEmployeeDetails.Tables[1] != null && dsEmployeeDetails.Tables[1].Rows.Count > 0)
                    {
                        txtTimeInDate.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeInDate"].ToString();
                        txtTimeIn.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeIn"].ToString();
                        txtTimeInDate.Enabled = txtTimeIn.Enabled = false;
                        lblWFMEmployeeTimeInTimeOutID.Text = dsEmployeeDetails.Tables[1].Rows[0]["WFMEmployeeTimeInTimeOutID"].ToString();

                        txtTimeOutDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                        txtTimeOut.Text = DateTime.Now.ToString("HH:mm");
                        txtTimeOut.Enabled = false;

                        btnSaveTimeIn.Enabled = false;
                        btnSaveTimeOut.Enabled = true;
                    }
                    // Time out details.
                    if (dsEmployeeDetails != null && dsEmployeeDetails.Tables[2] != null && dsEmployeeDetails.Tables[2].Rows.Count > 0)
                    {
                        if (dsEmployeeDetails != null && dsEmployeeDetails.Tables[1] != null && dsEmployeeDetails.Tables[1].Rows.Count == 0)
                        {
                            txtTimeInDate.Text = "";
                            txtTimeIn.Text = "";
                            txtTimeIn.Enabled = false;
                            lblNoTimeInOnlyTimeOut.Visible = true;
                        }
                        else
                        {
                            txtTimeInDate.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeInDate"].ToString();
                            txtTimeIn.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeIn"].ToString();
                            txtTimeIn.Enabled = false;
                            lblNoTimeInOnlyTimeOut.Visible = false;
                        }

                        txtTimeOutDate.Text = dsEmployeeDetails.Tables[2].Rows[0]["TimeOutDate"].ToString();
                        txtTimeOut.Text = dsEmployeeDetails.Tables[2].Rows[0]["TimeOut"].ToString();
                        txtTimeInDate.Enabled = txtTimeOutDate.Enabled = false;

                        if ((txtTimeInDate.Text.Length > 0) && (txtTimeInDate.Text != txtTimeOutDate.Text))
                        {
                            lblIsInShift.Visible = true;
                            chkIsInShift.Checked = true;
                            chkIsInShift.Enabled = true;
                        }
                        else
                        {
                            lblIsInShift.Visible = false;
                            chkIsInShift.Checked = false;
                            chkIsInShift.Enabled = false;
                        }
                        //chkIsInShift.Enabled = false;
                        btnSaveTimeIn.Enabled = false;
                        btnSaveTimeOut.Enabled = false;
                    }

                    if (dsEmployeeDetails != null && (dsEmployeeDetails.Tables[1] != null) && (dsEmployeeDetails.Tables[2] != null && dsEmployeeDetails.Tables[2].Rows.Count > 0))
                    {
                        if (dsEmployeeDetails.Tables[1].Rows.Count > 0)
                        {
                            txtTimeInDate.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeInDate"].ToString();
                            txtTimeIn.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeIn"].ToString();
                            txtTimeIn.Enabled = txtTimeInDate.Enabled = false;
                        }
                        else
                        {
                            txtTimeInDate.Text = "";
                            txtTimeIn.Text = "";
                            txtTimeIn.Enabled = txtTimeInDate.Enabled = false;
                            chkIsInShift.Enabled = false;
                        }

                        if (dsEmployeeDetails.Tables[2].Rows.Count > 0)
                        {
                            txtTimeOutDate.Text = dsEmployeeDetails.Tables[2].Rows[0]["TimeOutDate"].ToString();
                            txtTimeOut.Text = dsEmployeeDetails.Tables[2].Rows[0]["TimeOut"].ToString();
                            txtTimeInDate.Enabled = txtTimeOut.Enabled = false;
                        }
                        else
                        {
                            txtTimeOutDate.Text = "";
                            txtTimeOut.Text = "";
                            txtTimeOut.Enabled = false;
                            txtTimeInDate.Enabled = txtTimeOut.Enabled = false;
                        }
                        btnSaveTimeIn.Enabled = false;
                        btnSaveTimeOut.Enabled = false;
                    }

                    if (txtTimeInDate.Text.Length > 0 && (txtTimeInDate.Text != txtTimeOutDate.Text))
                    {
                        lblIsInShift.Visible = true;
                        chkIsInShift.Checked = true;
                        chkIsInShift.Enabled = true;
                    }
                    else
                    {
                        lblIsInShift.Visible = false;
                        chkIsInShift.Checked = false;
                        chkIsInShift.Enabled = false;
                    }
                    divMain.Visible = lblChromeMessage.Visible = hlnkHelp.Visible = true;
                }
                else
                {
                    //lblAlertMessage.Text = ConfigurationManager.AppSettings["NotAuthorisedToAccessThisPage"].ToString();
                    //lblAlertMessage.Type = BetiAlertMesageType.Error;
                    string Message = ConfigurationManager.AppSettings["NotAuthorisedToAccessThisPage"].ToString();
                    ShowAlert("Error!", Message, "danger");
                    divMain.Visible = false;
                }
            }
            else
            {
                txtTimeInDate.Enabled = txtTimeIn.Enabled = btnSaveTimeIn.Enabled = false;
                txtTimeOutDate.Enabled = txtTimeOut.Enabled = btnSaveTimeOut.Enabled = false;
            }
        }

        private void getEmployeeDetailsOptimized(string TodayDate)
        {
            WFHRegularisationBLL.WFHRegularisation objWFHRegularisationBLL = new WFHRegularisationBLL.WFHRegularisation();
            WFHRegularisationDAL.WFHRegularisation objWFHRegularisationDAL = new WFHRegularisationDAL.WFHRegularisation();

            DataSet dsEmployeeDetails = objWFHRegularisationDAL.GetEmployeeDetails(TodayDate, HttpContext.Current.Session["DASID"].ToString());

            /*
				Table[0] - Employee Details
				Table[1] - Time-In Details
				Table[2] - Time-Out Details
                If Employee Details found (Table[0]) Process further
			*/
            if (dsEmployeeDetails != null && dsEmployeeDetails.Tables[0] != null && dsEmployeeDetails.Tables[0].Rows.Count > 0)
            {
                //Check by Employee's Company Code, if Authorised to use this page
                bool blnCanAccess = objWFHRegularisationBLL.CanAccessTimeInTimeOut(dsEmployeeDetails.Tables[0].Rows[0]["Company"].ToString().Substring(0, 4));
                if (!blnCanAccess)
                {
                    //lblAlertMessage.Text = ConfigurationManager.AppSettings["NotAuthorisedToAccessThisPage"].ToString();
                    //lblAlertMessage.Type = BetiAlertMesageType.Error;
                    string Message= ConfigurationManager.AppSettings["NotAuthorisedToAccessThisPage"].ToString();
                    ShowAlert("Error!", Message, "danger");
                    divMain.Visible = false;
                    return;
                }/////////////

                #region "Set Employee Details"
                txtName.Text = dsEmployeeDetails.Tables[0].Rows[0]["Name"].ToString();
                txtCompany.Text = dsEmployeeDetails.Tables[0].Rows[0]["Company"].ToString();
                txtDASID.Text = dsEmployeeDetails.Tables[0].Rows[0]["DASID"].ToString();
                txtSAPNumber.Text = dsEmployeeDetails.Tables[0].Rows[0]["SAPnumber"].ToString();
                txtLocation.Text = dsEmployeeDetails.Tables[0].Rows[0]["Location"].ToString();
                txtOrgUnit.Text = dsEmployeeDetails.Tables[0].Rows[0]["OrgUnit"].ToString();
                txtServiceLine.Text = dsEmployeeDetails.Tables[0].Rows[0]["ServiceLine"].ToString();
                //Set Details In Date and Time
                txtTimeIn.Text = DateTime.Now.ToString("HH:mm");
                txtTimeInDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                txtTimeIn.Enabled = txtTimeInDate.Enabled = false;
                //Enable Time-In Button by Default
                btnSaveTimeIn.Enabled = true;
                //Set Details Out Date and Time
                txtTimeOut.Text = DateTime.Now.ToString("HH:mm");
                txtTimeOutDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                txtTimeOut.Enabled = txtTimeOutDate.Enabled = false;
                //Enable Time-In Button by Default
                btnSaveTimeOut.Enabled = true;
                #endregion


                bool timeInFound = false;
                // Set Time in details If found in DB for given Date
                if (dsEmployeeDetails.Tables[1] != null && dsEmployeeDetails.Tables[1].Rows.Count > 0)
                {
                    timeInFound = true;
                    //Set Time In Details from DB
                    txtTimeInDate.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeInDate"].ToString();
                    txtTimeIn.Text = dsEmployeeDetails.Tables[1].Rows[0]["TimeIn"].ToString();
                    lblWFMEmployeeTimeInTimeOutID.Text = dsEmployeeDetails.Tables[1].Rows[0]["WFMEmployeeTimeInTimeOutID"].ToString();
                    btnSaveTimeIn.Enabled = false;
                }
                // Set Time out details If found in DB for given Date
                if (dsEmployeeDetails.Tables[2] != null && dsEmployeeDetails.Tables[2].Rows.Count > 0)
                {
                    if (timeInFound)
                    {//If Time and Timeout both found hide label lblNoTimeInOnlyTimeOut
                        lblNoTimeInOnlyTimeOut.Visible = false;
                    }
                    else
                    {//If Time In not Found but Time out found clear Time-In fields and show label lblNoTimeInOnlyTimeOut
                        txtTimeInDate.Text = "";
                        txtTimeIn.Text = "";
                        lblNoTimeInOnlyTimeOut.Visible = true;
                    }
                    //Set Time Out Details from DB
                    txtTimeOutDate.Text = dsEmployeeDetails.Tables[2].Rows[0]["TimeOutDate"].ToString();
                    txtTimeOut.Text = dsEmployeeDetails.Tables[2].Rows[0]["TimeOut"].ToString();
                    txtTimeInDate.Enabled = txtTimeOutDate.Enabled = false;

                    if (timeInFound && (txtTimeInDate.Text != txtTimeOutDate.Text))
                    {//TimeInDate and TimeOutDate is not same consider user is in shift or working overnight enable shift checkbox and show respective message
                        lblIsInShift.Visible = true;
                        chkIsInShift.Checked = true;
                        chkIsInShift.Enabled = true;
                    }
                    else
                    {//else disable shift checkbox and hide respective message
                        lblIsInShift.Visible = false;
                        chkIsInShift.Checked = false;
                        chkIsInShift.Enabled = false;
                    }
                    //Since both TimeIn and Out found, disable both the buttons
                    btnSaveTimeIn.Enabled = false;
                    btnSaveTimeOut.Enabled = false;
                }

                divMain.Visible = lblChromeMessage.Visible = hlnkHelp.Visible = true;
            }
            else
            {
                txtTimeInDate.Enabled = txtTimeIn.Enabled = btnSaveTimeIn.Enabled = false;
                txtTimeOutDate.Enabled = txtTimeOut.Enabled = btnSaveTimeOut.Enabled = false;
            }
        }

        private void saveTimeRegularisation(string TimeinOutType)
        {
            int intFlag = 0;
            string strMessage = "";

            if (isDataValid())
            {
                string strDate = "";
                string strMonth = "";
                string strYear = "";

                if (TimeinOutType == TIMEIN && txtTimeInDate.Text.Length == 10)
                {
                    strDate = txtTimeInDate.Text.Split('-').GetValue(0).ToString();
                    strMonth = txtTimeInDate.Text.Split('-').GetValue(1).ToString();
                    strYear = txtTimeInDate.Text.Split('-').GetValue(2).ToString();
                }
                else if (TimeinOutType == TIMEOUT && txtTimeOutDate.Text.Length == 10)
                {
                    strDate = txtTimeOutDate.Text.Split('-').GetValue(0).ToString();
                    strMonth = txtTimeOutDate.Text.Split('-').GetValue(1).ToString();
                    strYear = txtTimeOutDate.Text.Split('-').GetValue(2).ToString();
                }
                WFHRegularisationBLL.WFHRegularisation objWFHRegularisationBLL = new WFHRegularisationBLL.WFHRegularisation();
                WFHRegularisationDAL.WFHRegularisation objWFHRegularisationDAL = new WFHRegularisationDAL.WFHRegularisation();

                objWFHRegularisationBLL.Name = txtName.Text;
                objWFHRegularisationBLL.Company = txtCompany.Text;
                objWFHRegularisationBLL.DASID = txtDASID.Text;
                objWFHRegularisationBLL.SAPNumber = txtSAPNumber.Text;
                objWFHRegularisationBLL.Location = txtLocation.Text;
                objWFHRegularisationBLL.OrgUnit = txtOrgUnit.Text;
                objWFHRegularisationBLL.IsInShift = chkIsInShift.Checked;

                if (TimeinOutType == TIMEIN)
                {
                    objWFHRegularisationBLL.RegularisationDate = (strDate + "-" + strMonth + "-" + strYear);
                    objWFHRegularisationBLL.TimeIn = TimeSpan.Parse(txtTimeIn.Text);
                    objWFHRegularisationBLL.TimeInTimeOutType = "I";
                    objWFHRegularisationBLL.IsTimeEntryComplete = false;
                }
                else if (TimeinOutType == TIMEOUT)
                {
                    objWFHRegularisationBLL.RegularisationDate = (strDate + "-" + strMonth + "-" + strYear);
                    objWFHRegularisationBLL.TimeOut = TimeSpan.Parse(txtTimeOut.Text);
                    objWFHRegularisationBLL.TimeInTimeOutType = "O";
                    objWFHRegularisationBLL.WFMEmployeeTimeInTimeOutID = int.Parse(lblWFMEmployeeTimeInTimeOutID.Text);
                    objWFHRegularisationBLL.IsTimeEntryComplete = true;
                }
                //objWFHRegularisationBLL.TimeInTimeOutType = lblTimeInTimeOutType.Text;
                objWFHRegularisationBLL.ServiceLine = txtServiceLine.Text;

                //if (lblWFHEmployeeTimeID.Text.Length > 0)
                //	objWFHRegularisationBLL.WFHEmployeeTimeID = int.Parse(lblWFHEmployeeTimeID.Text);
                //else
                //	objWFHRegularisationBLL.WFHEmployeeTimeID = 0;

                intFlag = objWFHRegularisationDAL.InsertEmployeeTimeInTimeOut(objWFHRegularisationBLL);
                //intFlag = 3;

                switch (intFlag)
                {
                    case 0:
                    case 1:
                        strMessage = SUCCESSFULLYINSERTEDTIMEINTIMEOUT;
                        lblWFHEmployeeTimeID.Text = "0";
                        chkIsInShift.Enabled = false;
                        btnSaveTimeIn.Enabled = btnSaveTimeOut.Enabled = false;

                        ShowAlert("Success!", "Time-In Time-Out entry inserted successfully", "success");
                        //lblAlertMessage.Type = BetiAlertMesageType.Success;
                        break;
                    case 2:
                        strMessage = FAILEDTOADDTIMEINTIMEOUT;
                        ShowAlert("Error!", "Failed to add Time-In Time-Out entry", "danger");
                        //lblAlertMessage.Type = BetiAlertMesageType.Error;
                        break;
                    case 3:
                        strMessage = INOUTTIMEALREADYEXISTS;
                        ShowAlert("Info!", "Time-In Time-Out entry already exists", "info");
                        //lblAlertMessage.Type = BetiAlertMesageType.Info;
                        break;
                }
                //lblAlertMessage.Text = strMessage;
                //lblAlertMessage.Visible = true;
            }
            else
            {
                ShowAlert("Info!", "Invalid Time-In Time-Out entry", "info");
                //lblAlertMessage.Text = INVALIDTIMEINTIMEOUT;
                //lblAlertMessage.Type = BetiAlertMesageType.Info;
                //lblAlertMessage.Visible = true;
            }
            //string script = "var windowObject = window.self; windowObject.opener = window.self; windowObject.close();";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "Close Window", script, true);
        }
        #endregion

        #region "Show Alert"

        void ShowAlert(string Header, string Message, string Type)
        {
            //Pass one of these values in Type: success, danger, warning, info
            AlertMessage.InnerHtml = Message;
            AlertHeader.InnerHtml = Header;
            AddClass(pageAlert, "alert-" + Type);
            AddClass(pageAlert, "show");
        }

        void AddClass(HtmlGenericControl control, string className)
        {
            var current = control.Attributes["class"] ?? "";
            if (!current.Split(' ').Contains(className))
                control.Attributes["class"] = current + " " + className;
        }

        void RemoveClass(HtmlGenericControl control, string className)
        {
            var current = control.Attributes["class"] ?? "";
            control.Attributes["class"] = string.Join(" ",
                current.Split(' ').Where(cls => cls != className));
        }

        #endregion

    }
}