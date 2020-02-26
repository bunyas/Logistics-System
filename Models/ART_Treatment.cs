using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class ART_Treatment
    {
        public int facility_code { get; set; }
        public string SAP_Code { get; set; }
        public string Facility { get; set; }
        public Nullable<int> DistrrictCode { get; set; }
        public string District_Name { get; set; }
        public Nullable<int> ImplimentingPartnerCode { get; set; }
        public string ImplimentingPartnerDescription { get; set; }
        public Nullable<int> ComprehensiveImplimentingPartnerCode { get; set; }
        public string ComprehensiveImplimentingPartnerDescription { get; set; }
        public Nullable<int> level_of_care { get; set; }
        public string Level_desc { get; set; }
        public Nullable<int> CDCRegionId { get; set; }
        public string CDCRegion { get; set; }
        public string SCTO { get; set; }
        public System.DateTime date_of_visit { get; set; }
        public int regimen_classification_code { get; set; }
        public string regimen_classification_desc { get; set; }
        public string Drug_1 { get; set; }
        public int Score_1 { get; set; }
        public string Drug_2 { get; set; }
        public int Score_2 { get; set; }
        public string Drug_3 { get; set; }
        public int Score_3 { get; set; }
        public string Drug_4 { get; set; }
        public int Score_4 { get; set; }
        public string Drug_5 { get; set; }
        public int Score_5 { get; set; }
        public string Drug_6 { get; set; }
        public int Score_6 { get; set; }
        public string Drug_7 { get; set; }
        public int Score_7 { get; set; }
        public string Drug_8 { get; set; }
        public int Score_8 { get; set; }
        public string Drug_9 { get; set; }
        public int Score_9 { get; set; }
        public string Drug_10 { get; set; }
        public int Score_10 { get; set; }
        public int Total_Score { get; set; }
    }
}