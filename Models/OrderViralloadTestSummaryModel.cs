using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class OrderViralloadTestSummaryModel
    {
        mascisEntities context = new mascisEntities();

        public int mFacilityCode;
        public System.DateTime mStartDate;
        public System.DateTime mEndDate;
        public string mOrderNumber;
        public int mID;
        public Nullable<int> mOld;
        public Nullable<int> mExpected_New;
        public Nullable<int> mQuantity;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ID { get { return mID; } set { mID = value; } }
        public int? Old { get { return mOld; } set { mOld = value; } }
        public int? Expected_New { get { return mExpected_New; } set { mExpected_New = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public int? Quantity { get { return mQuantity; } set { mQuantity = value; } }

        public Order_ViralLoadReagents_Summary GetRecordByKey(string order_no, int? id)
        {
            try
            {
                return context.Order_ViralLoadReagents_Summary.FirstOrDefault(s => s.OrderNumber == order_no && s.ID == id);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Order_TB_Summary GetRecordByKeyTB(string order_no, int? id)
        {
            try
            {
                return context.Order_TB_Summary.FirstOrDefault(s => s.OrderNumber == order_no && s.ID == id);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Order_RUTF_Header_Summary GetRecordByKeyRUFT(string order_no, int? id)
        {
            try
            {
                return context.Order_RUTF_Header_Summary.FirstOrDefault(s => s.OrderNumber == order_no && s.ID == id);
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
                    var x = new Order_ViralLoadReagents_Summary
                    {
                        OrderNumber = mOrderNumber,
                        FacilityCode = mFacilityCode,
                        StartDate = mStartDate,
                        EndDate = mEndDate,
                        ID = mID,
                        Quantity = mQuantity
                    };
                    context.Order_ViralLoadReagents_Summary.Add(x);
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

        private Boolean SaveRUTF()
        {
            try
            {
                if (GetRecordByKeyRUFT(mOrderNumber, mID) == null)
                {
                    var x = new Order_RUTF_Header_Summary
                    {
                        OrderNumber = mOrderNumber,
                        FacilityCode = mFacilityCode,
                        StartDate = mStartDate,
                        EndDate = mEndDate,
                        ID = mID,
                        Old = mOld,
                        Expected_New=mExpected_New
                    };
                    context.Order_RUTF_Header_Summary.Add(x);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean UpdateRUTF()
        {
            try
            {
                var x = GetRecordByKeyRUFT(mOrderNumber, mID);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    x.FacilityCode = mFacilityCode;
                    x.StartDate = mStartDate;
                    x.EndDate = mEndDate;
                    x.ID = mID;
                    x.Old = mOld;
                    x.Expected_New = mExpected_New;
                    context.Entry(x).State = EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    SaveRUTF();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        private Boolean SaveTB()
        {
            try
            {
                if (GetRecordByKeyTB(mOrderNumber, mID) == null)
                {
                    var x = new Order_TB_Summary
                    {
                        OrderNumber = mOrderNumber,
                        FacilityCode = mFacilityCode,
                        StartDate = mStartDate,
                        EndDate = mEndDate,
                        ID = mID,
                        Quantity = mQuantity
                    };
                    context.Order_TB_Summary.Add(x);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean UpdateTB()
        {
            try
            {
                var x = GetRecordByKeyTB(mOrderNumber, mID);
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
                    SaveTB();
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