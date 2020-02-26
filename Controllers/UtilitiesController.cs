using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mascis.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System.Web.Script.Serialization;
using System.Reflection;

namespace mascis.Controllers
{
    public class UtilitiesController : Controller
    {
        string Module;
        public IEnumerable currentData;
        mascisEntities context = new mascisEntities();
        string cat = string.Empty;
        // GET: Utilities
        public ActionResult Facilities()
        {
            return View();
        }
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult DrugRegimens()
        {
            return View();
        }
        public ActionResult CDCRegions()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var cdcregion = context.A_CDCRegion.ToList();
            ViewBag.datasource = cdcregion;
            return View();
        }
        public ActionResult GetCDCRegion()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var cdcregion = context.A_CDCRegion.ToList();
            return Json(cdcregion, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveCDCRegion(int ID, string Description,int _type)
        {
            string result = string.Empty;
            if(_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_CDCRegion.FirstOrDefault(e => e.CDCRegion.Trim() == Description.Trim());
                    if(desccheck == null)
                    {
                        A_CDCRegion region = new A_CDCRegion() { CDCRegionId = ID, CDCRegion = Description };
                        try
                        {
                            context.A_CDCRegion.Add(region);
                            context.SaveChanges();
                            result = Description +" has been saved successfully........";
                        }
                        catch(Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This CDC Region already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_CDCRegion.FirstOrDefault(e => e.CDCRegionId == ID);
                if (userexits != null)
                {
                    A_CDCRegion Region = new A_CDCRegion();
                    Region.CDCRegionId = ID;
                    Region.CDCRegion = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckCDCRegionNo()
        {
            var data = context.A_CDCRegion.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IMplementingPartners()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var implementing = context.A_ImplimentingPartners.AsNoTracking().ToArray();
            ViewBag.datasource = implementing;
            return View();
        }
        public ActionResult GetIMplementingPartners()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var implementing = context.A_ImplimentingPartners.ToList();
            return Json(implementing, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckIPNo()
        {
            var data = context.A_ImplimentingPartners.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveIP(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_ImplimentingPartners.FirstOrDefault(e => e.ImplementingPartnerDescription.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_ImplimentingPartners region = new A_ImplimentingPartners() { ImplimentingPartnerCode = ID, ImplementingPartnerDescription = Description };
                        try
                        {
                            context.A_ImplimentingPartners.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_ImplimentingPartners.FirstOrDefault(e => e.ImplimentingPartnerCode == ID);
                if (userexits != null)
                {
                    A_ImplimentingPartners Region = new A_ImplimentingPartners();
                    Region.ImplimentingPartnerCode = ID;
                    Region.ImplementingPartnerDescription = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClientType()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var Client = context.A_ClientType.AsNoTracking().ToArray();
            ViewBag.datasource = Client;
            return View();
        }
        public ActionResult GetClientType()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var Client = context.A_ClientType.ToList();
            return Json(Client, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckClientTypeNo()
        {
            var data = context.A_ClientType.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveClientType(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_ClientType.FirstOrDefault(e => e.client_type_desc.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_ClientType region = new A_ClientType() { client_type_code = ID, client_type_desc = Description };
                        try
                        {
                            context.A_ClientType.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_ClientType.FirstOrDefault(e => e.client_type_code == ID);
                if (userexits != null)
                {
                    A_ClientType Region = new A_ClientType();
                    Region.client_type_code = ID;
                    Region.client_type_desc = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FacilityType()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facilityType = context.A_FacilityType.AsNoTracking().ToArray();
            ViewBag.datasource = facilityType;
            return View();
        }
        public ActionResult GetFacilityType()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facilityType = context.A_FacilityType.ToList();
            return Json(facilityType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckFacilityTypeNo()
        {
            var data = context.A_FacilityType.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveFacilityType(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_FacilityType.FirstOrDefault(e => e.FacilityType.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        string id = string.Empty;
                        if(ID < 10)
                        {
                            id = "0" + ID;
                        }
                        else { id = ID.ToString(); }
                        A_FacilityType region = new A_FacilityType() { FacilityTypeId = id, FacilityType = Description };
                        try
                        {
                            context.A_FacilityType.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                string id = string.Empty;
                if (ID < 10)
                {
                    id = "0" + ID;
                }
                else { id = ID.ToString(); }
                var userexits = context.A_FacilityType.FirstOrDefault(e => e.FacilityTypeId == id);
                if (userexits != null)
                {
                    //A_FacilityType Region = new A_FacilityType();
                    //Region.FacilityType = id;
                    //Region.FacilityType = Description;
                    try
                    {
                        userexits.FacilityType = Description;
                        //context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryZone()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var Zone = context.A_DeliveryZone.AsNoTracking().ToArray();
            ViewBag.datasource = Zone;
            return View();
        }
        public ActionResult GetDeliveryZone()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var Zone = context.A_DeliveryZone.ToList();
            return Json(Zone, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckZoneNo()
        {
            var data = context.A_DeliveryZone.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDeliveryZone(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_DeliveryZone.FirstOrDefault(e => e.ZoneDescription.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_DeliveryZone region = new A_DeliveryZone() { ZoneCode = ID, ZoneDescription = Description };
                        try
                        {
                            context.A_DeliveryZone.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_DeliveryZone.FirstOrDefault(e => e.ZoneCode == ID);
                if (userexits != null)
                {
                    A_DeliveryZone Region = new A_DeliveryZone();
                    //Region.ZoneCode = ID;
                    Region.ZoneDescription = Description;
                    try
                    {
                        userexits.ZoneDescription = Description;
                        //context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LevelOfCare()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var Care = context.A_Facility_Level_Of_Care.AsNoTracking().ToArray();
            ViewBag.datasource = Care;
            return View();
        }
        public ActionResult GetLevelOfCare()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var Care = context.A_Facility_Level_Of_Care.ToList();
            return Json(Care, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckCareNo()
        {
            var data = context.A_Facility_Level_Of_Care.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveLevelOfCare(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_Facility_Level_Of_Care.FirstOrDefault(e => e.level_of_care.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_Facility_Level_Of_Care region = new A_Facility_Level_Of_Care() { level_of_care_code = ID, level_of_care = Description };
                        try
                        {
                            context.A_Facility_Level_Of_Care.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_Facility_Level_Of_Care.FirstOrDefault(e => e.level_of_care_code == ID);
                if (userexits != null)
                {
                    A_Facility_Level_Of_Care Region = new A_Facility_Level_Of_Care();
                    Region.level_of_care_code = ID;
                    Region.level_of_care = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ownership()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var ownership = context.A_Ownership.AsNoTracking().ToArray();
            ViewBag.datasource = ownership;
            return View();
        }
        public ActionResult GetOwnership()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var ownership = context.A_Ownership.ToList();
            return Json(ownership, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckOwnershipNo()
        {
            var data = context.A_Ownership.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOwnership(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_Ownership.FirstOrDefault(e => e.OwnershipDescription.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_Ownership region = new A_Ownership() { OwnershipId = ID, OwnershipDescription = Description };
                        try
                        {
                            context.A_Ownership.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_Ownership.FirstOrDefault(e => e.OwnershipId == ID);
                if (userexits != null)
                {
                    A_Ownership Region = new A_Ownership();
                    Region.OwnershipId = ID;
                    Region.OwnershipDescription = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Region()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var region = context.A_Region.AsNoTracking().ToArray();
            ViewBag.datasource = region;
            return View();
        }
        public ActionResult GetRegion()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var region = context.A_Region.ToList();
            return Json(region, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveRegion(string ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_Region.FirstOrDefault(e => e.Region_Description.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_Region region = new A_Region() { Region_Id = ID, Region_Description = Description };
                        try
                        {
                            context.A_Region.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_Region.FirstOrDefault(e => e.Region_Id == ID);
                if (userexits != null)
                {
                    A_Region Region = new A_Region();
                    Region.Region_Id = ID;
                    Region.Region_Description = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PatientLoad()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var patient = context.A_PatientLoad.AsNoTracking().ToArray();
            ViewBag.datasource = patient;
            return View();
        }
        public ActionResult GetPatientLoad()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var patient = context.A_PatientLoad.ToList();
            return Json(patient, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckPatientLoadNo()
        {
            var data = context.A_PatientLoad.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SavePatientLoad(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_PatientLoad.FirstOrDefault(e => e.PatientLoadDescription.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_PatientLoad region = new A_PatientLoad() { PatientLoadCode = ID, PatientLoadDescription = Description };
                        try
                        {
                            context.A_PatientLoad.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_PatientLoad.FirstOrDefault(e => e.PatientLoadCode == ID);
                if (userexits != null)
                {
                    A_PatientLoad Region = new A_PatientLoad();
                    Region.PatientLoadCode = ID;
                    Region.PatientLoadDescription = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Districts()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var cdcregion = context.A_CDCRegion.AsNoTracking().ToArray();
            ViewBag.CdcRegion = cdcregion;

            context.Configuration.ProxyCreationEnabled = false;
            var implementing = context.A_ImplimentingPartners.AsNoTracking().ToArray();
            ViewBag.Implementing = implementing;

            context.Configuration.ProxyCreationEnabled = false;
            var region = context.A_Region.AsNoTracking().ToArray();
            ViewBag.Region = region;

            context.Configuration.ProxyCreationEnabled = false;
            var district = context.A_District.AsNoTracking().ToArray();
            ViewBag.datasource = district;
            return View();
        }
        public ActionResult GetDistricts()
        {
            context.Configuration.ProxyCreationEnabled = false;
             var district = context.A_District.OrderBy(o=> o.District_Name).ToList();
            return Json(district, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckDistrictsNo()
        {
            var data = context.A_District.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDistrict(int DistrictCode,string DistrictName,string Region,int CDCRegion,int IP,string ISO_Code,string MinistryCode,bool? IsUban, bool? IsMunicipality, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(DistrictName))
                {
                    var desccheck = context.A_District.FirstOrDefault(e => e.District_Name.Trim() == DistrictName.Trim());
                    if (desccheck == null)
                    {
                        A_District region = new A_District() { District_Code = DistrictCode, District_Name = DistrictName,CDCRegionId = CDCRegion,ImplimentingPartnerCode=IP,Region_Id = Region,ISO_Code = ISO_Code,District_Ministry_Code = MinistryCode,Is_Urban = IsUban,Is_Municipality = IsMunicipality };
                        try
                        {
                            context.A_District.Add(region);
                            context.SaveChanges();
                            result = DistrictName + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_District.FirstOrDefault(e => e.District_Code == DistrictCode);
                if (userexits != null)
                {
                    A_District region = new A_District() { District_Code = DistrictCode, District_Name = DistrictName, CDCRegionId = CDCRegion, ImplimentingPartnerCode = IP, Region_Id = Region, ISO_Code = ISO_Code, District_Ministry_Code = MinistryCode, Is_Urban = IsUban, Is_Municipality = IsMunicipality };
                    try
                    {
                        userexits.District_Name = DistrictName;
                        userexits.CDCRegionId = CDCRegion;
                        userexits.District_Ministry_Code = MinistryCode;
                        userexits.ISO_Code = ISO_Code;
                        userexits.Region_Id = Region;
                        userexits.Is_Urban = IsUban;
                        userexits.Is_Municipality = IsMunicipality;
                        userexits.ImplimentingPartnerCode = IP;
                        //context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = DistrictName + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Regimen()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var category = context.A_DrugRegimenCategory.AsNoTracking().ToArray();
            ViewBag.RegCategory = category;

            context.Configuration.ProxyCreationEnabled = false;
            var classification = context.A_DrugRegimenClassification.AsNoTracking().ToArray();
            ViewBag.RegClassification = classification;

            context.Configuration.ProxyCreationEnabled = false;
            var Regmen = context.A_DrugRegimen.AsNoTracking().ToArray();
            ViewBag.datasource = Regmen;
            return View();
        }
        public ActionResult GetRegimen(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.A_DrugRegimen.ToList();
            context.Configuration.ProxyCreationEnabled = false;
            int count = context.A_DrugRegimen.ToList().Count;

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
        public ActionResult DeleteRegimen(int value)
        {
            var PatientSummary = context.Treatment_PatientSummary.Where(o => o.RegimenCode == value).ToList();

            var regmen = context.A_DrugRegimen.FirstOrDefault(o => o.RegimenCode == value);
            var desccheck = context.A_DrugRegimen.Where(e => (e.RegimenDesc.Trim() == regmen.RegimenDesc.Trim()) && e.RegimenClassification == regmen.RegimenClassification && e.RegimenCategoryCode == regmen.RegimenCategoryCode).ToList();
            if (PatientSummary.Count > 0 && desccheck.Count <= 1)
            {
                return Json("Regimen Cannot be deleted because it is being referenced in other tables", JsonRequestBehavior.AllowGet);
                
            }
            else
            {
                foreach (var n in PatientSummary)
                {
                    context.Treatment_PatientSummary.Remove(n);
                    context.SaveChanges();
                }
                context.A_DrugRegimen.Remove(regmen);
                context.SaveChanges();
                return Json("Successfully deleted", JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult SaveRegimen(int RegimenCode, string RegimenDesc, string ExtraInfo, string DrugCombination, int RegimenCategoryCode, int RegimenClassification, bool? StandardRegimen, bool? IsExtra, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(RegimenDesc))
                {
                    var desccheck = context.A_DrugRegimen.FirstOrDefault(e => (e.RegimenDesc.Trim() == RegimenDesc.Trim() || e.RegimenCode ==RegimenCode) && e.RegimenClassification == RegimenClassification && e.RegimenCategoryCode == RegimenCategoryCode);
                    if (desccheck == null)
                    {
                        A_DrugRegimen region = new A_DrugRegimen() { RegimenCode = Convert.ToInt16(RegimenCode), RegimenDesc = RegimenDesc, ExtraInfo = ExtraInfo, DrugCombination = DrugCombination, RegimenCategoryCode = Convert.ToInt16(RegimenCategoryCode), RegimenClassification = RegimenClassification, StandardRegimen = StandardRegimen, IsExtra = IsExtra };
                        try
                        {
                            context.A_DrugRegimen.Add(region);
                            context.SaveChanges();
                            result = RegimenDesc + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Regimen already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_DrugRegimen.FirstOrDefault(e => e.RegimenCode == RegimenCode);
                if (userexits != null)
                {
                    //A_District region = new A_District() { District_Code = DistrictCode, District_Name = DistrictName, CDCRegionId = CDCRegion, ImplimentingPartnerCode = IP, Region_Id = Region, ISO_Code = ISO_Code, District_Ministry_Code = MinistryCode, Is_Urban = IsUban, Is_Municipality = IsMunicipality };
                    try
                    {
                        userexits.RegimenDesc = RegimenDesc;
                        userexits.ExtraInfo = ExtraInfo;
                        userexits.DrugCombination = DrugCombination;
                        userexits.RegimenCategoryCode = Convert.ToInt16(RegimenCategoryCode);
                        userexits.RegimenClassification = RegimenClassification;
                        userexits.StandardRegimen = StandardRegimen;
                        userexits.IsExtra = IsExtra;
                        //context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = RegimenDesc + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductCategory()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var category = context.A_product_category.AsNoTracking().ToArray();
            ViewBag.datasource = category;
            return View();
        }
        public ActionResult GetProductCategory()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var category = context.A_product_category.ToList();
            return Json(category, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckProductCategoryNo()
        {
            var data = context.A_product_category.ToList();
            int count = data.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveProductCategory(int ID, string Description, int _type)
        {
            string result = string.Empty;
            if (_type == 1)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var desccheck = context.A_product_category.FirstOrDefault(e => e.category_desc.Trim() == Description.Trim());
                    if (desccheck == null)
                    {
                        A_product_category region = new A_product_category() { category_code = ID, category_desc = Description };
                        try
                        {
                            context.A_product_category.Add(region);
                            context.SaveChanges();
                            result = Description + " has been saved successfully........";
                        }
                        catch (Exception ex)
                        {
                            result = "Error \n" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        result = "This Implimenting Partners already exists";
                    }
                }
                else
                {
                    result = "PLease fill in the description";
                }
            }
            else if (_type == 2)
            {
                var userexits = context.A_product_category.FirstOrDefault(e => e.category_code == ID);
                if (userexits != null)
                {
                    A_product_category Region = new A_product_category();
                    Region.category_code = ID;
                    Region.category_desc = Description;
                    try
                    {
                        context.Entry(userexits).CurrentValues.SetValues(Region);
                        context.Entry(userexits).State = EntityState.Modified;
                        context.SaveChanges();
                        result = Description + " was updated successfully";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message.ToString();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Counties()
        {
            return View();
        }
        public ActionResult Subcounties()
        {
            return View();
        }
        public ActionResult Parishes()
        {
            return View();
        }
        public ActionResult Villages()
        {
            return View();
        }
        public ActionResult ManageRFSOFacility()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var cdcregion = context.A_CDCRegion.AsNoTracking().ToArray();
            ViewBag.CDCRegionId = cdcregion;

            context.Configuration.ProxyCreationEnabled = false;
            var deliveryzone = context.A_DeliveryZone.AsNoTracking().ToArray();
            ViewBag.DeliveryZoneCode = deliveryzone;

            context.Configuration.ProxyCreationEnabled = false;
            var districts = context.A_District.AsNoTracking().ToArray();
            ViewBag.DistrrictCode = districts;

            //var facilities = context.A_Facilities.AsNoTracking().ToArray();
            //ViewBag.FacilityCode = facilities;

            context.Configuration.ProxyCreationEnabled = false;
            var level_of_Care = context.A_Facility_Level_Of_Care.AsNoTracking().ToArray();
            ViewBag.level_of_care = level_of_Care;

            context.Configuration.ProxyCreationEnabled = false;
            var FacilityTypes = context.A_FacilityType.AsNoTracking().ToArray();
            ViewBag.FacilityTypeId = FacilityTypes;

            context.Configuration.ProxyCreationEnabled = false;
            var ImplimentingPartners = context.A_ImplimentingPartners.AsNoTracking().ToArray();
            ViewBag.ComprehensiveImplimentingPartnerCode = ImplimentingPartners;

            var ownerships = context.A_Ownership.AsNoTracking().ToArray();
            ViewBag.OwnershipId = ownerships;

            var client = context.A_ClientType.AsNoTracking().ToArray();
            ViewBag.client_type_code = client;

            context.Configuration.ProxyCreationEnabled = false;
            var users = context.AspNetUsers.AsNoTracking().ToArray();
            ViewBag.Users = users;

            context.Configuration.ProxyCreationEnabled = false;
            var patientloads = context.A_PatientLoad.AsNoTracking().ToArray();
            ViewBag.PatientLoads = patientloads;

            context.Configuration.ProxyCreationEnabled = false;
            var scto = context.fo_SCTO_ContactPerson.AsNoTracking().ToArray();
            ViewBag.SCTO = scto;
            context.Configuration.ProxyCreationEnabled = false;
            var RFSO = context.View_WebTemplate_FacilityRFSO_2.AsNoTracking();
            ViewBag.RFSO = RFSO;
            return View();
        }
        public ActionResult DataSourceRFSO(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.View_WebTemplate_FacilityRFSO.OrderBy(o=>o.Facility).ToList();
            int count = context.View_WebTemplate_FacilityRFSO.Count();

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
        public ActionResult BatchUpdate(string key, List<A_Facilities> changed, List<A_Facilities> added, List<A_Facilities> deleted)
        {
            mascisEntities dbCase = new mascisEntities();
            FacilityModel obj = new FacilityModel();
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    //dbCase.A_Facilities.Add(temp);
                    obj.FacilityCode = temp.FacilityCode;
                    obj.RFSO_UserName = temp.RFSO_UserName;
                    obj.DeliveryZoneCode = temp.DeliveryZoneCode;
                    obj.ImplimentingPartnerCode = temp.ImplimentingPartnerCode;
                    obj.DistrrictCode = temp.DistrrictCode;
                    obj.Facility = temp.Facility;
                    obj.SAP_Code = temp.SAP_Code;
                    obj.SupportedByMAUL = temp.SupportedByMAUL;
                    obj.IsAccredited = temp.IsAccredited;
                    obj.level_of_care = temp.level_of_care;
                    obj.client_type_code = temp.client_type_code;
                    obj.region_code = temp.region_code;
                    obj.OwnershipId = temp.OwnershipId;
                    obj.CDCRegionId = temp.CDCRegionId;
                    obj.FacilityTypeId = temp.FacilityTypeId;
                    obj.Longtitude = temp.Longtitude;
                    obj.Latititude = temp.Latititude;
                    obj.Village_Id = temp.Village_Id;
                    obj.ComprehensiveImplimentingPartnerCode = temp.ComprehensiveImplimentingPartnerCode;
                    obj.PatientLoadCode = temp.PatientLoadCode;
                    obj.IsActive = temp.IsActive;
                    obj.Nearest_Public_HF_Distance = temp.Nearest_Public_HF_Distance;
                    obj.DSDM = temp.DSDM;

                    if (!string.IsNullOrEmpty(temp.RFSO_UserName))
                    {
                        string id = context.AspNetUsers.FirstOrDefault(e => e.UserName == temp.RFSO_UserName).Id;
                        if (!string.IsNullOrEmpty(id))
                        {
                            var mScto = context.fo_SCTO_ContactPerson.FirstOrDefault(e => e.UserId == id);
                            if (mScto != null)
                            {
                                string scto = mScto.SCTO;
                                if (!string.IsNullOrEmpty(scto))
                                {
                                    var isAvailable = context.fo_SCTO.FirstOrDefault(e => e.SCTO == scto && e.SAP_Code == temp.SAP_Code);
                                    if (isAvailable == null)
                                    {
                                        var check = context.fo_SCTO.FirstOrDefault(e => e.SAP_Code == temp.SAP_Code);
                                        if (check == null)
                                        {
                                            fo_SCTO _SCTO = new fo_SCTO() { SCTO = scto, SAP_Code = temp.SAP_Code };
                                            context.fo_SCTO.Add(_SCTO);
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            fo_SCTO _SCTO = new fo_SCTO() { SCTO = scto, SAP_Code = temp.SAP_Code };
                                            context.Entry(check).CurrentValues.SetValues(_SCTO);
                                            context.Entry(check).State = EntityState.Modified;
                                            context.SaveChanges();
                                        }
                                    }

                                }
                            }
                        }
                    }
                        
                }
            }
            obj.Update();

            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {

                    //A_Facilities old = dbCase.A_Facilities.
                    //    FirstOrDefault(o => o.FacilityCode == temp.FacilityCode);
                    //if (old != null)
                    //{
                        //dbCase.Entry(old).CurrentValues.SetValues(temp);
                        obj.FacilityCode = temp.FacilityCode;
                        obj.RFSO_UserName = temp.RFSO_UserName;
                        obj.DeliveryZoneCode = temp.DeliveryZoneCode;
                        obj.ImplimentingPartnerCode = temp.ImplimentingPartnerCode;
                        obj.DistrrictCode = temp.DistrrictCode;
                        obj.Facility = temp.Facility;
                        obj.SAP_Code = temp.SAP_Code;
                        obj.SupportedByMAUL = temp.SupportedByMAUL;
                        obj.IsAccredited = temp.IsAccredited;
                        obj.level_of_care = temp.level_of_care;
                        obj.client_type_code = temp.client_type_code;
                        obj.region_code = temp.region_code;
                        obj.OwnershipId = temp.OwnershipId;
                        obj.CDCRegionId = temp.CDCRegionId;
                        obj.FacilityTypeId = temp.FacilityTypeId;
                        obj.Longtitude = temp.Longtitude;
                        obj.Latititude = temp.Latititude;
                        obj.Village_Id = temp.Village_Id;
                        obj.ComprehensiveImplimentingPartnerCode = temp.ComprehensiveImplimentingPartnerCode;
                        obj.PatientLoadCode = temp.PatientLoadCode;
                        obj.IsActive = temp.IsActive;
                        obj.Nearest_Public_HF_Distance = temp.Nearest_Public_HF_Distance;
                        obj.DSDM = temp.DSDM;
                    //obj.Update();
                    // }
                    //else
                    //{
                    //    obj.FacilityCode = temp.FacilityCode;
                    //    obj.RFSO_UserName = temp.RFSO_UserName;
                    //    obj.DeliveryZoneCode = temp.DeliveryZoneCode;
                    //    obj.ImplimentingPartnerCode = temp.ImplimentingPartnerCode;
                    //    obj.DistrrictCode = temp.DistrrictCode;
                    //    obj.Facility = temp.Facility;
                    //    obj.SAP_Code = temp.SAP_Code;
                    //    obj.SupportedByMAUL = temp.SupportedByMAUL;
                    //    obj.IsAccredited = temp.IsAccredited;
                    //    obj.level_of_care = temp.level_of_care;
                    //    obj.client_type_code = temp.client_type_code;
                    //    obj.region_code = temp.region_code;
                    //    obj.OwnershipId = temp.OwnershipId;
                    //    obj.CDCRegionId = temp.CDCRegionId;
                    //    obj.FacilityTypeId = temp.FacilityTypeId;
                    //    obj.Longtitude = temp.Longtitude;
                    //    obj.Latititude = temp.Latititude;
                    //    //obj.Update();
                    //}
                    if (!string.IsNullOrEmpty(temp.RFSO_UserName))
                    {
                        string id = context.AspNetUsers.FirstOrDefault(e => e.UserName == temp.RFSO_UserName).Id;
                        if (!string.IsNullOrEmpty(id))
                        {
                            var mScto = context.fo_SCTO_ContactPerson.FirstOrDefault(e => e.UserId == id);
                            if(mScto != null)
                            {
                                string scto = mScto.SCTO;
                                if (!string.IsNullOrEmpty(scto))
                                {
                                    var isAvailable = context.fo_SCTO.FirstOrDefault(e => e.SCTO == scto && e.SAP_Code == temp.SAP_Code);
                                    if (isAvailable == null)
                                    {
                                        var check = context.fo_SCTO.FirstOrDefault(e => e.SAP_Code == temp.SAP_Code);
                                        if(check == null)
                                        {
                                            fo_SCTO _SCTO = new fo_SCTO() { SCTO = scto, SAP_Code = temp.SAP_Code };
                                            context.fo_SCTO.Add(_SCTO);
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            fo_SCTO _SCTO = new fo_SCTO() { SCTO = scto, SAP_Code = temp.SAP_Code };
                                            context.Entry(check).CurrentValues.SetValues(_SCTO);
                                            context.Entry(check).State = EntityState.Modified;
                                            context.SaveChanges();
                                        }
                                        
                                    }

                                }
                            }
                            
                        }
                    }
                    
                }
            }
            //dbCase.SaveChanges();
            obj.Update();
            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    dbCase.A_Facilities.Remove(dbCase.A_Facilities.FirstOrDefault(o => o.FacilityCode == temp.FacilityCode));
                }
            }

            //dbCase.SaveChanges();
            obj.Update();
            return RedirectToAction("DataSourceRFSO");
        }
        public ActionResult DialogUpdate(A_Facilities value)
        {
            FacilityModel obj = new FacilityModel();
            obj.FacilityCode = value.FacilityCode;
            obj.RFSO_UserName = value.RFSO_UserName;
            obj.Update();
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public void ExportToExcel(string GridModel)
        {
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            if(cat == "Regimen")
            {
                obj.Columns[4].DataSource = context.A_DrugRegimenCategory.ToList();
                obj.Columns[5].DataSource = context.A_DrugRegimenClassification.ToList();
                //obj.Columns[3].DataSource = context.A_District.ToList();
            }
            else if(cat == "Districts")
            {
                obj.Columns[2].DataSource = context.A_Region.ToList();
                obj.Columns[7].DataSource = context.A_CDCRegion.ToList();
                obj.Columns[8].DataSource = context.A_ImplimentingPartners.ToList();
            }
            else if(cat == "RFSOFacility")
            {
                currentData = context.View_WebTemplate_FacilityRFSO.OrderBy(o => o.Facility).ToList();
                obj.Columns[6].DataSource = context.A_ClientType.ToList();
                obj.Columns[7].DataSource = context.A_Ownership.ToList();
                obj.Columns[8].DataSource = context.A_ImplimentingPartners.ToList();
                obj.Columns[9].DataSource = context.View_WebTemplate_FacilityRFSO_2.ToList();
                obj.Columns[10].DataSource = context.A_CDCRegion.ToList();
                obj.Columns[11].DataSource = context.A_FacilityType.ToList();
                //obj.Columns[14].DataSource = context.village.ToList();
                obj.Columns[15].DataSource = context.A_PatientLoad.ToList();

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
                if (ds.Key == "CDCRegions")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_CDCRegion>>(str);
                    Module = "CDC Regions  " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "IMplementingPartners")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_ImplimentingPartners>>(str);
                    Module = "Implementing partners " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "ClientType")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_ClientType>>(str);
                    Module = "Client Type " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "FacilityType")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_FacilityType>>(str);
                    Module = "Facility Type " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "DeliveryZone")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_DeliveryZone>>(str);
                    Module = "Delivery Zone " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "LevelOfCare")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_Facility_Level_Of_Care>>(str);
                    Module = "Level Of Care " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "Ownership")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_Ownership>>(str);
                    Module = "Ownership " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "Region")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_Region>>(str);
                    Module = "Region " + DateTime.Now.ToLongDateString();
                   
                    continue;
                }
                if (ds.Key == "PatientLoad")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_PatientLoad>>(str);
                    Module = "Patient Load " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "Districts")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_District>>(str);
                    Module = "Districts " + DateTime.Now.ToLongDateString();
                    cat = "Districts";
                    continue;
                }
                if (ds.Key == "Regimen")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_DrugRegimen>>(str);
                    Module = "Regimen " + DateTime.Now.ToLongDateString();
                    cat = "Regimen";
                    continue;
                }
                if (ds.Key == "ProductCategory")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<A_product_category>>(str);
                    Module = "Product Category" + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "RFSOFacility")
                {
                    string str = Convert.ToString(ds.Value);
                    //currentData = JsonConvert.DeserializeObject<IEnumerable<Chss_M_Stock_Management>>(str);
                    Module = "Manage SCTO Facility " + DateTime.Now.ToLongDateString();
                    cat = "RFSOFacility";
                    continue;
                }
                if (ds.Key == "CareAvailabilityData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ART_Tools_Availability>>(str);
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
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ART_Treatment>>(str);
                    Module = "CHSS ART Patient Care and Management Initiation of adolscent to treatment " + DateTime.Now.ToLongDateString();
                    continue;
                }
                if (ds.Key == "PaediatricData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<ART_Treatment>>(str);
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
                    currentData = JsonConvert.DeserializeObject<IEnumerable<Scoring_Summary>>(str);
                    Module = "CHSS Scoring summary " + DateTime.Now.ToLongDateString();
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