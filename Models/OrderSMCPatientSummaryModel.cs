using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class OrderSMCPatientSummaryModel
    {
        mascisEntities context = new mascisEntities();

        public int mFacilityCode;
        public System.DateTime mStartDate;
        public System.DateTime mEndDate;
        public string mOrderNumber;
        public int mID;
        public Nullable<int> mQuantity;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ID { get { return mID; } set { mID = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public int? Quantity { get { return mQuantity; } set { mQuantity = value; } }

        public Order_SMC_SLM_Summary GetRecordByKey(string order_no, int? id)
        {
            try
            {
                return context.Order_SMC_SLM_Summary.FirstOrDefault(s => s.OrderNumber == order_no && s.ID == id);
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
                if (GetRecordByKey(mOrderNumber, mID) == null)
                {
                    var x = new Order_SMC_SLM_Summary
                    {
                        OrderNumber = mOrderNumber,
                        FacilityCode = mFacilityCode,
                        StartDate = mStartDate,
                        EndDate = mEndDate,
                        ID = mID,
                        Quantity = mQuantity
                    };
                    context.Order_SMC_SLM_Summary.Add(x);
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
                var x = GetRecordByKey(mOrderNumber, mID);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    x.FacilityCode = mFacilityCode;
                    x.StartDate = mStartDate;
                    x.EndDate = mEndDate;
                    x.ID = mID;
                    x.Quantity = mQuantity;
                    context.Entry(x).State = EntityState.Modified;
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