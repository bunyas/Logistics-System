using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Syncfusion.JavaScript.DataVisualization.Maps;
using Syncfusion.JavaScript.DataVisualization.Models;
using mascis;
 
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

namespace SyncfusionMvcApplication1
{
    public partial class MapsController : Controller
    {


        public ActionResult FacilityMap()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MapConnection"].ConnectionString;
            DataTable dt = new DataTable();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            //local
            //using (SqlConnection con = new SqlConnection(@"Password=root85;Persist Security Info=True;User ID=sa;Initial Catalog=MEDICALACCESS;Data Source=."))

            //Server
            //using (SqlConnection con = new SqlConnection(@"Password=root85;Persist Security Info=True;User ID=sa;Initial Catalog=MEDICALACCESS;Data Source=MEDACCKAMSAP03\MSSQL2017"))
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spView_Facility_GIS_CodinatesGetAll";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    SqlParameter[] sqlParameters = new SqlParameter[1];

                    sqlParameters[0] = new SqlParameter("@FacilityCode", SqlDbType.Int);
                    sqlParameters[0].IsNullable = true;

                    //if (wdd_Facility.SelectedValue != "")
                    //    sqlParameters[0].Value = wdd_Facility.SelectedValue;
                    //else
                    sqlParameters[0].Value = DBNull.Value;
                    sqlParameters[0].Direction = ParameterDirection.Input;


                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(sqlParameters);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    //return serializer.Serialize(rows);
                }
            }
            ViewBag.GISdata = rows;// Syncfusion_LocationData.GetSyncfusionLocationData();

            return View();
        }
        //
        // GET: /DaraMarker/
        public ActionResult MapsFeatures()
        {

            ViewData["datasource"] = Syncfusion_LocationData.GetSyncfusionLocationData();
            ViewData["mapdata"] = this.GetWroldMap();
            return View();
        }
        //FacilityMap
        //public ActionResult FacilityMap()
        //{
        //    ViewData["datasource"] = Syncfusion_LocationData.GetSyncfusionLocationData();
        //    return View();
        //}
        public ActionResult ConvertDataTabletoString()
        { 
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            mascisEntities context = new mascisEntities();
                var x = context.A_Facilities.AsNoTracking().ToList();
                List<MapMarker> syncfusionLocationData = new List<MapMarker>();
                foreach (var n in x)
                {
                    var district = context.A_District.FirstOrDefault(e => e.District_Code == n.DistrrictCode);
                    var DZ = context.A_DeliveryZone.FirstOrDefault(e => e.ZoneCode == n.DeliveryZoneCode);
                    var CP = context.A_ImplimentingPartners.FirstOrDefault(e => e.ImplimentingPartnerCode == n.ComprehensiveImplimentingPartnerCode);
                    var CDC = context.A_CDCRegion.FirstOrDefault(e => e.CDCRegionId == n.CDCRegionId);
                    var CT = context.A_ClientType.FirstOrDefault(e => e.client_type_code == n.client_type_code);
                    var LOC = context.A_Facility_Level_Of_Care.FirstOrDefault(e => e.level_of_care_code == n.level_of_care);

                    LocationData msyncfusionLocationData = new LocationData()
                    {
                        Name = n.Facility,
                        Latitude = Convert.ToDouble(n.Latititude),
                        Longitude = Convert.ToDouble(n.Longtitude),

                    };
                    if (district != null)
                    {
                        msyncfusionLocationData.District = district.District_Name;
                    }
                    if (LOC != null)
                    {
                        msyncfusionLocationData.LevelOfCare = LOC.level_of_care;
                    }
                    if (CT != null)
                    {
                        msyncfusionLocationData.CleintType = CT.client_type_desc;
                    }
                    if (CDC != null)
                    {
                        msyncfusionLocationData.CDCRegion = CDC.CDCRegion;
                    }
                    if (CP != null)
                    {
                        msyncfusionLocationData.IP = CP.ImplementingPartnerDescription;
                    }
                    if (DZ != null)
                    {
                        msyncfusionLocationData.Sector = DZ.ZoneDescription;
                    }
                    syncfusionLocationData.Add(msyncfusionLocationData);

                }
                //List<MapMarker> syncfusionLocationData = new List<MapMarker>
                //{
                //    new LocationData {Name = "Chennai", Country = "India", Latitude =13.0839 , Longitude = 80.27 , Description = "Syncfusion's branch office is located in AnnaNagar, Chennai", Address ="EYMARD Complex AJ 217 4th Avenue Shanthi Colony Anna Nagar Chennai-40 India" },
                //    new LocationData {Name = "North Carolina", Country = "United States", Latitude =35.5 , Longitude = -80 , Description = "Syncfusion's corporate office is located in Research Triangle Park North Carolina", Address ="Company Headquarters 2501 Aerial Center Parkway Suite 200 Morrisville NC 27560 USA" },
                //};
               // return serializer.Serialize(syncfusionLocationData);
            return Json(syncfusionLocationData, JsonRequestBehavior.AllowGet);//syncfusionLocationData;

        }
        public object GetWroldMap()
        {
            string allText = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/Uganda.json"));
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            return new MapData(allText);
        }
    }
        public class Syncfusion_LocationData
        {
       
            public static List<MapMarker> GetSyncfusionLocationData()
            {
            mascisEntities context = new mascisEntities();
            var x = context.A_Facilities.AsNoTracking().ToList();
            List<MapMarker> syncfusionLocationData = new List<MapMarker>();
            foreach (var n in x)
            {
                var district = context.A_District.FirstOrDefault(e => e.District_Code == n.DistrrictCode);
                var DZ = context.A_DeliveryZone.FirstOrDefault(e => e.ZoneCode == n.DeliveryZoneCode);
                var CP = context.A_ImplimentingPartners.FirstOrDefault(e => e.ImplimentingPartnerCode == n.ComprehensiveImplimentingPartnerCode);
                var CDC = context.A_CDCRegion.FirstOrDefault(e => e.CDCRegionId == n.CDCRegionId);
                var CT = context.A_ClientType.FirstOrDefault(e => e.client_type_code == n.client_type_code);
                var LOC = context.A_Facility_Level_Of_Care.FirstOrDefault(e => e.level_of_care_code == n.level_of_care);

                LocationData msyncfusionLocationData = new LocationData()
                 {
                    Name = n.Facility,
                    Latitude = Convert.ToDouble(n.Latititude),
                    Longitude = Convert.ToDouble(n.Longtitude),
                    
                 };
                if (district != null)
                {
                    msyncfusionLocationData.District = district.District_Name;
                }
                if (LOC != null)
                {
                    msyncfusionLocationData.LevelOfCare = LOC.level_of_care;
                }
                if (CT != null)
                {
                    msyncfusionLocationData.CleintType = CT.client_type_desc;
                }
                if (CDC != null)
                {
                    msyncfusionLocationData.CDCRegion = CDC.CDCRegion;
                }
                if (CP != null)
                {
                    msyncfusionLocationData.IP = CP.ImplementingPartnerDescription;
                }
                if (DZ != null)
                {
                    msyncfusionLocationData.Sector = DZ.ZoneDescription;
                }
                syncfusionLocationData.Add(msyncfusionLocationData);

            }
                //List<MapMarker> syncfusionLocationData = new List<MapMarker>
                //{
                //    new LocationData {Name = "Chennai", Country = "India", Latitude =13.0839 , Longitude = 80.27 , Description = "Syncfusion's branch office is located in AnnaNagar, Chennai", Address ="EYMARD Complex AJ 217 4th Avenue Shanthi Colony Anna Nagar Chennai-40 India" },
                //    new LocationData {Name = "North Carolina", Country = "United States", Latitude =35.5 , Longitude = -80 , Description = "Syncfusion's corporate office is located in Research Triangle Park North Carolina", Address ="Company Headquarters 2501 Aerial Center Parkway Suite 200 Morrisville NC 27560 USA" },
                //};
                return syncfusionLocationData;
            }
       }
    public class LocationData : MapMarker
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string district;
        public string District
        {
            get { return district; }
            set { district = value; }
        }
        private string levelOfCare;
        public string LevelOfCare
        {
            get { return levelOfCare; }
            set { levelOfCare = value; }
        }
        private string client;
        public string CleintType
        {
            get { return client; }
            set { client = value; }
        }
        private string cdc;
        public string CDCRegion
        {
            get { return cdc; }
            set { cdc = value; }
        }
        private string cp;
        public string IP
        {
            get { return cp; }
            set { cp = value; }
        }
        private string sector;
        public string Sector
        {
            get { return sector; }
            set { sector = value; }
        }
    }
 }
