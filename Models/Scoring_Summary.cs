using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class Scoring_Summary
    {
        public int facility_code { get; set; }
        public string SAP_Code { get; set; }
        public string Facility { get; set; }
        public Nullable<int> ImplimentingPartnerCode { get; set; }
        public string ImplimentingPartnerDescription { get; set; }
        public Nullable<int> DistrrictCode { get; set; }
        public string District_Name { get; set; }
        public Nullable<int> level_of_care { get; set; }
        public string Level_desc { get; set; }
        public Nullable<int> CDCRegionId { get; set; }
        public string CDCRegion { get; set; }
        public Nullable<int> ComprehensiveImplimentingPartnerCode { get; set; }
        public string ComprehensiveImplimentingPartnerDescription { get; set; }
        public string SCTO { get; set; }
        public System.DateTime date_of_visit { get; set; }
        public string ART_Availability { get; set; }
        public string ART_DispensingLog { get; set; }
        public string ART_Register { get; set; }
        public string ART_Treatment { get; set; }
        public double ART_Total { get; set; }
        public double ART_Spars_Total { get; set; }
        public string Stock_Card { get; set; }
        public string Physical_Count { get; set; }
        public string Stock_Card_Fill { get; set; }
        public string Updating_Stock_Card { get; set; }
        public string Stock_Book_Use { get; set; }
        public string AMC { get; set; }
        public double Stock_Total { get; set; }
        public double Stock_Spars_Total { get; set; }
        public string Requisition_System { get; set; }
        public string Supplier { get; set; }
        public string Facility_Store { get; set; }
        public string Issued_to_Patients { get; set; }
        public double Traceability_Total { get; set; }
        public double Traceability_Spars_Total { get; set; }
        public string Report_Availability { get; set; }
        public string Order_Accuracy { get; set; }
        public string Patient_report { get; set; }
        public string Test_Kits { get; set; }
        public double Order_Total { get; set; }
        public double Order_Spars_Total { get; set; }
        public string Cleanliness { get; set; }
        public string Storage_System { get; set; }
        public string Storage_Condition { get; set; }
        public string Storage_Practice { get; set; }
        public double Store_Total { get; set; }
        public double Store_Spars_Total { get; set; }
        public string FEFO_Use { get; set; }
        public string Expiry_Presence { get; set; }
        public string Tacking_tools { get; set; }
        public double Expiry_Total { get; set; }
        public double Expiry_Spars_Total { get; set; }
        public double Final_Total { get; set; }
        public double Final_Spars_Total { get; set; }
        public string Score { get; set; }
        public string Spars_Score { get; set; }

    }
}