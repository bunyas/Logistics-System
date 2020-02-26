using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mascis.Models
{
    class OrderSMCModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mProductCode;
        private double? mOpeningBalance;
        private double? mQtyRecieved;
        private double? mConsumption;
        private double? mLossesAndAdjustments;
        private double? mTotalClosingBalance;
        private double? mQuantityToOrder;
        private int? mQuantityAllocated;
        private string mComments;
        private string mRFSONotes;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ProductCode { get { return mProductCode; } set { mProductCode = value; } }
        public double? OpeningBalance { get { return mOpeningBalance; } set { mOpeningBalance = value; } }
        public double? QtyRecieved { get { return mQtyRecieved; } set { mQtyRecieved = value; } }
        public double? Consumption { get { return mConsumption; } set { mConsumption = value; } }
        public double? LossesAndAdjustments { get { return mLossesAndAdjustments; } set { mLossesAndAdjustments = value; } }
        public double? TotalClosingBalance { get { return mTotalClosingBalance; } set { mTotalClosingBalance = value; } }
        public double? QuantityToOrder { get { return mQuantityToOrder; } set { mQuantityToOrder = value; } }
        public int? QuantityAllocated { get { return mQuantityAllocated; } set { mQuantityAllocated = value; } }
        public string Comments { get { return mComments; } set { mComments = value; } }
        public string RFSONotes { get { return mRFSONotes; } set { mRFSONotes = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public Order_SMC_SLM GetRecordByKey(string order_no, int? product_code)
        {
            try
            {
                return context.Order_SMC_SLM.FirstOrDefault(s => s.OrderNumber == order_no && s.ProductCode == product_code);
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
                    var x = new Order_SMC_SLM
                    {
                        OrderNumber=mOrderNumber,
                        //FacilityCode = mFacilityCode,
                        //StartDate = mStartDate,
                        //EndDate = mEndDate,
                        ProductCode = mProductCode,
                        OpeningBalance = mOpeningBalance,
                        QtyRecieved = mQtyRecieved,
                        Consumption = mConsumption,
                        LossesAndAdjustments = mLossesAndAdjustments,
                        TotalClosingBalance = mTotalClosingBalance,
                        QuantityToOrder = mQuantityToOrder,
                        QuantityAllocated = mQuantityAllocated,
                        Comments = mComments,
                        RFSONotes = mRFSONotes
                    };
                    context.Order_SMC_SLM.Add(x);
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
                    x.QtyRecieved = mQtyRecieved;
                    x.Consumption = mConsumption;
                    x.LossesAndAdjustments = mLossesAndAdjustments;
                    x.TotalClosingBalance = mTotalClosingBalance;
                    x.QuantityToOrder = mQuantityToOrder;
                    x.QuantityAllocated = mQuantityAllocated;
                    x.Comments = mComments;
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
                    x.QuantityAllocated = mQuantityAllocated;
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
