using mascis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mascis.Controllers
{
    public class DashboardViewerController : Controller
    {
        // GET: DashboardViewer
        public ActionResult Index()
        {
            //ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "bin\\\\WorldWideCarSalesDashboard.sydx"; // Or use the remote (online) Dashboard Path. For example, https://dashboardsdk.syncfusion.com//Dashboards//WorldWideCarSalesDashboard.sydx
            //ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\mascis_home.sydx";
            //ViewBag.ServiceUrl = "http://localhost:54770/DashboardService"; // Or use the remote (online) Dashboard Service. For example, https://dashboardsdk.syncfusion.com/DashboardService/DashboardService.svc;
            //return View();
            //localhost:17144
            //ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "bin\\\\mascis_home.sydx";

            //DashboardWindowsServiceInfo dashboardViewer = new DashboardWindowsServiceInfo();
            //ViewBag.Errormessage = dashboardViewer.Errormessage;
            //ViewBag.ServiceUrl = dashboardViewer.ServiceUrl;
            //return View();

            ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "bin\\\\WorldWideCarSalesDashboard.sydx"; // Or use the remote (online) Dashboard Path. For example, https://dashboardsdk.syncfusion.com//Dashboards//WorldWideCarSalesDashboard.sydx
            ViewBag.ServiceUrl = "http://localhost:17144"; // Or use the remote (online) Dashboard Service. For example, https://dashboardsdk.syncfusion.com/DashboardService/DashboardService.svc;
            return View();

        }

        public ActionResult Dashboard()
        {
            //ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\mascis_home.sydx";
            ViewBag.DashboardPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "\\\\") + "Dashboards\\\\mascis_home.sydx";

            DashboardWindowsServiceInfo dashboardViewer = new DashboardWindowsServiceInfo();
            ViewBag.Errormessage = dashboardViewer.Errormessage;
            ViewBag.ServiceUrl = dashboardViewer.ServiceUrl;
            return View();
        }
    }
}