using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class ARTPatients
    {
        public int facility_code { get; set; }
        public string SAP_Code { get; set; }
        public string Facility { get; set; }
        public Nullable<int> DistrrictCode { get; set; }
        public string District_Name { get; set; }
        public Nullable<int> ImplimentingPartnerCode { get; set; }
        public string ImplimentingPartnerDescription { get; set; }
        public Nullable<int> level_of_care { get; set; }
        public string Level_Desc { get; set; }
        public Nullable<int> CDCRegionId { get; set; }
        public string CDCRegion { get; set; }
        public Nullable<int> ComprehensiveImplimentingPartnerCode { get; set; }
        public string ComprehensiveImplimentingPartnerDescription { get; set; }
        public string SCTO { get; set; }
        public System.DateTime date_of_visit { get; set; }
        public Nullable<int> Q_one { get; set; }
        public Nullable<int> Q_two { get; set; }
        public Nullable<int> Q_three { get; set; }
        public Nullable<int> Q_four { get; set; }
        public Nullable<int> Q_five { get; set; }
        public Nullable<int> Q_six { get; set; }
        public Nullable<int> Sum { get; set; }
        public string Percentage { get; set; }
    }
}