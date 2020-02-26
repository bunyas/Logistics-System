using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using Newtonsoft.Json;
using System.Collections;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using Syncfusion.XlsIO;
using Syncfusion.JavaScript.DataSources;
using Syncfusion.JavaScript;

namespace mascis.Controllers
{
    public class A_FacilitiesController : Controller
    {
        string cat = string.Empty;
        string Module;
        public IEnumerable currentData;
        private mascisEntities db = new mascisEntities();

        // GET: A_Facilities
        public ActionResult Index()
        {
            var a_Facilities = db.A_Facilities.Include(a => a.A_CDCRegion).Include(a => a.A_DeliveryZone).Include(a => a.A_District).Include(a => a.A_Facilities1).Include(a => a.A_Facilities2).Include(a => a.A_Facility_Level_Of_Care).Include(a => a.A_FacilityType).Include(a => a.A_ImplimentingPartners).Include(a => a.A_Ownership);
            return View(a_Facilities.ToList());
        }
        public ActionResult Facilities()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cdcregion = db.A_CDCRegion.AsNoTracking().ToArray();
            ViewBag.CDCRegionId = cdcregion;

            db.Configuration.ProxyCreationEnabled = false;
            var deliveryzone = db.A_DeliveryZone.AsNoTracking().ToArray();
            ViewBag.DeliveryZoneCode = deliveryzone;

            db.Configuration.ProxyCreationEnabled = false;
            var districts = db.A_District.AsNoTracking().Where(a=>a.District_Name != null).ToArray();
            ViewBag.DistrrictCode = districts;

            //var facilities = db.A_Facilities.AsNoTracking().ToArray();
            //ViewBag.FacilityCode = facilities;

            db.Configuration.ProxyCreationEnabled = false;
            var level_of_Care = db.A_Facility_Level_Of_Care.AsNoTracking().ToArray();
            ViewBag.level_of_care = level_of_Care;

            db.Configuration.ProxyCreationEnabled = false;
            var FacilityTypes = db.A_FacilityType.AsNoTracking().ToArray();
            ViewBag.FacilityTypeId = FacilityTypes;

            db.Configuration.ProxyCreationEnabled = false;
            var ImplimentingPartners = db.A_ImplimentingPartners.AsNoTracking().ToArray();
            ViewBag.ComprehensiveImplimentingPartnerCode = ImplimentingPartners;

            var ownerships = db.A_Ownership.AsNoTracking().ToArray();
            ViewBag.OwnershipId = ownerships;

            var client = db.A_ClientType.AsNoTracking().ToArray();
            ViewBag.client_type_code = client;

            db.Configuration.ProxyCreationEnabled = false;
            var users = db.AspNetUsers.AsNoTracking().ToArray();
            ViewBag.Users = users;

            db.Configuration.ProxyCreationEnabled = false;
            var patientloads = db.A_PatientLoad.AsNoTracking().ToArray();
            ViewBag.PatientLoads = patientloads;

            db.Configuration.ProxyCreationEnabled = false;
            var scto = db.fo_SCTO_ContactPerson.AsNoTracking().ToArray();
            ViewBag.SCTO = scto;
            db.Configuration.ProxyCreationEnabled = false;
            var _scto = db.fo_SCTO.AsNoTracking().ToArray();
            ViewBag._SCTO = _scto;

            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.View_A_Facility.ToList(); 
            ViewBag.datasource = DataSource;

