using mascis.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace mascis.Controllers
{
    public class DashboardController : Controller
    {
        mascisEntities context = new mascisEntities();
        // GET: Dashboard
        public ActionResult FacilityDashboard()
        {
            ViewBag.Dash = "YES";
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var facility = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility_id).Facility;

            ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\dashboard.sydx";
            DashboardWindowsServiceInfo dashboardViewer = new DashboardWindowsServiceInfo();
            ViewBag.Errormessage = dashboardViewer.Errormessage;
            ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
            // ViewBag.ServiceUrl = dashboardViewer.ServiceUrl;
             //ViewBag.ServiceUrl = "http://10.33.0.15:5556/DashboardService.svc";
            //ViewBag.ServiceUrl = "https://mascis.medicalaccess.co.ug:5556/";

            ViewBag.ServiceUrl = "https://mascis.medicalaccess.co.ug:5556/DashboardService.svc";

            ViewBag.filterParams = "Facility =" + GetNumbers(facility);//"Facility=fcName";
            return View();
        }
        //LMISDashboard
        public ActionResult LMISDashboard()
        {
            ViewBag.Dash = "YES";
            //ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\mascis_home.sydx";
            ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\dashboard.sydx";
            DashboardWindowsServiceInfo dashboardViewer = new DashboardWindowsServiceInfo();
            ViewBag.Errormessage = dashboardViewer.Errormessage;
            ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
            //ViewBag.ServiceUrl = dashboardViewer.ServiceUrl;
            //ViewBag.ServiceUrl = "http://10.33.0.15:5556/DashboardService.svc";
            //ViewBag.ServiceUrl = "https://mascis.medicalaccess.co.ug:5556/DashboardService.svc"; 
            ViewBag.ServiceUrl = "https://mascis.medicalaccess.co.ug:5556/DashboardService.svc";

            //if (user != null)//Check if the login user is valid or not
            //    ViewBag.filterParameters = "Company Id =" + Session["TenantId"]; //Tenant ID of the current user is get from session and passing to filter parameters variable.
            //else
            //    ViewBag.filterParameters = "";

            //ViewBag.filterParams = "Facility =" + GetFacility(new UserManagement().getUserFacilityList());//"Facility=fcName";
            return View();
        }
        public ActionResult SCTODashboard()
        {
            ViewBag.Dash = "YES";
            //ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\Mascis_FacilityDashboard.sydx";
            ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\dashboard.sydx";
            DashboardWindowsServiceInfo dashboardViewer = new DashboardWindowsServiceInfo();
            ViewBag.Errormessage = dashboardViewer.Errormessage;
            ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
            //ViewBag.ServiceUrl = dashboardViewer.ServiceUrl;
            ViewBag.ServiceUrl = "http://10.33.0.15:5556/DashboardService.svc";
            //ViewBag.ServiceUrl = "https://mascis.medicalaccess.co.ug:5556/";
            //ViewBag.ServiceUrl = "https://mascis.medicalaccess.co.ug:5556/DashboardService.svc";
            //if (user != null)//Check if the login user is valid or not
            //    ViewBag.filterParameters = "Company Id =" + Session["TenantId"]; //Tenant ID of the current user is get from session and passing to filter parameters variable.
            //else
            //    ViewBag.filterParameters = "";

            ViewBag.filterParams = "Facility =" + GetFacility(new UserManagement().getUserFacilityList());//"Facility=fcName";
            return View();
        }

        public bool CheckInternetConnection()
        {
            string CheckUrl = "http://mobapp.medicalaccess.co.ug:5555/api/Facility_View";
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 5000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;

            }
            catch (WebException ex)
            {
                // Console.WriteLine (".....no connection..." + ex.ToString ());

                return false;
            }
        }

        private static string GetNumbers(string input)
        {
            return new string(input.Where(c => c != '"').ToArray());
        }
        public ActionResult Dashboard()
        {
            ViewBag.Dash = "YES";
            ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\mascis_home.sydx";
            DashboardWindowsServiceInfo dashboardViewer = new DashboardWindowsServiceInfo();
            ViewBag.Errormessage = dashboardViewer.Errormessage;
            ViewBag.ServiceUrl = dashboardViewer.ServiceUrl;
            return View();
        }

        private string GetFacility(List<int> id)
        {
            string facilitries = string.Empty;
            //if (id.Contains(","))
            {
                //string[] b = id.Split(',');
                foreach (var a in id)
                {
                    //if (!a.Contains(","))
                    {
                        int c = Convert.ToInt32(a);
                        facilitries += context.A_Facilities.FirstOrDefault(o => o.FacilityCode == c).Facility;
                        facilitries += ",";

                    }
                }
            }

            return facilitries;
        }

    }
    public class DashboardParameterSettings
    {
        public bool ShowIcon;

        public List<DashboardParameterData> Data;
    }

    public class DashboardParameterData
    {
        public string ParameterName;

        public bool ShowInParameterDialog;

        public bool ShowInPromptDialog;

        public string Value;

        public List<ParameterValue> Values;

        public ParameterValueRange DateValue;
    }

    public class ParameterValueRange
    {
        public string Start;

        public string End;
    }
  
    public class ParameterValue
    {
        public string Value;
    }
}