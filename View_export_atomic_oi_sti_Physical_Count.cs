//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class View_export_atomic_oi_sti_Physical_Count
    {
        public int FacilityCode { get; set; }
        public string SAP_Code { get; set; }
        public string Facility { get; set; }
        public Nullable<int> DeliveryZoneCode { get; set; }
        public string ZoneDescription { get; set; }
        public Nullable<int> DistrrictCode { get; set; }
        public string District_Name { get; set; }
        public Nullable<int> ImplimentingPartnerCode { get; set; }
        public string ImplementingPartnerDescription { get; set; }
        public Nullable<int> level_of_care { get; set; }
        public string level_Desc { get; set; }
        public string SCTO { get; set; }
        public System.DateTime DateOfPhysicalCount { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public int ProductCode { get; set; }
        public string Product_SAP_code { get; set; }
        public string product_description { get; set; }
        public string BatchNo { get; set; }
        public Nullable<double> Quantity_Dispensary { get; set; }
        public Nullable<double> Quantity_Store { get; set; }
        public Nullable<double> Total { get; set; }
        public string Comment { get; set; }
        public string AddedBy { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<bool> record_status { get; set; }
    }
}
