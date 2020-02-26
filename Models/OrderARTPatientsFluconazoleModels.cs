using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace mascis.Models
{
    class OrderARTPatientsFluconazoleModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private short mConditionCode;
        private int? mAdults;
        private int? mChildren;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public short ConditionCode { get { return mConditionCode; } set { mConditionCode = value; } }
        public int? Adults { get { return mAdults; } set { mAdults = value; } }
        public int? Children { get { return mChildren; } set { mChildren = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public Treatment_Fluconazole GetRecordByKey(string order_no, int? condtiton_code)
        {
            try
            {
                return context.Treatment_Fluconazole.FirstOrDefault(s => s.OrderNumber == order_no && s.ConditionCode == condtiton_code);
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
                if (GetRecordByKey(mOrderNumber, mConditionCode) == null)
                {
                    var x = new Treatment_Fluconazole
                    {
                        OrderNumber=mOrderNumber,
                        //FacilityCode = mFacilityCode,
                        //StartDate = mStartDate,
                        //EndDate = mEndDate,
                        ConditionCode = mConditionCode,
                        Adults = mAdults,
                        Children = mChildren
                    };
                    context.Treatment_Fluconazole.Add(x);
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
                var x = GetRecordByKey(mOrderNumber, mConditionCode);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    //x.FacilityCode = mFacilityCode;
                    //x.StartDate = mStartDate;
                    //x.EndDate = mEndDate;
                    x.ConditionCode = mConditionCode;
                    x.Adults = mAdults;
                    x.Children = mChildren;
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
