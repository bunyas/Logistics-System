using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class OrderTBModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mProductCode;
        private double? mOpeningBalance;
        private double? mQtyRecieved;
        private double? mDispensed2MonthsReview;
        private double? mLossesAndAdjustments;
        private double? mDaysOutofStock;
        private double? mClosingBalance;
        private double? mAdjustedAMC;
        private double? mMonthsOfStock;
        //private double? mMaximumStockQuantity;
        private double? mQuantityRequired;
        private string mComments;
        private double? mQuantity_Allocated;
        private string mRFSONotes;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ProductCode { get { return mProductCode; } set { mProductCode = value; } }
        public double? OpeningBalance { get { return mOpeningBalance; } set { mOpeningBalance = value; } }
        public double? QtyRecieved { get { return mQtyRecieved; } set { mQtyRecieved = value; } }
        public double? Dispensed2MonthsReview { get { return mDispensed2MonthsReview; } set { mDispensed2MonthsReview = value; } }
        public double? LossesAndAdjustments { get { return mLossesAndAdjustments; } set { mLossesAndAdjustments = value; } }
        public double? DaysOutofStock { get { return mDaysOutofStock; } set { mDaysOutofStock = value; } }
        public double? ClosingBalance { get { return mClosingBalance; } set { mClosingBalance = value; } }
        public double? AdjustedAMC { get { return mAdjustedAMC; } set { mAdjustedAMC = value; } }
        public double? MonthsOfStock { get { return mMonthsOfStock; } set { mMonthsOfStock = value; } }
       // public double? MaximumStockQuantity { get { return mMaximumStockQuantity; } set { mMaximumStockQuantity = value; } }
        public double? QuantityRequired { get { return mQuantityRequired; } set { mQuantityRequired = value; } }
        public string Comments { get { return mComments; } set { mComments = value; } }
        public double? Quantity_Allocated { get { return mQuantity_Allocated; } set { mQuantity_Allocated = value; } }
        public string RFSONotes { get { return mRFSONotes; } set { mRFSONotes = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public Order_TB GetRecordByKey(string order_no, int? product_code)
        {
            try
            {
                return context.Order_TB.FirstOrDefault(s => s.OrderNumber == order_no && s.ProductCode == product_code);
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
                    var x = new Order_TB
                    {
                        OrderNumber = mOrderNumber,
                        ProductCode = mProductCode,
                        OpeningBalance = mOpeningBalance,
                        QtyRecieved = mQtyRecieved,
                        Dispensed2MonthsReview = mDispensed2MonthsReview,
                        LossesAndAdjustments = mLossesAndAdjustments,
                        DaysOutofStock = mDaysOutofStock,
                        ClosingBalance = mClosingBalance,
                        AdjustedAMC = mAdjustedAMC,
                        MonthsOfStock = mMonthsOfStock,
                       // MaximumStockQuantity = mMaximumStockQuantity,
                        QuantityRequired = mQuantityRequired,
                        Comments = mComments,
                        Quantity_Allocated = mQuantity_Allocated,
                        RFSONotes = mRFSONotes
                    };
                    context.Order_TB.Add(x);
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
                    x.ProductCode = mProductCode;
                    x.OpeningBalance = mOpeningBalance;
                    x.QtyRecieved = mQtyRecieved;
                    x.Dispensed2MonthsReview = mDispensed2MonthsReview;
                    x.LossesAndAdjustments = mLossesAndAdjustments;
                    x.DaysOutofStock = mDaysOutofStock;
                    x.ClosingBalance = mClosingBalance;
                    x.AdjustedAMC = mAdjustedAMC;
                    x.MonthsOfStock = mMonthsOfStock;
                    //x.MaximumStockQuantity = mMaximumStockQuantity;
                    x.QuantityRequired = mQuantityRequired;
                    x.Comments = mComments;
                    x.Quantity_Allocated = mQuantity_Allocated;
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