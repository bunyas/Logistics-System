using mascis.Models;
using Newtonsoft.Json;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace mascis.Controllers
{
    public class CHSSController : Controller
    {
        string Module;
        public IEnumerable currentData;
        List<Scoring_Summary> ScoreSummary;
        List<StoreCondition> ConditionList;
        List<StoreCleanliness> CleanlinessList;
        List<StoreSystem> SystemList;
        List<StorePractice> PracticeList;
        List<Chss_Facility_Visits> mvisits;
        private class RegCodes
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        List<RegCodes> codes = new List<RegCodes>();
        // GET: CHSS
        mascisEntities contex = new mascisEntities();
        public ActionResult Index()
        {
             codes = new List<RegCodes>()
                {
                    new RegCodes(){ ID=2,Name="TDF/3TC/EFV"},
                    new RegCodes(){ ID=117,Name="TDF/3TC+DTG"},
                    new RegCodes(){ ID=5,Name="ABC/3TC/EFV"},
                    new RegCodes(){ ID=14,Name="ABC/3TC/EFV"},
                    new RegCodes(){ ID=13,Name="ABC/3TC/NVP"},
                    new RegCodes(){ ID=18,Name="ABC+3TC+LPV/r"},
                    new RegCodes(){ ID=26,Name="ABC+3TC+LPV/r"},
                    new RegCodes(){ ID=119,Name="ABC+3TC+LPV/r"},
                    //new RegCodes(){ ID=17,Name="AZT/3TC/LPV/r"},
                };
            mascisEntities db = new mascisEntities();
            Summary(db.Chss_Facility_Visits.ToList());
            BindVisits(db.Chss_Facility_Visits.ToList());
            BindLinkedToSap();
            BindSolutionInstalled();
            BindMos("230003");
            LoadProducts();
            BindMonths();
            BindYear();
            return View();
        }
        public ActionResult PatientCareAndManagement()
        {
             codes = new List<RegCodes>()
                {
                    new RegCodes(){ ID=2,Name="TDF/3TC/EFV"},
                    new RegCodes(){ ID=117,Name="TDF/3TC+DTG"},
                    new RegCodes(){ ID=5,Name="ABC/3TC/EFV"},
                    new RegCodes(){ ID=14,Name="ABC/3TC/EFV"},
                    new RegCodes(){ ID=13,Name="ABC/3TC/NVP"},
                    new RegCodes(){ ID=18,Name="ABC+3TC+LPV/r"},
                    new RegCodes(){ ID=26,Name="ABC+3TC+LPV/r"},
                    new RegCodes(){ ID=119,Name="ABC+3TC+LPV/r"},
                    //new RegCodes(){ ID=17,Name="AZT/3TC/LPV/r"},
                };
            var fa = contex.A_Facilities.Select(e => new { e.Facility, e.FacilityCode }).OrderBy(e => e.Facility).ToList();
            ViewBag.facilities = fa;
            return View();
        }
        public ActionResult StockManagement()
        {
            BindMonths();
            BindYear();
            return View();
        }
        public ActionResult Traceability()
        {
            BindMonths();
            BindYear();
            return View();
        }
        public ActionResult OrderingAndReporting()
        {
            BindMonths();
            BindYear();
            return View();
        }
        public ActionResult StoreManagement()
        {
            BindMonths();
            BindYear();
            return View();
        }
        public ActionResult ExpiryTracking()
        {
            BindMonths();
            BindYear();
            return View();
        }
        public ActionResult FeedBack()
        {
            BindMonths();
            BindYear();
            return View();
        }
        public ActionResult ScoringSummary()
        {
            BindMonths();
            BindYear();
            return View();
        }
        private void BindMonths()
        {
            List<Months> m = new List<Months>
            {
                new Months() { ID = 1, Description = "JANUARY" },
                new Months() { ID = 2, Description = "FEBRUARY" },
                new Months() { ID = 3, Description = "MARCH" },
                new Months() { ID = 4, Description = "APRIL" },
                new Months() { ID = 5, Description = "MAY" },
                new Months() { ID = 6, Description = "JUNE" },
                new Months() { ID = 7, Description = "JULY" },
                new Months() { ID = 8, Description = "AUGUST" },
                new Months() { ID = 9, Description = "SEPTEMBER" },
                new Months() { ID = 10, Description = "OCTOBER" },
                new Months() { ID = 11, Description = "NOVEMBER" },
                new Months() { ID = 12, Description = "DECEMBER" }
            };
            ViewData["DropDownSource"] = m;
        }
        private void BindYear()
        {
            List<string> list = new List<string>();
            for (int i = 0; i <= 15; i++)
            {
                int w = DateTime.Now.Year - i;
                list.Add(w.ToString());
                ViewData["DropDownSource1"] = list;
            }
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindFacilityDetails(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Details> mlist = new List<Chss_M_Details>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var m = db.Chss_M_Details.ToList();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();

                            if (a.Count > 0)
                            {
                                foreach (var c in a)
                                {

                                    mlist.Add(c);
                                }
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)

                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();

                            if (a.Count > 0)
                            {
                                foreach (var c in a)
                                {

                                    mlist.Add(c);
                                }
                            }
                        }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var m = db.Chss_M_Details.ToList();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();

                            if (a.Count > 0)
                            {

                                foreach (var c in a)
                                {

                                    mlist.Add(c);
                                }
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)

                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();

                            if (a.Count > 0)
                            {

                                foreach (var c in a)
                                {

                                    mlist.Add(c);
                                }
                            }
                        }
                }
            }
            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindEquipment(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_Equipment_Verification.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_Equipment_Verification.Where(o => ((o.Date_of_visit >= sDate && o.Date_of_visit <= eDate) || (sDate == null && o.Date_of_visit == eDate) || (o.Date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.FacilityCode == facilityid || facilityid == 0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindLocation(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_Location.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_Location.Where(o=> ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null)|| (sDate == null && eDate == null)) && (o.facility_Code == facilityid || facilityid==0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindSupervisor(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_Supervisors.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_Supervisors.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.facility_code == facilityid || facilityid == 0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindSupervised(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_Supervised.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_Supervised.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.facility_code == facilityid || facilityid == 0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindCareAvailability(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_ART_Patient_Care_Availability.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_ART_Patient_Care_Availability.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.facility_code == facilityid || facilityid == 0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindARTRegister(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_ART_Patient_Care_Register.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_ART_Patient_Care_Register.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.facility_code == facilityid || facilityid == 0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
            
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindDispensingLog(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_ART_Patient_Care_DispensingLog.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_ART_Patient_Care_DispensingLog.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.facility_code == facilityid || facilityid == 0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindPaediatric(string startMonth, string endMonth, int facilityid)
        {

            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_ART_Patient_Care_Treatment.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_ART_Patient_Care_Treatment.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.FacilityCode == facilityid || facilityid == 0) && o.regimen_classification_code == 2).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindAdolsent(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_ART_Patient_Care_Treatment.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_ART_Patient_Care_Treatment.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.FacilityCode == facilityid || facilityid == 0) && o.regimen_classification_code == 1).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindStock(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_Stock_Management.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_Stock_Management.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.facility_code == facilityid || facilityid == 0)).ToList();

            return Json(mlist, "application/json",Encoding.UTF8,JsonRequestBehavior.AllowGet);
          
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindSpecialization(string startMonth, string endMonth, int facilityid)
        {
            List<ArtSpecialization> mlist = new List<ArtSpecialization>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Traceability_Specialisation.Where(e => e.Result == true).ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var d in selectedVisits)
                        {
                            var x = m.Where(e => e.facility_code == d.facility_code & e.date_of_visit.Date == d.date_of_visit.Date).ToList();
                            if (x.Count > 0)
                            {
                                foreach (var n in x)
                                {
                                    ArtSpecialization a = new ArtSpecialization
                                    {
                                        facility_code = n.facility_code,
                                        CDCRegion = n.CDCRegion,
                                        CDCRegionId = n.CDCRegionId,
                                        ComprehensiveImplimentingPartnerCode = n.ComprehensiveImplimentingPartnerCode,
                                        ComprehensiveImplimentingPartnerDescription = n.ComprehensiveImplimentingPartnerDescription,
                                        date_of_visit = n.date_of_visit,
                                        District_Name = n.District_Name,
                                        DistrrictCode = n.DistrrictCode,
                                        Facility = n.Facility,
                                        ImplimentingPartnerCode = n.ImplimentingPartnerCode,
                                        ImplimentingPartnerDescription = n.ImplimentingPartnerDescription,
                                        Level_Desc = n.Level_desc,
                                        //level_of_care = n.l,
                                        SAP_Code = n.SAP_Code,
                                        SCTO = n.SCTO
                                    };
                                    if (n.Result == true)
                                    {
                                        a.Description = n.Description;
                                    }
                                    mlist.Add(a);
                                }
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var d in selectedVisits)
                        {
                            var x = m.Where(e => e.facility_code == d.facility_code & e.date_of_visit.Date == d.date_of_visit.Date).ToList();
                            if (x.Count > 0)
                            {
                                foreach (var n in x)
                                {
                                    ArtSpecialization a = new ArtSpecialization
                                    {
                                        facility_code = n.facility_code,
                                        CDCRegion = n.CDCRegion,
                                        CDCRegionId = n.CDCRegionId,
                                        ComprehensiveImplimentingPartnerCode = n.ComprehensiveImplimentingPartnerCode,
                                        ComprehensiveImplimentingPartnerDescription = n.ComprehensiveImplimentingPartnerDescription,
                                        date_of_visit = n.date_of_visit,
                                        District_Name = n.District_Name,
                                        DistrrictCode = n.DistrrictCode,
                                        Facility = n.Facility,
                                        ImplimentingPartnerCode = n.ImplimentingPartnerCode,
                                        ImplimentingPartnerDescription = n.ImplimentingPartnerDescription,
                                        Level_Desc = n.Level_desc,
                                        // level_of_care = n.,
                                        SAP_Code = n.SAP_Code,
                                        SCTO = n.SCTO
                                    };
                                    if (n.Result == true)
                                    {
                                        a.Description = n.Description;
                                    }
                                    mlist.Add(a);
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Traceability_Specialisation.Where(e => e.Result == true).ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var d in selectedVisits)
                        {
                            var x = m.Where(e => e.facility_code == d.facility_code & e.date_of_visit.Date == d.date_of_visit.Date).ToList();
                            if (x.Count > 0)
                            {
                                foreach (var n in x)
                                {
                                    ArtSpecialization a = new ArtSpecialization
                                    {
                                        facility_code = n.facility_code,
                                        CDCRegion = n.CDCRegion,
                                        CDCRegionId = n.CDCRegionId,
                                        ComprehensiveImplimentingPartnerCode = n.ComprehensiveImplimentingPartnerCode,
                                        ComprehensiveImplimentingPartnerDescription = n.ComprehensiveImplimentingPartnerDescription,
                                        date_of_visit = n.date_of_visit,
                                        District_Name = n.District_Name,
                                        DistrrictCode = n.DistrrictCode,
                                        Facility = n.Facility,
                                        ImplimentingPartnerCode = n.ImplimentingPartnerCode,
                                        ImplimentingPartnerDescription = n.ImplimentingPartnerDescription,
                                        Level_Desc = n.Level_desc,
                                        //level_of_care = n.level_of_care,
                                        SAP_Code = n.SAP_Code,
                                        SCTO = n.SCTO
                                    };
                                    if (n.Result == true)
                                    {
                                        a.Description = n.Description;
                                    }
                                    mlist.Add(a);
                                }
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var d in selectedVisits)
                        {
                            var x = m.Where(e => e.facility_code == d.facility_code & e.date_of_visit.Date == d.date_of_visit.Date).ToList();
                            if (x.Count > 0)
                            {
                                foreach (var n in x)
                                {
                                    ArtSpecialization a = new ArtSpecialization
                                    {
                                        facility_code = n.facility_code,
                                        CDCRegion = n.CDCRegion,
                                        CDCRegionId = n.CDCRegionId,
                                        ComprehensiveImplimentingPartnerCode = n.ComprehensiveImplimentingPartnerCode,
                                        ComprehensiveImplimentingPartnerDescription = n.ComprehensiveImplimentingPartnerDescription,
                                        date_of_visit = n.date_of_visit,
                                        District_Name = n.District_Name,
                                        DistrrictCode = n.DistrrictCode,
                                        Facility = n.Facility,
                                        ImplimentingPartnerCode = n.ImplimentingPartnerCode,
                                        ImplimentingPartnerDescription = n.ImplimentingPartnerDescription,
                                        Level_Desc = n.Level_desc,
                                        //level_of_care = n.level_of_care,
                                        SAP_Code = n.SAP_Code,
                                        SCTO = n.SCTO
                                    };
                                    if (n.Result == true)
                                    {
                                        a.Description = n.Description;
                                    }
                                    mlist.Add(a);
                                }
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindRequisitionSystem(string startMonth, string endMonth, int facilityid)
        {
            List<ARTRequisition> mlist = new List<ARTRequisition>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Traceability_Requisition_System.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTRequisition c = new ARTRequisition();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Sum = (a[0].Result + a[1].Result + a[2].Result);
                                double percent = (((float)(c.Sum) / 3) * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTRequisition c = new ARTRequisition();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Sum = (a[0].Result + a[1].Result + a[2].Result);
                                double percent = (((float)(c.Sum) / 3) * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";
                                mlist.Add(c);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Traceability_Requisition_System.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTRequisition c = new ARTRequisition();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Sum = (a[0].Result + a[1].Result + a[2].Result);
                                double percent = (((float)(c.Sum) / 3) * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTRequisition c = new ARTRequisition();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Sum = (a[0].Result + a[1].Result + a[2].Result);
                                double percent = (((float)(c.Sum) / 3) * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";
                                mlist.Add(c);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindSuplier(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Traceability_Supplier_To_Facility_Store> mlist = new List<Chss_M_Traceability_Supplier_To_Facility_Store>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var list = db.Chss_M_Traceability_Supplier_To_Facility_Store.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var list = db.Chss_M_Traceability_Supplier_To_Facility_Store.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindFacilityStore(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Traceability_Facility_Store_To_DispensingUnit> mlist = new List<Chss_M_Traceability_Facility_Store_To_DispensingUnit>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var list = db.Chss_M_Traceability_Facility_Store_To_DispensingUnit.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var list = db.Chss_M_Traceability_Facility_Store_To_DispensingUnit.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindIssuedToPatients(string startMonth, string endMonth, int facilityid)
        {
            List<ARTPatients> mlist = new List<ARTPatients>();

            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Traceability_Issued_To_Patients.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTPatients c = new ARTPatients();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Results;
                                c.Q_two = a[1].Results;
                                c.Q_three = a[2].Results;
                                c.Q_four = a[3].Results;
                                c.Q_five = a[4].Results;
                                c.Q_six = a[5].Results;
                                c.Sum = a[5].Results;
                                c.Percentage = (c.Sum * 100) + "%";
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTPatients c = new ARTPatients();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Results;
                                c.Q_two = a[1].Results;
                                c.Q_three = a[2].Results;
                                c.Q_four = a[3].Results;
                                c.Q_five = a[4].Results;
                                c.Q_six = a[5].Results;
                                c.Sum = a[5].Results;
                                c.Percentage = (c.Sum * 100) + "%";
                                mlist.Add(c);
                            }
                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Traceability_Issued_To_Patients.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTPatients c = new ARTPatients();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Results;
                                c.Q_two = a[1].Results;
                                c.Q_three = a[2].Results;
                                c.Q_four = a[3].Results;
                                c.Q_five = a[4].Results;
                                c.Q_six = a[5].Results;
                                c.Sum = a[5].Results;
                                c.Percentage = (c.Sum * 100) + "%";
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            ARTPatients c = new ARTPatients();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_Desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Results;
                                c.Q_two = a[1].Results;
                                c.Q_three = a[2].Results;
                                c.Q_four = a[3].Results;
                                c.Q_five = a[4].Results;
                                c.Q_six = a[5].Results;
                                c.Sum = a[5].Results;
                                c.Percentage = (c.Sum * 100) + "%";
                                mlist.Add(c);
                            }
                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindAccuracy(string startMonth, string endMonth, int facilityid)
        {

            List<Chss_M_Order_Accuracy> orderList = new List<Chss_M_Order_Accuracy>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindOrderReport(string startMonth, string endMonth, int facilityid)
        {
            List<OrderReport> ReportList = new List<OrderReport>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Order_Report.ToList();
                if (startMonth == endMonth)
                {

                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            OrderReport c = new OrderReport();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Q_four = a[3].Result;
                                c.Q_five = a[4].Result;
                                c.Q_six = a[5].Result;
                                c.Sum = a[5].Result;
                                c.Percentage = (c.Sum * 100) + "%";
                                ReportList.Add(c);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            OrderReport c = new OrderReport();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Q_four = a[3].Result;
                                c.Q_five = a[4].Result;
                                c.Q_six = a[5].Result;
                                c.Sum = a[5].Result;
                                c.Percentage = (c.Sum * 100) + "%";
                                ReportList.Add(c);
                            }
                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var m = db.Chss_M_Order_Report.ToList();
                if (startMonth == endMonth)
                {

                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            OrderReport c = new OrderReport();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Q_four = a[3].Result;
                                c.Q_five = a[4].Result;
                                c.Q_six = a[5].Result;
                                c.Sum = a[5].Result;
                                c.Percentage = (c.Sum * 100) + "%";
                                ReportList.Add(c);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Accuracy.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = m.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            OrderReport c = new OrderReport();
                            if (a.Count > 0)
                            {
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_one = a[0].Result;
                                c.Q_two = a[1].Result;
                                c.Q_three = a[2].Result;
                                c.Q_four = a[3].Result;
                                c.Q_five = a[4].Result;
                                c.Q_six = a[5].Result;
                                c.Sum = a[5].Result;
                                c.Percentage = (c.Sum * 100) + "%";
                                ReportList.Add(c);
                            }
                        }
                    }
                }
            }
            return Json(ReportList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindOrderBalance(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Order_Balance> orderList = new List<Chss_M_Order_Balance>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Balance.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Balance.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Balance.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Balance.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindPatientReport(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Order_Patients_Report> orderList = new List<Chss_M_Order_Patients_Report>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Patients_Report.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Patients_Report.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Patients_Report.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_Patients_Report.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindHivtestkits(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Order_TestKits> orderList = new List<Chss_M_Order_TestKits>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_TestKits.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_TestKits.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_TestKits.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Order_TestKits.ToList();
                        foreach (var n in selectedVisits)
                        {
                            //Chss_M_Order_Accuracy mOrder = new Chss_M_Order_Accuracy();
                            var mOrder = list.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                orderList.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindTrackingTools(string startMonth, string endMonth, int facilityid)
        {
            List<ExpiryTools> mlist = new List<ExpiryTools>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) && facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Tracking_Tools.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryTools c = new ExpiryTools();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.ARVs = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.ARVs_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Lab = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Lab_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Oppo = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Oppo_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Tracking_Tools.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryTools c = new ExpiryTools();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.ARVs = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.ARVs_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Lab = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Lab_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Oppo = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Oppo_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Tracking_Tools.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryTools c = new ExpiryTools();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.ARVs = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.ARVs_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Lab = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Lab_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Oppo = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Oppo_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Tracking_Tools.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryTools c = new ExpiryTools();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.ARVs = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.ARVs_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Lab = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Lab_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        if (z.Guid_Id == 59)
                                        {
                                            c.Oppo = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }
                                        else if (z.Guid_Id == 60)
                                        {
                                            c.Oppo_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "0")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindFefoUse(string startMonth, string endMonth, int facilityid)
        {
            List<Expiry_FEFO> mlist = new List<Expiry_FEFO>();
            if (string.IsNullOrEmpty(startMonth) && string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth) && facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Fefo_Use.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Expiry_FEFO c = new Expiry_FEFO();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 3)
                                    {
                                        c.Store = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 6)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 5)
                                    {
                                        c.Dispensary = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Fefo_Use.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Expiry_FEFO c = new Expiry_FEFO();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 3)
                                    {
                                        c.Store = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 6)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 5)
                                    {
                                        c.Dispensary = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Fefo_Use.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Expiry_FEFO c = new Expiry_FEFO();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 3)
                                    {
                                        c.Store = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 6)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 5)
                                    {
                                        c.Dispensary = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Fefo_Use.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Expiry_FEFO c = new Expiry_FEFO();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 3)
                                    {
                                        c.Store = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 6)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 5)
                                    {
                                        c.Dispensary = z.Result;
                                        if (z.Result == "1")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindExpiryPresence(string startMonth, string endMonth, int facilityid)
        {
            List<ExpiryPresence> mlist = new List<ExpiryPresence>();
            if (string.IsNullOrEmpty(startMonth) && string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Presence.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryPresence c = new ExpiryPresence();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        c.ARVs = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        c.Oppo = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 10)
                                    {
                                        if (z.Guid_Id == 56)
                                        {
                                            c.Q_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }

                                        }
                                        if (z.Guid_Id == 57)
                                        {
                                            c.Q_2 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (z.Guid_Id == 58)
                                        {
                                            c.Q_3 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Presence.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryPresence c = new ExpiryPresence();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        c.ARVs = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        c.Oppo = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 10)
                                    {
                                        if (z.Guid_Id == 56)
                                        {
                                            c.Q_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }

                                        }
                                        if (z.Guid_Id == 57)
                                        {
                                            c.Q_2 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (z.Guid_Id == 58)
                                        {
                                            c.Q_3 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }
                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Presence.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryPresence c = new ExpiryPresence();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        c.ARVs = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        c.Oppo = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 10)
                                    {
                                        if (z.Guid_Id == 56)
                                        {
                                            c.Q_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }

                                        }
                                        if (z.Guid_Id == 57)
                                        {
                                            c.Q_2 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (z.Guid_Id == 58)
                                        {
                                            c.Q_3 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Expiry_Presence.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var a = list.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                ExpiryPresence c = new ExpiryPresence();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                int count = 0;
                                foreach (var z in a)
                                {
                                    if (z.Category_Code == 7)
                                    {
                                        c.ARVs = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 8)
                                    {
                                        c.Lab = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 9)
                                    {
                                        c.Oppo = z.Result;
                                        if (z.Result == "0")
                                        {
                                            c.Score += 1;
                                            count += 1;
                                        }
                                        else if (z.Result == "1")
                                        {
                                            count += 1;
                                        }
                                    }
                                    if (z.Category_Code == 10)
                                    {
                                        if (z.Guid_Id == 56)
                                        {
                                            c.Q_1 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }

                                        }
                                        if (z.Guid_Id == 57)
                                        {
                                            c.Q_2 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (z.Guid_Id == 58)
                                        {
                                            c.Q_3 = z.Result;
                                            if (z.Result == "1")
                                            {
                                                c.Score += 1;
                                                count += 1;
                                            }
                                            else if (z.Result == "1")
                                            {
                                                count += 1;
                                            }
                                        }

                                    }
                                }
                                double r = ((float)(c.Score) / count);
                                double percent = (r * 100);
                                c.Percentage = Math.Round(percent, 0) + "%";

                                mlist.Add(c);
                            }
                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoadCleanlinessDataGrid(string startDate, string endDate, int facilityid)
        {
            if (string.IsNullOrEmpty(startDate) & string.IsNullOrEmpty(endDate)) { }
            else if (!string.IsNullOrEmpty(startDate) & !string.IsNullOrEmpty(endDate) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                CleanlinessList = new List<StoreCleanliness>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCleanliness(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCleanliness(selectedVisits);
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                CleanlinessList = new List<StoreCleanliness>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCleanliness(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCleanliness(selectedVisits);
                    }
                }
            }
            return Json(CleanlinessList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoadSystemDataGrid(string startDate, string endDate, int facilityid)
        {
            if (string.IsNullOrEmpty(startDate) & string.IsNullOrEmpty(endDate)) { }
            else if (!string.IsNullOrEmpty(startDate) & !string.IsNullOrEmpty(endDate) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                SystemList = new List<StoreSystem>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindStorageSystem(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindStorageSystem(selectedVisits);
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                SystemList = new List<StoreSystem>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindStorageSystem(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindStorageSystem(selectedVisits);
                    }
                }
            }
            return Json(SystemList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoadPracticeDataGrid(string startDate, string endDate, int facilityid)
        {
            if (string.IsNullOrEmpty(startDate) & string.IsNullOrEmpty(endDate)) { }
            else if (!string.IsNullOrEmpty(startDate) & !string.IsNullOrEmpty(endDate) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                PracticeList = new List<StorePractice>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindPractice(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindPractice(selectedVisits);
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                PracticeList = new List<StorePractice>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindPractice(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindPractice(selectedVisits);
                    }
                }
            }
            return Json(PracticeList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoadConditionDataGrid(string startDate, string endDate, int facilityid)
        {
            if (string.IsNullOrEmpty(startDate) & string.IsNullOrEmpty(endDate)) { }
            else if (!string.IsNullOrEmpty(startDate) & !string.IsNullOrEmpty(endDate) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                ConditionList = new List<StoreCondition>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCondition(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCondition(selectedVisits);
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                ConditionList = new List<StoreCondition>();
                mvisits = new List<Chss_Facility_Visits>();
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                if (startDate == endDate)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCondition(selectedVisits);
                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        BindCondition(selectedVisits);
                    }
                }
            }
            return Json(ConditionList, JsonRequestBehavior.AllowGet);
        }
        private void BindCleanliness(List<Chss_Facility_Visits> visits)
        {
            mascisEntities db = new mascisEntities();
            var x = db.Chss_M_Storage_Cleanliness.ToList();
            CleanlinessList = new List<StoreCleanliness>();
            // var visits = db.Chss_Facility_Visits.ToList();
            foreach (var n in visits)
            {
                var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                if (a.Count > 0)
                {
                    StoreCleanliness c = new StoreCleanliness();
                    c.CDCRegion = a[0].CDCRegion;
                    c.CDCRegionId = a[0].CDCRegionId;
                    c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                    c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                    c.date_of_visit = a[0].date_of_visit;
                    c.District_Name = a[0].District_Name;
                    c.DistrrictCode = a[0].DistrrictCode;
                    c.Facility = a[0].Facility;
                    c.facility_code = a[0].facility_code;
                    c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                    c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                    c.Level_Desc = a[0].Level_desc;
                    c.level_of_care = a[0].level_of_care;
                    c.SAP_Code = a[0].SAP_Code;
                    c.SCTO = a[0].SCTO;
                    c.Q_1 = a[0].Score;
                    c.Q_2 = a[1].Score;
                    c.Q_3 = a[2].Score;

                    int sum = 0;
                    if (a[0].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[1].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[2].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[0].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[1].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[2].Score == "0")
                    {
                        sum += 1;
                    }
                    double r = (float)(c.Score) / sum;
                    double percent = (r * 100);
                    c.Percentage = Math.Round(percent, 0) + "%";
                    CleanlinessList.Add(c);
                }

            }
            ViewBag.cleanliness = CleanlinessList;
        }
        private void BindStorageSystem(List<Chss_Facility_Visits> visits)
        {
            mascisEntities db = new mascisEntities();
            var x = db.Chss_M_Storage_System.ToList();
            SystemList = new List<StoreSystem>();
            // var visits = db.Chss_Facility_Visits.ToList();
            foreach (var n in visits)
            {
                var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                if (a.Count > 0)
                {
                    StoreSystem c = new StoreSystem();
                    c.CDCRegion = a[0].CDCRegion;
                    c.CDCRegionId = a[0].CDCRegionId;
                    c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                    c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                    c.date_of_visit = a[0].date_of_visit;
                    c.District_Name = a[0].District_Name;
                    c.DistrrictCode = a[0].DistrrictCode;
                    c.Facility = a[0].Facility;
                    c.facility_code = a[0].facility_code;
                    c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                    c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                    c.Level_Desc = a[0].Level_desc;
                    c.level_of_care = a[0].level_of_care;
                    c.SAP_Code = a[0].SAP_Code;
                    c.SCTO = a[0].SCTO;
                    c.Q_1 = a[0].Score;
                    c.Q_2 = a[1].Score;
                    c.Q_3 = a[2].Score;
                    c.Q_4 = a[3].Score;
                    int sum = 0;
                    if (a[0].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[1].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[2].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[3].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[0].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[1].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[2].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[3].Score == "0")
                    {
                        sum += 1;
                    }
                    double r = (float)(c.Score) / sum;
                    double percent = (r * 100);
                    c.Percentage = Math.Round(percent, 0) + "%";
                    SystemList.Add(c);
                }

            }
            ViewBag.Storesystem = SystemList;

        }
        private void BindPractice(List<Chss_Facility_Visits> visits)
        {
            mascisEntities db = new mascisEntities();
            var x = db.Chss_M_Storage_Practice.ToList();
            PracticeList = new List<StorePractice>();
            //var visits = db.Chss_Facility_Visits.ToList();
            foreach (var n in visits)
            {
                var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                if (a.Count > 0)
                {
                    StorePractice c = new StorePractice();
                    c.CDCRegion = a[0].CDCRegion;
                    c.CDCRegionId = a[0].CDCRegionId;
                    c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                    c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                    c.date_of_visit = a[0].date_of_visit;
                    c.District_Name = a[0].District_Name;
                    c.DistrrictCode = a[0].DistrrictCode;
                    c.Facility = a[0].Facility;
                    c.facility_code = a[0].facility_code;
                    c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                    c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                    c.level_Desc = a[0].Level_desc;
                    c.level_of_care = a[0].level_of_care;
                    c.SAP_Code = a[0].SAP_Code;
                    c.SCTO = a[0].SCTO;
                    c.Q_1 = a[0].Score;
                    c.Q_2 = a[1].Score;
                    c.Q_3 = a[2].Score;
                    c.Q_4 = a[3].Score;
                    c.Q_5 = a[4].Score;
                    int sum = 0;
                    if (a[0].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[1].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[2].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[3].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[4].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[0].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[1].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[2].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[3].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[4].Score == "0")
                    {
                        sum += 1;
                    }
                    double r = (float)(c.Score) / sum;
                    double percent = (r * 100);
                    c.Percentage = Math.Round(percent, 0) + "%";
                    PracticeList.Add(c);
                }

            }
            ViewBag.Storepractice = PracticeList;
        }
        private void BindCondition(List<Chss_Facility_Visits> visits)
        {
            mascisEntities db = new mascisEntities();
            var x = db.Chss_M_Store_Condition.ToList();
            ConditionList = new List<StoreCondition>();
            //var visits = db.Chss_Facility_Visits.ToList();
            foreach (var n in visits)
            {
                var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                if (a.Count > 0)
                {
                    StoreCondition c = new StoreCondition();
                    c.CDCRegion = a[0].CDCRegion;
                    c.CDCRegionId = a[0].CDCRegionId;
                    c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                    c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                    c.date_of_visit = a[0].date_of_visit;
                    c.District_Name = a[0].District_Name;
                    c.DistrrictCode = a[0].DistrrictCode;
                    c.Facility = a[0].Facility;
                    c.facility_code = a[0].facility_code;
                    c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                    c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                    c.Level_Desc = a[0].Level_desc;
                    c.level_of_care = a[0].level_of_care;
                    c.SAP_Code = a[0].SAP_Code;
                    c.SCTO = a[0].SCTO;
                    c.Q_1 = a[0].Score;
                    c.Q_2 = a[1].Score;
                    c.Q_3 = a[2].Score;
                    c.Q_4 = a[3].Score;
                    c.Q_5 = a[4].Score;
                    c.Q_6 = a[5].Score;
                    c.Q_7 = a[6].Score;
                    c.Q_8 = a[7].Score;
                    c.Q_9 = a[8].Score;
                    c.Q_10 = a[9].Score;
                    c.Q_11 = a[10].Score;
                    c.Q_12 = a[11].Score;
                    c.Q_13 = a[12].Score;
                    int sum = 0;
                    if (a[0].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[1].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[2].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[3].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[4].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[5].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[6].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[7].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[8].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[9].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[10].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[11].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[12].Score == "1")
                    {
                        c.Score += 1;
                        sum += 1;
                    }
                    if (a[0].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[1].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[2].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[3].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[4].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[5].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[6].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[7].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[8].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[9].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[10].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[11].Score == "0")
                    {
                        sum += 1;
                    }
                    if (a[12].Score == "0")
                    {
                        sum += 1;
                    }
                    double r = (float)(c.Score) / sum;
                    double percent = (r * 100);
                    c.Percentage = Math.Round(percent, 0) + "%";
                    ConditionList.Add(c);
                }

            }
            ViewBag.Storecondition = ConditionList;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindFeedBack(string startMonth, string endMonth, int facilityid)
        {
            List<Feedbackdetails> mlist = new List<Feedbackdetails>();
            if (string.IsNullOrEmpty(startMonth) && string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_FeedBackDetails.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var p = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            if (p.Count > 0)
                            {
                                foreach (var t in p)
                                {
                                    Feedbackdetails c = new Feedbackdetails();
                                    c.CDCRegion = t.CDCRegion;
                                    c.CDCRegionId = t.CDCRegionId;
                                    c.ComprehensiveImplimentingPartnerCode = t.ComprehensiveImplimentingPartnerCode;
                                    c.ComprehensiveImplimentingPartnerDescription = t.ComprehensiveImplimentingPartnerDescription;
                                    c.date_of_visit = t.date_of_visit;
                                    c.District_Name = t.District_Name;
                                    c.DistrrictCode = t.DistrrictCode;
                                    c.Facility = t.Facility;
                                    c.facility_code = t.facility_code;
                                    c.ImplimentingPartnerCode = t.ImplimentingPartnerCode;
                                    c.ImplimentingPartnerDescription = t.ImplimentingPartnerDescription;
                                    c.Level_desc = t.Level_desc;
                                    c.level_of_care = t.level_of_care;
                                    c.SAP_Code = t.SAP_Code;
                                    c.SCTO = t.SCTO;
                                    c.Module_Desc = t.Module_Desc;
                                    c.Sector_Desc = t.Sector_Desc;
                                    c.Current_Issues = t.Comment;
                                    c.Previous_Issues = t.Previous_Findings;
                                    c.Previous_Followup = t.Followup;
                                    c.Time_line = t.Time_line;
                                    c.Responsible_Person = t.Responsible_Person;
                                    c.Recommendation = t.Recommendation;
                                    c.Action_Plan = t.Action_Plan;

                                    mlist.Add(c);
                                }
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var p = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            if (p.Count > 0)
                            {
                                foreach (var t in p)
                                {
                                    Feedbackdetails c = new Feedbackdetails();
                                    c.CDCRegion = t.CDCRegion;
                                    c.CDCRegionId = t.CDCRegionId;
                                    c.ComprehensiveImplimentingPartnerCode = t.ComprehensiveImplimentingPartnerCode;
                                    c.ComprehensiveImplimentingPartnerDescription = t.ComprehensiveImplimentingPartnerDescription;
                                    c.date_of_visit = t.date_of_visit;
                                    c.District_Name = t.District_Name;
                                    c.DistrrictCode = t.DistrrictCode;
                                    c.Facility = t.Facility;
                                    c.facility_code = t.facility_code;
                                    c.ImplimentingPartnerCode = t.ImplimentingPartnerCode;
                                    c.ImplimentingPartnerDescription = t.ImplimentingPartnerDescription;
                                    c.Level_desc = t.Level_desc;
                                    c.level_of_care = t.level_of_care;
                                    c.SAP_Code = t.SAP_Code;
                                    c.SCTO = t.SCTO;
                                    c.Module_Desc = t.Module_Desc;
                                    c.Sector_Desc = t.Sector_Desc;
                                    c.Current_Issues = t.Comment;
                                    c.Previous_Issues = t.Previous_Findings;
                                    c.Previous_Followup = t.Followup;
                                    c.Time_line = t.Time_line;
                                    c.Responsible_Person = t.Responsible_Person;
                                    c.Recommendation = t.Recommendation;
                                    c.Action_Plan = t.Action_Plan;

                                    mlist.Add(c);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_FeedBackDetails.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var p = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            if (p.Count > 0)
                            {
                                foreach (var t in p)
                                {
                                    Feedbackdetails c = new Feedbackdetails();
                                    c.CDCRegion = t.CDCRegion;
                                    c.CDCRegionId = t.CDCRegionId;
                                    c.ComprehensiveImplimentingPartnerCode = t.ComprehensiveImplimentingPartnerCode;
                                    c.ComprehensiveImplimentingPartnerDescription = t.ComprehensiveImplimentingPartnerDescription;
                                    c.date_of_visit = t.date_of_visit;
                                    c.District_Name = t.District_Name;
                                    c.DistrrictCode = t.DistrrictCode;
                                    c.Facility = t.Facility;
                                    c.facility_code = t.facility_code;
                                    c.ImplimentingPartnerCode = t.ImplimentingPartnerCode;
                                    c.ImplimentingPartnerDescription = t.ImplimentingPartnerDescription;
                                    c.Level_desc = t.Level_desc;
                                    c.level_of_care = t.level_of_care;
                                    c.SAP_Code = t.SAP_Code;
                                    c.SCTO = t.SCTO;
                                    c.Module_Desc = t.Module_Desc;
                                    c.Sector_Desc = t.Sector_Desc;
                                    c.Current_Issues = t.Comment;
                                    c.Previous_Issues = t.Previous_Findings;
                                    c.Previous_Followup = t.Followup;
                                    c.Time_line = t.Time_line;
                                    c.Responsible_Person = t.Responsible_Person;
                                    c.Recommendation = t.Recommendation;
                                    c.Action_Plan = t.Action_Plan;

                                    mlist.Add(c);
                                }
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var p = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            if (p.Count > 0)
                            {
                                foreach (var t in p)
                                {
                                    Feedbackdetails c = new Feedbackdetails();
                                    c.CDCRegion = t.CDCRegion;
                                    c.CDCRegionId = t.CDCRegionId;
                                    c.ComprehensiveImplimentingPartnerCode = t.ComprehensiveImplimentingPartnerCode;
                                    c.ComprehensiveImplimentingPartnerDescription = t.ComprehensiveImplimentingPartnerDescription;
                                    c.date_of_visit = t.date_of_visit;
                                    c.District_Name = t.District_Name;
                                    c.DistrrictCode = t.DistrrictCode;
                                    c.Facility = t.Facility;
                                    c.facility_code = t.facility_code;
                                    c.ImplimentingPartnerCode = t.ImplimentingPartnerCode;
                                    c.ImplimentingPartnerDescription = t.ImplimentingPartnerDescription;
                                    c.Level_desc = t.Level_desc;
                                    c.level_of_care = t.level_of_care;
                                    c.SAP_Code = t.SAP_Code;
                                    c.SCTO = t.SCTO;
                                    c.Module_Desc = t.Module_Desc;
                                    c.Sector_Desc = t.Sector_Desc;
                                    c.Current_Issues = t.Comment;
                                    c.Previous_Issues = t.Previous_Findings;
                                    c.Previous_Followup = t.Followup;
                                    c.Time_line = t.Time_line;
                                    c.Responsible_Person = t.Responsible_Person;
                                    c.Recommendation = t.Recommendation;
                                    c.Action_Plan = t.Action_Plan;

                                    mlist.Add(c);
                                }
                            }
                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindRX_Solution(string startMonth, string endMonth, int facilityid)
        {
            List<RX_Solution> mlist = new List<RX_Solution>();
            if (string.IsNullOrEmpty(startMonth) && string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_FeedBack_Solution.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                RX_Solution c = new RX_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                RX_Solution c = new RX_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_FeedBack_Solution.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                RX_Solution c = new RX_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                RX_Solution c = new RX_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindPSolution(string startMonth, string endMonth, int facilityid)
        {
            List<Pharm_Solution> mlist = new List<Pharm_Solution>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_Pharm_Solution.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Pharm_Solution c = new Pharm_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Pharm_Solution c = new Pharm_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }
                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_Pharm_Solution.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Pharm_Solution c = new Pharm_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }
                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var a = x.Where(e => e.date_of_visit.Date == n.date_of_visit.Date && e.facility_code == n.facility_code).ToList();
                            if (a.Count > 0)
                            {
                                Pharm_Solution c = new Pharm_Solution();
                                c.CDCRegion = a[0].CDCRegion;
                                c.CDCRegionId = a[0].CDCRegionId;
                                c.ComprehensiveImplimentingPartnerCode = a[0].ComprehensiveImplimentingPartnerCode;
                                c.ComprehensiveImplimentingPartnerDescription = a[0].ComprehensiveImplimentingPartnerDescription;
                                c.date_of_visit = a[0].date_of_visit;
                                c.District_Name = a[0].District_Name;
                                c.DistrrictCode = a[0].DistrrictCode;
                                c.Facility = a[0].Facility;
                                c.facility_code = a[0].facility_code;
                                c.ImplimentingPartnerCode = a[0].ImplimentingPartnerCode;
                                c.ImplimentingPartnerDescription = a[0].ImplimentingPartnerDescription;
                                c.Level_desc = a[0].Level_desc;
                                c.level_of_care = a[0].level_of_care;
                                c.SAP_Code = a[0].SAP_Code;
                                c.SCTO = a[0].SCTO;
                                c.Q_1 = a[0].Result;
                                c.Q_2 = a[1].Result;
                                c.Q_3 = a[2].Result;
                                c.Q_4 = a[3].Result;
                                c.Q_5 = a[4].Result;
                                c.Q_6 = a[5].Result;
                                c.Q_7 = a[6].Result;
                                mlist.Add(c);
                            }
                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindActionArea(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_FeedBack_ActionArea> mlist = new List<Chss_M_FeedBack_ActionArea>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_FeedBack_ActionArea.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var y in mOrder)
                            {
                                mlist.Add(y);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Stock_Management.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var y in mOrder)
                            {
                                mlist.Add(y);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var x = db.Chss_M_FeedBack_ActionArea.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var y in mOrder)
                            {
                                mlist.Add(y);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        var list = db.Chss_M_Stock_Management.ToList();
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = x.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var y in mOrder)
                            {
                                mlist.Add(y);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindScores(string startMonth, string endMonth, int facilityid)
        {
            mascisEntities db = new mascisEntities();
            var mlist = db.Chss_M_Scores.ToList();
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startMonth))
            {
                sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(endMonth))
            {
                eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            }

            mlist = db.Chss_M_Scores.Where(o => ((o.date_of_visit >= sDate && o.date_of_visit <= eDate) || (sDate == null && o.date_of_visit == eDate) || (o.date_of_visit == sDate && eDate == null) || (sDate == null && eDate == null)) && (o.facility_Code == facilityid || facilityid == 0)).ToList();

            return Json(mlist, JsonRequestBehavior.AllowGet);

            //ScoreSummary = new List<Scoring_Summary>();
            //if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            //else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            //{
            //    DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            //    DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            //    mascisEntities db = new mascisEntities();
            //    var visits = db.Chss_Facility_Visits.ToList();
            //    if (startMonth == endMonth)
            //    {
            //        var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
            //        if (selectedVisits.Count > 0)
            //        {
            //            Summary(selectedVisits);

            //        }
            //    }
            //    else
            //    {
            //        var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
            //        if (selectedVisits.Count > 0)
            //        {
            //            Summary(selectedVisits);
            //        }
            //    }
            //}
            //else
            //{
            //    DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
            //    DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
            //    mascisEntities db = new mascisEntities();
            //    var visits = db.Chss_Facility_Visits.ToList();
            //    if (startMonth == endMonth)
            //    {
            //        var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
            //        if (selectedVisits.Count > 0)
            //        {
            //            Summary(selectedVisits);

            //        }
            //    }
            //    else
            //    {
            //        var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
            //        if (selectedVisits.Count > 0)
            //        {
            //            Summary(selectedVisits);
            //        }
            //    }
            //}

            //return Json(ScoreSummary, JsonRequestBehavior.AllowGet);
        }
        private void Summary(List<Chss_Facility_Visits> Facilitiesvisits)
        {
            ScoringRepository repository = new ScoringRepository();
            repository.LoadScores(Facilitiesvisits);
            ScoreSummary = repository.ScoresCollection;
        }
        private void BindLinkedToSap()
        {
            List<Chss_M_FeedBack_Solution> Final_list = new List<Chss_M_FeedBack_Solution>();
            LinkedToSAP m = new LinkedToSAP();
            LinkedToSAP n = new LinkedToSAP();
            List<LinkedToSAP> mlist = new List<LinkedToSAP>();
            mascisEntities db = new mascisEntities();
            var facilities = db.Chss_M_Facilities.Where(e => e.IsActive == true).ToList();
            var RX_sol = db.Chss_M_FeedBack_Solution.Where(e => e.Guid_Id == 67 & e.Result == 1).OrderBy(e => e.date_of_visit).ToList();
            foreach (var x in facilities)
            {
                var t = RX_sol.Where(e => e.facility_code == x.FacilityCode).ToList();
                if (t.Count > 0)
                {
                    Final_list.Add(t[0]);
                }
            }
            int SolCount = Final_list.Count;
            //int SolCount = 100;
            int Fcount = facilities.Count;
            double calc = (float)(SolCount) / (float)(Fcount);
            int solPer = Convert.ToInt32(calc * 100);
            int nonePer = 100 - solPer;
            m.Range_name = "LINKED TO MASCIS/SAP " + solPer.ToString() + "%";
            m.Percentage = solPer;
            n.Range_name = "NOT LINKED TO MASCIS/SAP " + nonePer.ToString() + "%";
            n.Percentage = nonePer;
            mlist.Add(n);
            mlist.Add(m);
            ViewBag.Datasource2 = mlist;

        }
        private void BindSolutionInstalled()
        {
            List<Chss_M_FeedBack_Solution> Final_list = new List<Chss_M_FeedBack_Solution>();
            LinkedToSAP m = new LinkedToSAP();
            LinkedToSAP n = new LinkedToSAP();
            List<LinkedToSAP> mlist = new List<LinkedToSAP>();
            mascisEntities db = new mascisEntities();
            var facilities = db.Chss_M_Facilities.Where(e => e.IsActive == true).ToList();
            var RX_sol = db.Chss_M_FeedBack_Solution.Where(e => e.Guid_Id == 65 & e.Result == 1).OrderBy(e => e.date_of_visit).ToList();
            foreach (var x in facilities)
            {
                var t = RX_sol.Where(e => e.facility_code == x.FacilityCode).ToList();
                if (t.Count > 0)
                {
                    Final_list.Add(t[0]);
                }
            }
            int SolCount = Final_list.Count;
            //int SolCount = 100;
            int Fcount = facilities.Count;
            double calc = (float)(SolCount) / (float)(Fcount);
            int solPer = Convert.ToInt32(calc * 100);
            int nonePer = 100 - solPer;
            m.Range_name = "With Rx Solution installed " + solPer.ToString() + "%";
            m.Percentage = solPer;
            n.Range_name = "Without Rx Solution installed " + nonePer.ToString() + "%";
            n.Percentage = nonePer;
            mlist.Add(n);
            mlist.Add(m);
            ViewBag.Datasource3 = mlist;

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindMos(string _product)
        {

            List<Scoring_Range_Chart> mlist = new List<Scoring_Range_Chart>();
            if (!string.IsNullOrEmpty(_product))
            {
                Scoring_Range_Chart a = new Scoring_Range_Chart();
                Scoring_Range_Chart b = new Scoring_Range_Chart();
                Scoring_Range_Chart c = new Scoring_Range_Chart();
                int Below = 0, between = 0, above = 0;
                mascisEntities db = new mascisEntities();
                var stock = db.Chss_M_Stock_Management.ToList();
                var facilities = db.Chss_M_Facilities.Where(e => e.IsActive == true).ToList();
                foreach (var n in facilities)
                {
                    var m = stock.Where(e => e.facility_code == n.FacilityCode & e.product_code == Convert.ToInt32(_product)).ToList();
                    if (m.Count > 0)
                    {
                        foreach (var x in m)
                        {
                            if (x.Month_Of_Stock >= 4)
                            {
                                above += 1;
                            }
                            else if (x.Month_Of_Stock < 2)
                            {
                                Below += 1;
                            }
                            else if (x.Month_Of_Stock >= 2 & x.Month_Of_Stock < 4)
                            {
                                between += 1;
                            }
                        }
                    }
                    a.Range_name = "Below 2 months";
                    a.Percentage = Below;
                    b.Range_name = "Between 2 to 4 months";
                    b.Percentage = between;
                    c.Range_name = "above 4 months";
                    c.Percentage = above;

                }
                mlist.Add(a);
                mlist.Add(b);
                mlist.Add(c);
                ViewBag.datasource4 = mlist;
            }
            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindFeedBackPams(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Feedback_Parameters> mlist = new List<Chss_M_Feedback_Parameters>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var z = db.Chss_M_Feedback_Parameters.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var z = db.Chss_M_Feedback_Parameters.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.facility_code == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BindStockMovement(string startMonth, string endMonth, int facilityid)
        {
            List<Chss_M_Frost_Stock_Movement> mlist = new List<Chss_M_Frost_Stock_Movement>();
            if (string.IsNullOrEmpty(startMonth) & string.IsNullOrEmpty(endMonth)) { }
            else if (!string.IsNullOrEmpty(startMonth) & !string.IsNullOrEmpty(endMonth) & facilityid > 0)
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var z = db.Chss_M_Frost_Stock_Movement.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.FacilityCode == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date & e.facility_code == facilityid).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.FacilityCode == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }
            else
            {
                DateTime sDate = DateTime.ParseExact(startMonth, "dd/MM/yyyy", null);
                DateTime eDate = DateTime.ParseExact(endMonth, "dd/MM/yyyy", null);
                mascisEntities db = new mascisEntities();
                var visits = db.Chss_Facility_Visits.ToList();
                var z = db.Chss_M_Frost_Stock_Movement.ToList();
                if (startMonth == endMonth)
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date == sDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.FacilityCode == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }

                    }
                }
                else
                {
                    var selectedVisits = visits.Where(e => e.date_of_visit.Date >= sDate.Date & e.date_of_visit.Date <= eDate.Date).ToList();
                    if (selectedVisits.Count > 0)
                    {
                        foreach (var n in selectedVisits)
                        {
                            var mOrder = z.Where(e => e.FacilityCode == n.facility_code & e.date_of_visit.Date == n.date_of_visit.Date).ToList();
                            foreach (var x in mOrder)
                            {
                                mlist.Add(x);
                            }

                        }
                    }
                }
            }

            return Json(mlist, JsonRequestBehavior.AllowGet);
        }
        private void BindVisits(List<Chss_Facility_Visits> visits)
        {
            mascisEntities db = new mascisEntities();
            var Facilities = db.Chss_M_Facilities.ToList();
            List<Chss_Facility_Visits> final_Visits = new List<Chss_Facility_Visits>();
            foreach (var n in Facilities)
            {
                var x = visits.Where(e => e.facility_code == n.FacilityCode).OrderBy(e => e.date_of_visit).ToList();
                if (x.Count > 0)
                {
                    final_Visits.Add(x[0]);
                }
            }
            ScoringRepository summary = new ScoringRepository();
            summary.LoadScores(final_Visits);
            ViewBag.datasource1 = summary.Scores_ChartCollection;
        }
        private void LoadProducts()
        {
            mascisEntities db = new mascisEntities();
            var product = db.Chss_M_Products.OrderBy(e => e.product_description).ToList();
            ViewData["pros"] = product;
        }

        public void ExportToExcel(string GridModel)
        {
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            //GridProperties obj = (GridProperties)Syncfusion.JavaScript.Utils.DeserializeToModel(typeof(GridProperties), GridModel);
            exp.Export(obj, currentData, Module + ".xlsx", ExcelVersion.Excel2010, false, false, /*"flat-saffron"*/"none");

            //ExcelExport exp = new ExcelExport();
            //ExcelEngine excel = new ExcelEngine();
            //IApplication application = excel.Excel;
            //GridProperties obj = ConvertGridObject(GridModel);
            //IWorkbook workbook = application.Workbooks.Create(2);

            ////List<Orders> reports = BindDataSource();


            //workbook.Worksheets[0].ImportData(currentData, 1, 1, false);

            //workbook.SaveAs(Module + ".xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2010);
        }
        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483644;
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (ds.Key == "FacilityData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Details>>(str);
                    Module = "CHSS Facility details " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "SupervisorData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Supervisors>>(str);
                    Module = "CHSS Supervisors " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "SupervisedData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Supervised>>(str);
                    Module = "CHSS Supervised " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "LocationData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Location>>(str);
                    Module = "CHSS Location " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "SpecializationData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ArtSpecialization>>(str);
                    Module = "CHSS Traceability of commodities Specialization " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "RequisitionSystemData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ARTRequisition>>(str);
                    Module = "CHSS Traceability of commodities Requisitioning system " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "SupplierData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Traceability_Supplier_To_Facility_Store>>(str);
                    Module = "CHSS Traceability of commodities From the supplier to the Facility store " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "FacilityStoreData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Traceability_Facility_Store_To_DispensingUnit>>(str);
                    Module = "CHSS Traceability of commodities From Facility store to the ART dispensing unit " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "PatientsIssuedData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ARTPatients>>(str);
                    Module = "CHSS Traceability of commodities Issued to patients " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "CleanlinessData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<StoreCleanliness>>(str);
                    Module = "CHSS Store Management Store cleanliness " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "SystemData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<StoreSystem>>(str);
                    Module = "CHSS Store Management Storage system " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "ConditionData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<StoreCondition>>(str);
                    Module = "CHSS Store Management Storage condition " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "PracticeData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<StorePractice>>(str);
                    Module = "CHSS Store Management Storage practice " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "StockData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Stock_Management>>(str);
                    Module = "CHSS Stock Management " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "CareAvailabilityData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_ART_Patient_Care_Availability>>(str);
                    Module = "CHSS ART Patient Care and Management Tools availability " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "DispensingLogData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_ART_Patient_Care_DispensingLog>>(str);
                    Module = "CHSS ART Patient Care and Management Dispensing log " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "ARTRegisterData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_ART_Patient_Care_Register>>(str);
                    Module = "CHSS ART Patient Care and Management Art register " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "AdolescentData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_ART_Patient_Care_Treatment>>(str);
                    Module = "CHSS ART Patient Care and Management Initiation of adolscent to treatment " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "PaediatricData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_ART_Patient_Care_Treatment>>(str);
                    Module = "CHSS ART Patient Care and Management Initiation of Peadiatrics to treatment " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "FeedbackData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Feedbackdetails>>(str);
                    Module = "CHSS FeedBack Details " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "RX_solutionData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<RX_Solution>>(str);
                    Module = "CHSS FeedBack Details Rx solution " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "Ph_solutionData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Pharm_Solution>>(str);
                    Module = "CHSS FeedBack Details Pharmaconvigilence solution " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "ActionAreaData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_FeedBack_ActionArea>>(str);
                    Module = "CHSS FeedBack Details Action area " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "FefoUseData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Expiry_FEFO>>(str);
                    Module = "CHSS Expiry tracking FEFO use " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "ExpiryData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ExpiryPresence>>(str);
                    Module = "CHSS Expiry tracking Presence of expired commodities " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "TrackingData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ExpiryTools>>(str);
                    Module = "CHSS Expiry tracking Tools " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "OrderReportData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<OrderReport>>(str);
                    Module = "CHSS Ordering and reporting Order Report " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "OrderAccuracyData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Order_Accuracy>>(str);
                    Module = "CHSS Ordering and reporting  Accuracy of ARV and E-MTCT Medicines Order " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "BalanceComparisonData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Order_Balance>>(str);
                    Module = "CHSS Ordering and reporting  Accuracy of ARV and E-MTCT Medicines Order balances " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "OrderPatientData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Order_Patients_Report>>(str);
                    Module = "CHSS Ordering and reporting Accuracy of Patient Report " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "TestKitsData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Order_TestKits>>(str);
                    Module = "CHSS Ordering and reporting Accuracy of HIV test kits order  " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "ScoringData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Scores>>(str);
                    Module = "CHSS Scoring summary " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "MovementData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Frost_Stock_Movement>>(str);
                    Module = "Option For Stock Movement" + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "ParameterData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Feedback_Parameters>>(str);
                    Module = "Stock Analysis" + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "EquipData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Equipment_Verification>>(str);
                    Module = "Equipment Verification" + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (property != null)
                {
                    Type type = property.PropertyType;
                    string serialize = serializer.Serialize(ds.Value);
                    object value = serializer.Deserialize(serialize, type);
                    property.SetValue(gridProp, value, null);
                }
            }
            return gridProp;
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

    }
}