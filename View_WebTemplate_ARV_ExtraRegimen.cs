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
    
    public partial class View_WebTemplate_ARV_ExtraRegimen
    {
        public int regimen_classification_code { get; set; }
        public short RegimenCategoryCode { get; set; }
        public string RegimenCategoryDesc { get; set; }
        public string regimen_classification_desc { get; set; }
        public string RegimenDesc { get; set; }
        public Nullable<int> Patients_Existing { get; set; }
        public Nullable<int> Patients_New { get; set; }
        public Nullable<int> Pregnant_Existing { get; set; }
        public Nullable<int> Pregnant_New { get; set; }
        public Nullable<int> ExistingPatients { get; set; }
        public Nullable<int> NewPatients { get; set; }
        public short RegimenCode { get; set; }
        public Nullable<bool> StandardRegimen { get; set; }
        public Nullable<int> FacilityCode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}