using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mascis.Models
{
    class OrderARTModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mDrugCode;
        private double? mOpeningBalance;
        private double? mQuantityRecieved;
        private double? mPMTCT_Consumption;
        private double? mART_Consumption;
        private double? mLosses_Adjustments;
        private double? mClosingBalance;
        private double? mMonths_Stock_atHand;
        private double? mQuantity_Required_Current_Patients;
        private double? mEstimatedNew_ART_Patients;
        private double? mEstimatedNew_HIV_Pregnant;
        private double? mDrugsRequired_NewPatients;
        private double? mTotalDrugs_Required;
        private string mNotes;
        private double? mQuantity_Allocated;
        private string mRFSONotes;
        private double? mART_eMTCT_Consumption;
        private double? mDaysOutOfStockDuring2Months;
        private double? mAdjustedAMC;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int DrugCode { get { return mDrugCode; } set { mDrugCode = value; } }
        public double? OpeningBalance { get { return mOpeningBalance; } set { mOpeningBalance = value; } }
        public double? QuantityRecieved { get { return mQuantityRecieved; } set { mQuantityRecieved = value; } }
        public double? PMTCT_Consumption { get { return mPMTCT_Consumption; } set { mPMTCT_Consumption = value; } }
        public double? ART_Consumption { get { return mART_Consumption; } set { mART_Consumption = value; } }
        public double? Losses_Adjustments { get { return mLosses_Adjustments; } set { mLosses_Adjustments = value; } }
        public double? ClosingBalance { get { return mClosingBalance; } set { mClosingBalance = value; } }
        public double? Months_Stock_atHand { get { return mMonths_Stock_atHand; } set { mMonths_Stock_atHand = value; } }
        public double? Quantity_Required_Current_Patients { get { return mQuantity_Required_Current_Patients; } set { mQuantity_Required_Current_Patients = value; } }
        public double? EstimatedNew_ART_Patients { get { return mEstimatedNew_ART_Patients; } set { mEstimatedNew_ART_Patients = value; } }
        public double? EstimatedNew_HIV_Pregnant { get { return mEstimatedNew_HIV_Pregnant; } set { mEstimatedNew_HIV_Pregnant = value; } }
        public double? DrugsRequired_NewPatients { get { return mDrugsRequired_NewPatients; } set { mDrugsRequired_NewPatients = value; } }
        public double? TotalDrugs_Required { get { return mTotalDrugs_Required; } set { mTotalDrugs_Required = value; } }
        public string Notes { get { return mNotes; } set { mNotes = value; } }
        public double? Quantity_Allocated { get { return mQuantity_Allocated; } set { mQuantity_Allocated = value; } }
        public string RFSONotes { get { return mRFSONotes; } set { mRFSONotes = value; } }
        public double? ART_eMTCT_Consumption { get { return mART_eMTCT_Consumption; } set { mART_eMTCT_Consumption = value; } }
        public double? DaysOutOfStockDuring2Months { get { return mDaysOutOfStockDuring2Months; } set { mDaysOutOfStockDuring2Months = value; } }
        public double? AdjustedAMC { get { return mAdjustedAMC; } set { mAdjustedAMC = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public Order_DrugDetails GetRecordByKey(string order_no, int? product_code)
        {
            try
            {
                return context.Order_DrugDetails.FirstOrDefault(s => s.OrderNumber == order_no && s.DrugCode == product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        private Boolean Save()
        {
            try
            {
                if (GetRecordByKey(mOrderNumber, mDrugCode) == null)
                {
                    var x = new Order_DrugDetails
                    {
                        OrderNumber=mOrderNumber.Trim(),
                        //FacilityCode = mFacilityCode,
                        //StartDate = mStartDate,
                        //EndDate = mEndDate,
                        DrugCode = mDrugCode,
                        OpeningBalance = mOpeningBalance,
                        QuantityRecieved = mQuantityRecieved,
                        PMTCT_Consumption = mPMTCT_Consumption,
                        ART_Consumption = mART_Consumption,
                        Losses_Adjustments = mLosses_Adjustments,
                        ClosingBalance = mClosingBalance,
                        Months_Stock_atHand = mMonths_Stock_atHand,
                        Quantity_Required_Current_Patients = mQuantity_Required_Current_Patients,
                        EstimatedNew_ART_Patients = mEstimatedNew_ART_Patients,
                        EstimatedNew_HIV_Pregnant = mEstimatedNew_HIV_Pregnant,
                        DrugsRequired_NewPatients = mDrugsRequired_NewPatients,
                        TotalDrugs_Required = mTotalDrugs_Required,
                        Notes = mNotes,
                        Quantity_Allocated = mQuantity_Allocated,
                        RFSONotes = mRFSONotes,
                        ART_eMTCT_Consumption= mART_eMTCT_Consumption,
                        DaysOutOfStockDuring2Months= mDaysOutOfStockDuring2Months,
                        AdjustedAMC= mAdjustedAMC
                    };
                    context.Order_DrugDetails.Add(x);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean Update()
        {
            try
            {
                var x = GetRecordByKey(mOrderNumber, mDrugCode);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber.Trim();
                    //x.FacilityCode = mFacilityCode;
                    //x.StartDate = mStartDate;
                    //x.EndDate = mEndDate;
                    x.DrugCode = mDrugCode;
                    x.OpeningBalance = mOpeningBalance;
                    x.QuantityRecieved = mQuantityRecieved;
                    x.PMTCT_Consumption = mPMTCT_Consumption;
                    x.ART_Consumption = mART_Consumption;
                    x.Losses_Adjustments = mLosses_Adjustments;
                    x.ClosingBalance = mClosingBalance;
                    x.Months_Stock_atHand = mMonths_Stock_atHand;
                    x.Quantity_Required_Current_Patients = mQuantity_Required_Current_Patients;
                    x.EstimatedNew_ART_Patients = mEstimatedNew_ART_Patients;
                    x.EstimatedNew_HIV_Pregnant = mEstimatedNew_HIV_Pregnant;
                    x.DrugsRequired_NewPatients = mDrugsRequired_NewPatients;
                    x.TotalDrugs_Required = mTotalDrugs_Required;
                    x.Notes = mNotes;
                    x.Quantity_Allocated = mQuantity_Allocated;
                    x.RFSONotes = mRFSONotes;
                    x.ART_eMTCT_Consumption = mART_eMTCT_Consumption;
                    x.DaysOutOfStockDuring2Months = mDaysOutOfStockDuring2Months;
                    x.AdjustedAMC = mAdjustedAMC;
                    context.SaveChanges();
                }
                else
                {
                    Save();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean UpdateAllocation()
        {
            try
            {
                var x = GetRecordByKey(mOrderNumber, mDrugCode);
                if (x != null)
                {
                    x.DrugCode = mDrugCode;
                    x.Quantity_Allocated = mQuantity_Allocated;
                    x.RFSONotes = mRFSONotes;
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        
    }
}
