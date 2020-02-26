using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.SqlClient;
using mascis.Models;
using System.Data.Entity;
using System.Configuration;

namespace mascis.GIS_Maps
{
    public partial class GISMaps : System.Web.UI.Page
    {
        mascisEntities context = new mascisEntities();
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    //wdd_Facility.SelectedValue = new UserManagement().getCurrentuser();
            //}
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public string ConvertDataTabletoString()
        {
            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(@"Password=root85;Persist Security Info=True;User ID=sa;Initial Catalog=MEDICALACCESS;Data Source=MEDACCKAMSAP03\MSSQL2017"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spView_Facility_GIS_CodinatesGetAll";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    SqlParameter[] sqlParameters = new SqlParameter[1];

                    sqlParameters[0] = new SqlParameter("@FacilityCode", SqlDbType.Int);
                    sqlParameters[0].IsNullable = true;
                    if (wdd_Facility.SelectedValue != "")
                        sqlParameters[0].Value = wdd_Facility.SelectedValue;
                    else
                        sqlParameters[0].Value = DBNull.Value;
                    sqlParameters[0].Direction = ParameterDirection.Input;
 
                     
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(sqlParameters);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return serializer.Serialize(rows);
                }
            }
        }

        public IQueryable<A_Facilities> getComponent()
        {
            try
            {
                return context.A_Facilities.OrderBy(s => s.Facility);

            }
            catch (Exception)
            {
                return null;
            }
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {

        }
    }
}