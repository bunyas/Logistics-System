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
    
    public partial class View_WebTemplate_ARV_Emergency
    {
        public int product_order { get; set; }
        public Nullable<int> product_code { get; set; }
        public string product_numbering { get; set; }
        public string product_description { get; set; }
        public string Basic_Unit { get; set; }
        public Nullable<double> OpeningBalance { get; set; }
        public Nullable<double> QuantityRecieved { get; set; }
        public Nullable<double> PMTCT_Consumption { get; set; }
        public Nullable<double> ART_Consumption { get; set; }
        public Nullable<double> Losses_Adjustments { get; set; }
        public Nullable<double> ClosingBalance { get; set; }
        public Nullable<double> Months_Stock_atHand { get; set; }
        public Nullable<double> Quantity_Required_Current_Patients { get; set; }
        public Nullable<double> EstimatedNew_ART_Patients { get; set; }
        public Nullable<double> EstimatedNew_HIV_Pregnant { get; set; }
        public Nullable<double> DrugsRequired_NewPatients { get; set; }
        public Nullable<double> TotalDrugs_Required { get; set; }
        public string Notes { get; set; }
        public string RFSONotes { get; set; }
        public Nullable<int> FacilityCode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> DatePrepared { get; set; }
        public Nullable<bool> FinalSubmission { get; set; }
        public Nullable<bool> FinalSubmissionCheck { get; set; }
        public Nullable<bool> RFSO_SentBackTofacility { get; set; }
        public string RFSO_SentBackBy { get; set; }
        public Nullable<System.DateTime> RFSO_SentBackDate { get; set; }
        public string RFSO_ApprovedBy { get; set; }
        public Nullable<System.DateTime> RFSO_DateOfApproval { get; set; }
        public Nullable<bool> RFSO_ApproveOrder { get; set; }
        public Nullable<bool> EportedToSAP { get; set; }
        public Nullable<System.DateTime> Facility_UpdateDate { get; set; }
        public string Facility_UserName { get; set; }
        public Nullable<double> Quantity_Allocated { get; set; }
    }
}
