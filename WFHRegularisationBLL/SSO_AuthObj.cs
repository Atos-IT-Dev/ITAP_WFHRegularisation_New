using System.Web;
using Microsoft.IdentityModel.Claims;
using System.Configuration;

namespace WFHRegularisationBLL
{
    public class SSO_AuthObj
    {
        public SSO_AuthObj()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void SetAppParams()
        {
            try
            {
                IClaimsPrincipal claimsPrincipal = HttpContext.Current.User as IClaimsPrincipal;
                IClaimsIdentity claimsIdentity = (IClaimsIdentity)claimsPrincipal.Identity;
                string strLoggedDasId = "", strLoggedUserName = "";
                foreach (Claim claim in claimsIdentity.Claims)
                {
                    if (claim.ClaimType.ToLower().Contains("dasid"))
                    {
                        strLoggedDasId = claim.Value;
                    }
                    if (claim.ClaimType.ToLower().Contains("name"))
                    {
                        strLoggedUserName = claim.Value;
                    }
                }
                if (ConfigurationSettings.AppSettings["DASID"] != null)
                {
                    HttpContext.Current.Session.Add(ConfigurationSettings.AppSettings["NAME"], strLoggedUserName);
                    HttpContext.Current.Session.Add(ConfigurationSettings.AppSettings["DASID"], strLoggedDasId);
                }
                HttpContext.Current.Session.Add("NAME", strLoggedUserName);
                HttpContext.Current.Session.Add("DASID", strLoggedDasId);
            }
            catch { }

        }

    }
}