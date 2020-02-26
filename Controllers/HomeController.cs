#region Copyright Syncfusion Inc. 2001-2017.
// Copyright Syncfusion Inc. 2001-2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mascis.Models;

namespace mascis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexAdministrator()
        {
            return View();
        }

        public ActionResult IndexHSIP()
        {
            return View();
        }

        public ActionResult IndexMAULT()
        {
            return View();
        }

        public ActionResult IndexFacilityHSIP()
        {
            return View();
        }

        public ActionResult IndexFacilityMAULT()
        {
            return View();
        }

        public ActionResult IndexSTO()
        {
            return View();
        }

        public ActionResult IndexCodinator()
        {
            return View();
        }

        public ActionResult IndexCHSS()
        {
            return View();
        }

        public ActionResult IndexCCU()
        {
            return View();
        }

        public ActionResult IndexQualityAssurance()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Mi()
        {
            //UserManagement obj = new UserManagement();
            //obj.createRole("Administrator");
            //obj.createRole("TechnicalSupportOfficer");
            //obj.createRole("Facility");
            //obj.createRole("MAULT");
            //obj.createRole("MonitoringAndEvaluation");

            return View();
        }
    }
}