            //var a_Facilities = db.A_Facilities.Include(a => a.A_CDCRegion).Include(a => a.A_DeliveryZone).Include(a => a.A_District).Include(a => a.A_Facilities1).Include(a => a.A_Facilities2).Include(a => a.A_Facility_Level_Of_Care).Include(a => a.A_FacilityType).Include(a => a.A_ImplimentingPartners).Include(a => a.A_Ownership);
            return View(/*a_Facilities.ToList()*/);
        }
        public ActionResult DataSourceFacility(DataManager dm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.View_A_Facility.OrderBy(o => o.Facility).ToList();
            int count = db.View_A_Facility.ToList().Count();

            DataOperations operation = new DataOperations();
            //Performing filtering operation
            if (dm.Where != null)
            {
                data = operation.PerformWhereFilter(data, dm.Where, "and");
                var filtered = (IEnumerable<object>)data;
                count = filtered.Count();
            }
            //Performing search operation
            if (dm.Search != null)
            {
                data = operation.PerformSearching(data, dm.Search);
                var searched = (IEnumerable<object>)data;
                count = searched.Count();
            }
            //Performing sorting operation
            if (dm.Sorted != null)
                data = operation.PerformSorting(data, dm.Sorted);

            //Performing paging operations
            if (dm.Skip != 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take != 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SCTO()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var UserTable = db.AspNetUsers.AsNoTracking().ToArray();
            ViewBag.user = UserTable;
            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.fo_SCTO_ContactPerson.ToList();
            ViewBag.datasource = DataSource;
            return View();
        }
        public ActionResult GetSCTO()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.fo_SCTO_ContactPerson.ToList();
            return Json(DataSource, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveScto(string scto,string Telephone, string Tel, string email,string userid, int _type)
        {
            var result = string.Empty;
            if(_type == 1)
            {
                if (string.IsNullOrEmpty(scto))
                {
                    result = "Please type in the scto";
                }
                else
                {
                    var userexits = db.fo_SCTO_ContactPerson.FirstOrDefault(e => e.SCTO == scto);
                    if (userexits == null)
                    {
                        var _userid = db.fo_SCTO_ContactPerson.FirstOrDefault(e => e.UserId == userid);
                        if (_userid == null)
                        {
                            fo_SCTO_ContactPerson person = new fo_SCTO_ContactPerson();
                            person.SCTO = scto;
                            person.SCTO_Email = email;
                            person.SCTO_TelNo1 = Telephone;
                            person.SCTO_TelNo2 = Tel;
                            person.UserId = userid;
                            db.fo_SCTO_ContactPerson.Add(person);
                            try
                            {
                                db.SaveChanges();
                                result = scto + " Was saved successfully";
                            }
                            catch (Exception ex)
                            {
                                result = ex.Message.ToString();
                            }
                        }
                        else
                        {
                            result = "This user was already saved as " + _userid.SCTO;
                        }
                    }
                    else
                    {
                        result = "This SCTO name already exists";
                    }
                }
            }
            else if(_type == 2)
            {
                var userexits = db.fo_SCTO_ContactPerson.FirstOrDefault(e => e.SCTO == scto);
                if(userexits != null)
                {
                    fo_SCTO_ContactPerson person = new fo_SCTO_ContactPerson();
                    person.SCTO = scto;
                    person.SCTO_Email = email;
                    person.SCTO_TelNo1 = Telephone;
                    person.SCTO_TelNo2 = Tel;
                    person.UserId = userid;
                    try
                    {
                        db.Entry(userexits).CurrentValues.SetValues(person);
                        db.Entry(userexits).State = EntityState.Modified;
                        db.SaveChanges();
                        result = scto + " was updated successfully";
                    }
                    catch(Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
                
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogInsert(View_A_Facility value/*, string testdata, string testdata2, string testdata3*/)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var _feedback = db.A_Facilities.FirstOrDefault(f => f.FacilityCode  == value.FacilityCode);
            db.Configuration.ProxyCreationEnabled = false;

            A_Facilities a_Facilities = new A_Facilities();
            a_Facilities.CDCRegionId = value.CDCRegionId;
            a_Facilities.client_type_code = value.client_type_code;
            a_Facilities.ComprehensiveImplimentingPartnerCode = value.ComprehensiveImplimentingPartnerCode;
            a_Facilities.DeliveryZoneCode = value.DeliveryZoneCode;
            a_Facilities.DistrrictCode = value.DistrrictCode;
            a_Facilities.DSDM = value.DSDM;
            a_Facilities.Facility = value.Facility;
            a_Facilities.FacilityCode = value.FacilityCode;
            a_Facilities.FacilityTypeId = value.FacilityTypeId;
            a_Facilities.ImplimentingPartnerCode = value.ImplimentingPartnerCode;
            a_Facilities.IsAccredited = value.IsAccredited;
            a_Facilities.IsActive = value.IsActive;
            a_Facilities.Latititude = value.Latititude;
            a_Facilities.level_of_care = value.level_of_care;
            a_Facilities.Longtitude = value.Longtitude;
            a_Facilities.Nearest_Public_HF_Distance = value.Nearest_Public_HF_Distance;
            a_Facilities.OwnershipId = value.OwnershipId;
            a_Facilities.PatientLoadCode = value.PatientLoadCode;
            a_Facilities.region_code = value.region_code;
            a_Facilities.RFSO_UserName = value.RFSO_UserName;
            a_Facilities.SAP_Code = value.SAP_Code;
            a_Facilities.SupportedByMAUL = value.SupportedByMAUL;
            a_Facilities.Village_Id = value.Village_Id;
            a_Facilities.Active_ART = value.Active_ART;
            if (_feedback == null)
            {
                if (a_Facilities.ImplimentingPartnerCode == 0) { a_Facilities.ImplimentingPartnerCode = null; }
                if (a_Facilities.OwnershipId == 0) { a_Facilities.OwnershipId = null; }
                if (a_Facilities.level_of_care == 0) { a_Facilities.level_of_care = null; }
                if (a_Facilities.PatientLoadCode == 0) { a_Facilities.PatientLoadCode = null; }
                if (a_Facilities.client_type_code == 0) { a_Facilities.client_type_code = null; }
                if (a_Facilities.CDCRegionId == 0) { a_Facilities.CDCRegionId = null; }
                if (a_Facilities.ComprehensiveImplimentingPartnerCode == 0) { a_Facilities.ComprehensiveImplimentingPartnerCode = null; }
                if (a_Facilities.DistrrictCode == 0) { a_Facilities.DistrrictCode = null; }
                if(a_Facilities.DeliveryZoneCode == 0) { a_Facilities.DeliveryZoneCode = null; }
                var pcode = complaintCode();
                a_Facilities.FacilityCode  = ++pcode;
                if(string.IsNullOrEmpty(a_Facilities.Facility) || string.IsNullOrEmpty(a_Facilities.SAP_Code)) { }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.A_Facilities.Add(a_Facilities);
                }
                
            }
            else
            {
                var a = db.A_Facilities.FirstOrDefault(e => e.FacilityCode == value.FacilityCode);
                if (a.DeliveryZoneCode == value.DeliveryZoneCode)
                {
                    db.Entry(_feedback).CurrentValues.SetValues(a_Facilities);
                    db.Entry(_feedback).State = EntityState.Modified;
                }
                else
                {
                    A_Sector_Change sector_Change = new A_Sector_Change()
                    {
                        facility_code = value.FacilityCode,
                        date_of_change = DateTime.Now,
                        Old_sector = Convert.ToInt32(a.DeliveryZoneCode),
                        New_sector = value.DeliveryZoneCode
                    };
                    db.Configuration.ProxyCreationEnabled = false;
                    db.A_Sector_Change.Add(sector_Change);
                    db.SaveChanges();

                    db.Entry(_feedback).CurrentValues.SetValues(a_Facilities);
                    db.Entry(_feedback).State = EntityState.Modified;

                }
            }
            db.SaveChanges();
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCDCRegion(int district)
        {
            int? data = 0;
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.A_District.FirstOrDefault(o=> o.District_Code == district);
            if(result.CDCRegionId != null)
            {
                data = result.CDCRegionId;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIP(int district)
        {
            int? data = 0;
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.A_District.FirstOrDefault(o => o.District_Code == district);
            if (result.ImplimentingPartnerCode != null)
            {
                data = result.ImplimentingPartnerCode;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public int complaintCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var personcode = db.A_Facilities.OrderByDescending(o => o.FacilityCode ).Take(1).Select(f => f.FacilityCode).FirstOrDefault();
            return (personcode);
        }
        // GET: A_Facilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Facilities a_Facilities = db.A_Facilities.Find(id);
            if (a_Facilities == null)
            {
                return HttpNotFound();
            }
            return View(a_Facilities);
        }

        // GET: A_Facilities/Create
        public ActionResult Create()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.CDCRegionId = new SelectList(db.A_CDCRegion, "CDCRegionId", "CDCRegion");
            ViewBag.DeliveryZoneCode = new SelectList(db.A_DeliveryZone, "ZoneCode", "ZoneDescription");
            ViewBag.DistrrictCode = new SelectList(db.A_District, "District_Code", "District_Name");
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility");
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility");
            ViewBag.level_of_care = new SelectList(db.A_Facility_Level_Of_Care, "level_of_care_code", "level_of_care");
            ViewBag.FacilityTypeId = new SelectList(db.A_FacilityType, "FacilityTypeId", "FacilityType");
            ViewBag.ComprehensiveImplimentingPartnerCode = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription");
            ViewBag.OwnershipId = new SelectList(db.A_Ownership, "OwnershipId", "OwnershipDescription");
            return View();
        }

        // POST: A_Facilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacilityCode,DeliveryZoneCode,ImplimentingPartnerCode,DistrrictCode,Facility,SAP_Code,SupportedByMAUL,IsAccredited,level_of_care,client_type_code,region_code,RFSO_UserName,OwnershipId,CDCRegionId,FacilityTypeId,Longtitude,Latititude,Village_Id,ComprehensiveImplimentingPartnerCode,PatientLoadCode,IsActive,Nearest_Public_HF_Distance,DSDM,SCTO")] A_Facilities a_Facilities)
        {
            if (ModelState.IsValid)
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.A_Facilities.Add(a_Facilities);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.CDCRegionId = new SelectList(db.A_CDCRegion, "CDCRegionId", "CDCRegion", a_Facilities.CDCRegionId);
            ViewBag.DeliveryZoneCode = new SelectList(db.A_DeliveryZone, "ZoneCode", "ZoneDescription", a_Facilities.DeliveryZoneCode);
            ViewBag.DistrrictCode = new SelectList(db.A_District, "District_Code", "District_Name", a_Facilities.DistrrictCode);
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", a_Facilities.FacilityCode);
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", a_Facilities.FacilityCode);
            ViewBag.level_of_care = new SelectList(db.A_Facility_Level_Of_Care, "level_of_care_code", "level_of_care", a_Facilities.level_of_care);
            ViewBag.FacilityTypeId = new SelectList(db.A_FacilityType, "FacilityTypeId", "FacilityType", a_Facilities.FacilityTypeId);
            ViewBag.ComprehensiveImplimentingPartnerCode = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription", a_Facilities.ComprehensiveImplimentingPartnerCode);
            ViewBag.OwnershipId = new SelectList(db.A_Ownership, "OwnershipId", "OwnershipDescription", a_Facilities.OwnershipId);
            return View(a_Facilities);
        }

        // GET: A_Facilities/Edit/5
        public ActionResult Edit(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Facilities a_Facilities = db.A_Facilities.Find(id);
            if (a_Facilities == null)
            {
                return HttpNotFound();
            }
            ViewBag.CDCRegionId = new SelectList(db.A_CDCRegion, "CDCRegionId", "CDCRegion", a_Facilities.CDCRegionId);
            ViewBag.DeliveryZoneCode = new SelectList(db.A_DeliveryZone, "ZoneCode", "ZoneDescription", a_Facilities.DeliveryZoneCode);
            ViewBag.DistrrictCode = new SelectList(db.A_District, "District_Code", "District_Name", a_Facilities.DistrrictCode);
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", a_Facilities.FacilityCode);
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", a_Facilities.FacilityCode);
            ViewBag.level_of_care = new SelectList(db.A_Facility_Level_Of_Care, "level_of_care_code", "level_of_care", a_Facilities.level_of_care);
            ViewBag.FacilityTypeId = new SelectList(db.A_FacilityType, "FacilityTypeId", "FacilityType", a_Facilities.FacilityTypeId);
            ViewBag.ComprehensiveImplimentingPartnerCode = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription", a_Facilities.ComprehensiveImplimentingPartnerCode);
            ViewBag.OwnershipId = new SelectList(db.A_Ownership, "OwnershipId", "OwnershipDescription", a_Facilities.OwnershipId);
            return View(a_Facilities);
        }

        // POST: A_Facilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacilityCode,DeliveryZoneCode,ImplimentingPartnerCode,DistrrictCode,Facility,SAP_Code,SupportedByMAUL,IsAccredited,level_of_care,client_type_code,region_code,RFSO_UserName,OwnershipId,CDCRegionId,FacilityTypeId,Longtitude,Latititude,Village_Id,ComprehensiveImplimentingPartnerCode,PatientLoadCode,IsActive,Nearest_Public_HF_Distance,DSDM,SCTO")] A_Facilities a_Facilities)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (ModelState.IsValid)
            {
                db.Entry(a_Facilities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CDCRegionId = new SelectList(db.A_CDCRegion, "CDCRegionId", "CDCRegion", a_Facilities.CDCRegionId);
            ViewBag.DeliveryZoneCode = new SelectList(db.A_DeliveryZone, "ZoneCode", "ZoneDescription", a_Facilities.DeliveryZoneCode);
            ViewBag.DistrrictCode = new SelectList(db.A_District, "District_Code", "District_Name", a_Facilities.DistrrictCode);
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", a_Facilities.FacilityCode);
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", a_Facilities.FacilityCode);
            ViewBag.level_of_care = new SelectList(db.A_Facility_Level_Of_Care, "level_of_care_code", "level_of_care", a_Facilities.level_of_care);
            ViewBag.FacilityTypeId = new SelectList(db.A_FacilityType, "FacilityTypeId", "FacilityType", a_Facilities.FacilityTypeId);
            ViewBag.ComprehensiveImplimentingPartnerCode = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription", a_Facilities.ComprehensiveImplimentingPartnerCode);
            ViewBag.OwnershipId = new SelectList(db.A_Ownership, "OwnershipId", "OwnershipDescription", a_Facilities.OwnershipId);
            return View(a_Facilities);
        }

        // GET: A_Facilities/Delete/5
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Facilities a_Facilities = db.A_Facilities.Find(id);
            if (a_Facilities == null)
            {
                return HttpNotFound();
            }
            return View(a_Facilities);
        }

        // POST: A_Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            A_Facilities a_Facilities = db.A_Facilities.Find(id);
            db.A_Facilities.Remove(a_Facilities);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void ExportToExcel(string GridModel)
        {
           // IEnumerable datasource = db.A_Facilities.ToList();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            if(cat == "FacilityData")
            {
                currentData = db.View_A_Facility.ToList();
                obj.Columns[1].DataSource = db.A_DeliveryZone.ToList();
                obj.Columns[2].DataSource = db.A_ImplimentingPartners.ToList();
                obj.Columns[3].DataSource = db.A_District.ToList();
                obj.Columns[8].DataSource = db.A_Facility_Level_Of_Care.ToList();
                obj.Columns[9].DataSource = db.A_ClientType.ToList();
                obj.Columns[11].DataSource = db.A_Ownership.ToList();
                obj.Columns[12].DataSource = db.A_CDCRegion.ToList();
                obj.Columns[13].DataSource = db.A_FacilityType.ToList();
                obj.Columns[16].DataSource = db.A_ImplimentingPartners.ToList();
                obj.Columns[17].DataSource = db.A_PatientLoad.ToList();
                //obj.Columns[21].DataSource = db.fo_SCTO.ToList();
            }
            
            //GridProperties obj = (GridProperties)Syncfusion.JavaScript.Utils.DeserializeToModel(typeof(GridProperties), GridModel);
            exp.Export(obj, currentData, Module + ".xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
             
                if (ds.Key == "FacilityData")
                {
                    string str = Convert.ToString(ds.Value);
                    //currentData = JsonConvert.DeserializeObject<IEnumerable<View_A_Facility>>(str);
                    Module = "Facility details " + DateTime.Now.ToLongDateString();
                    cat = "FacilityData";
                    continue;
                }
                if (ds.Key == "SCTO")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<fo_SCTO_ContactPerson>>(str);
                    Module = "SCTO " + DateTime.Now.ToLongDateString();
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
    }
}
