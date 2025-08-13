using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.IdentityModel.Web;
using System.Web.Security;
using WFHRegularisationDAL;

namespace WFHRegularisationUI
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // added by Suvarna
            if (HttpContext.Current.Session["DASID"] == null || (ConfigurationManager.AppSettings["DASID"] != null && HttpContext.Current.Session[ConfigurationManager.AppSettings["DASID"].ToString()] == null))
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                if (Context.Session != null) Context.Session.Abandon();
                Context.Response.Redirect(ConfigurationManager.AppSettings["SignoutURL"], false);
                return;
            }
            if (!IsPostBack)
            {
                EmployeeData.SetEmpoyeeSession();
                lbl_Userrole_Box.Text = Session["USER_ROLES"].ToString();
                lbl_Dasid_Box.Text= Session["DASID"].ToString();
                lbl_UserName_Box.Text = Session["Name"].ToString();
                lbl_UserName_Dropdown.InnerText = Session["Name"].ToString();
                h6_Env.InnerText = "Environment-" + ConfigurationManager.AppSettings["ENV"];
				VersionLink.Text = Session["APP_VERSION"].ToString();
			}
        }
    }
}