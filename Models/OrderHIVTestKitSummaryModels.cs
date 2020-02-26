using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace mascis.Models
{
    class OrderHIVTestKitSummaryModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mitem_code;
        private double? mHCT;
        private double? mPMTCT;
        private double? mClinical_Diagnosis;
        private double? mSMC;
        private double? mQuality_Control;
        private double? mTotal;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ProductCode { get { return mitem_code; } set { mitem_code = value; } }
        public double? HCT { get { return mHCT; } set { mHCT = value; } }
        public double? PMTCT { get { return mPMTCT; } set { mPMTCT = value; } }
        public double? Clinical_Diagnosis { get { return mClinical_Diagnosis; } set { mClinical_Diagnosis = value; } }
        public double? SMC { get { return mSMC; } set { mSMC = value; } }
        public double? Quality_Control { get { return mQuality_Control; } set { mQuality_Control = value; } }
        public double? Total { get { return mTotal; } set { mTotal = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public order_hiv_rapid_test_kit_summary GetRecordByKey(string order_no, int? product_code)
        {
            try
            {
                return context.order_hiv_rapid_test_kit_summary.FirstOrDefault(s => s.OrderNumber == order_no && s.item_code == product_code);
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
                if (GetRecordByKey(mOrderNumber, mitem_code) == null)
                {
                    var x = new order_hiv_rapid_test_kit_summary
                    {
                        OrderNumber=mOrderNumber,
                        //Facility_Code = mFacilityCode,
                        //Start_Date = mStartDate,
                        //End_Date = mEndDate,
                        item_code = mitem_code,
                        HCT = mHCT,
                        PMTCT = mPMTCT,
                        Clinical_Diagnosis = mClinical_Diagnosis,
                        SMC = mSMC,
                        Quality_Control = mQuality_Control,
                        Total=mTotal,
                    };
                    context.order_hiv_rapid_test_kit_summary.Add(x);
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
                var x = GetRecordByKey(mOrderNumber, mitem_code);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    //x.Facility_Code = mFacilityCode;
                    //x.Start_Date = mStartDate;
                    //x.End_Date = mEndDate;
                    x.item_code = mitem_code;
                    x.HCT = mHCT;
                    x.PMTCT = mPMTCT;
                    x.Clinical_Diagnosis = mClinical_Diagnosis;
                    x.SMC = mSMC;
                    x.Quality_Control = mQuality_Control;
                    x.Total = mTotal;
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
        
    }
}
