using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace mascis.Models
{
    class OrderARTPatientModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private short mRegimenCode;
        private int? mPatients_Existing;
        private int? mPatients_New;
        private int? mPregnant_Existing;
        private int? mPregnant_New;
        private int? mExisting_0_3Yrs;
        private int? mNew_0_3Yrs;
        private int? mExisting_3_10Yrs;
        private int? mNew_3_10Yrs;
        private int? mExisting_10_15Yrs_Less35Kg;
        private int? mNew_10_15Yrs_Less35Kg;
        private int? mExisting_10_15Yrs_Greater35Kg;
        private int? mNew_10_15Yrs_Greater35Kg;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public short RegimenCode { get { return mRegimenCode; } set { mRegimenCode = value; } }
        public int? Patients_Existing { get { return mPatients_Existing; } set { mPatients_Existing = value; } }
        public int? Patients_New { get { return mPatients_New; } set { mPatients_New = value; } }
        public int? Pregnant_Existing { get { return mPregnant_Existing; } set { mPregnant_Existing = value; } }
        public int? Pregnant_New { get { return mPregnant_New; } set { mPregnant_New = value; } }

        public int? Existing_0_3Yrs { get { return mExisting_0_3Yrs; } set { mExisting_0_3Yrs = value; } }
        public int? New_0_3Yrs { get { return mNew_0_3Yrs; } set { mNew_0_3Yrs = value; } }
        public int? Existing_3_10Yrs { get { return mExisting_3_10Yrs; } set { mExisting_3_10Yrs = value; } }
        public int? New_3_10Yrs { get { return mNew_3_10Yrs; } set { mNew_3_10Yrs = value; } }
        public int? Existing_10_15Yrs_Less35Kg { get { return mExisting_10_15Yrs_Less35Kg; } set { mExisting_10_15Yrs_Less35Kg = value; } }
        public int? New_10_15Yrs_Less35Kg { get { return mNew_10_15Yrs_Less35Kg; } set { mNew_10_15Yrs_Less35Kg = value; } }
        public int? Existing_10_15Yrs_Greater35Kg { get { return mExisting_10_15Yrs_Greater35Kg; } set { mExisting_10_15Yrs_Greater35Kg = value; } }
        public int? New_10_15Yrs_Greater35Kg { get { return mNew_10_15Yrs_Greater35Kg; } set { mNew_10_15Yrs_Greater35Kg = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }


        public Treatment_PatientSummary GetRecordByKey(string order_no, int? regimne_code)
        {
            try
            {
                return context.Treatment_PatientSummary.FirstOrDefault(s => s.OrderNumber == order_no && s.RegimenCode == regimne_code);
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
                if (GetRecordByKey(mOrderNumber, mRegimenCode) == null)
                {
                    var x = new Treatment_PatientSummary
                    {
                        OrderNumber=mOrderNumber,
                        //FacilityCode = mFacilityCode,
                        //StartDate = mStartDate,
                        //EndDate = mEndDate,
                        RegimenCode = mRegimenCode,
                        Patients_Existing = mPatients_Existing,
                        Patients_New = mPatients_New,
                        Pregnant_Existing = mPregnant_Existing,
                        Pregnant_New = mPregnant_New,
                        Existing_0_3Yrs = mExisting_0_3Yrs,
                        New_0_3Yrs = mNew_0_3Yrs,
                        Existing_3_10Yrs = mExisting_3_10Yrs,
                        New_3_10Yrs = mNew_3_10Yrs,
                        Existing_10_15Yrs_Less35Kg = mExisting_10_15Yrs_Less35Kg,
                        New_10_15Yrs_Less35Kg = mNew_10_15Yrs_Less35Kg,
                        Existing_10_15Yrs_Greater35Kg = mExisting_10_15Yrs_Greater35Kg,
                        New_10_15Yrs_Greater35Kg = mNew_10_15Yrs_Greater35Kg
                    };
                    context.Treatment_PatientSummary.Add(x);
                    context.SaveChanges();
                }
                return true;
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
                var x = GetRecordByKey(mOrderNumber, mRegimenCode);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    //x.FacilityCode = mFacilityCode;
                    //x.StartDate = mStartDate;
                    //x.EndDate = mEndDate;
                    x.RegimenCode = mRegimenCode;
                    x.Patients_Existing = mPatients_Existing;
                    x.Patients_New = mPatients_New;
                    x.Pregnant_Existing = mPregnant_Existing;
                    x.Pregnant_New = mPregnant_New;
                    x.Existing_0_3Yrs = mExisting_0_3Yrs;
                    x.New_0_3Yrs = mNew_0_3Yrs;
                    x.Existing_3_10Yrs = mExisting_3_10Yrs;
                    x.New_3_10Yrs = mNew_3_10Yrs;
                    x.Existing_10_15Yrs_Less35Kg = mExisting_10_15Yrs_Less35Kg;
                    x.New_10_15Yrs_Less35Kg = mNew_10_15Yrs_Less35Kg;
                    x.Existing_10_15Yrs_Greater35Kg = mExisting_10_15Yrs_Greater35Kg;
                    x.New_10_15Yrs_Greater35Kg = mNew_10_15Yrs_Greater35Kg;
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
