using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mascis.Models
{
    class OrderViralLoadReagentsModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mProductCode;
        private double? mOpeningBalance;
        private double? mQuantityRecieved;
        private double? mConsumption;
        private double? mLossesAndAdjustments;
        private double? mTotalClosingBalance;
        private double? mQuantityOrdered;
        private int? mQuantityAllocated;
        private string mComments;
        private string mRFSONotes;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ProductCode { get { return mProductCode; } set { mProductCode = value; } }
        public double? OpeningBalance { get { return mOpeningBalance; } set { mOpeningBalance = value; } }
        public double? QuantityRecieved { get { return mQuantityRecieved; } set { mQuantityRecieved = value; } }
        public double? Consumption { get { return mConsumption; } set { mConsumption = value; } }
        public double? LossesAndAdjustments { get { return mLossesAndAdjustments; } set { mLossesAndAdjustments = value; } }
        public double? TotalClosingBalance { get { return mTotalClosingBalance; } set { mTotalClosingBalance = value; } }
        public double? QuantityOrdered { get { return mQuantityOrdered; } set { mQuantityOrdered = value; } }
        public int? QuantityAllocated { get { return mQuantityAllocated; } set { mQuantityAllocated = value; } }
        public string Comments { get { return mComments; } set { mComments = value; } }
        public string RFSONotes { get { return mRFSONotes; } set { mRFSONotes = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public Order_ViralLoadReagents_Detail GetRecordByKey(string order_no, int? product_code)
        {
            try
            {
                return context.Order_ViralLoadReagents_Detail.FirstOrDefault(s => s.OrderNumber == order_no  && s.ProductCode == product_code);
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
                if (GetRecordByKey(mOrderNumber, mProductCode) == null)
                {
                    var x = new Order_ViralLoadReagents_Detail
                    {
                        OrderNumber=mOrderNumber,
                        //FacilityCode = mFacilityCode,
                        //StartDate = mStartDate,
                        //EndDate = mEndDate,
                        ProductCode = mProductCode,
                        OpeningBalance = mOpeningBalance,
                        QuantityRecieved = mQuantityRecieved,
                        Consumption = mConsumption,
                        LossesAndAdjustments = mLossesAndAdjustments,
                        TotalClosingBalance = mTotalClosingBalance,
                        QuantityOrdered = mQuantityOrdered,
                        Quantity_Allocated = mQuantityAllocated,
                        Comment = mComments,
                        RFSONotes = mRFSONotes
                    };
                    context.Order_ViralLoadReagents_Detail.Add(x);
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
                var x = GetRecordByKey(mOrderNumber, mProductCode);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    //x.FacilityCode = mFacilityCode;
                    //x.StartDate = mStartDate;
                    //x.EndDate = mEndDate;
                    x.ProductCode = mProductCode;
                    x.OpeningBalance = mOpeningBalance;
                    x.QuantityRecieved = mQuantityRecieved;
                    x.Consumption = mConsumption;
                    x.LossesAndAdjustments = mLossesAndAdjustments;
                    x.TotalClosingBalance = mTotalClosingBalance;
                    x.QuantityOrdered = mQuantityOrdered;
                    x.Quantity_Allocated = mQuantityAllocated;
                    x.Comment = mComments;
                    x.RFSONotes = mRFSONotes;
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
                var x = GetRecordByKey(mOrderNumber, mProductCode);
                if (x != null)
                { 
                    x.ProductCode = mProductCode;
                    x.Quantity_Allocated = mQuantityAllocated;
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
