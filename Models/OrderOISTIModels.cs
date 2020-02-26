using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mascis.Models
{
    class OrderOISTIModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mProductCode;
        private double? mPhysicalCountAtBeginningOfReviewPeriod;
        private double? mQuantityReceivedDuringTheTwoMonthsCycle;
        private double? mQuantityUsedDuringTwoMonths;
        private double? mLossesAndAdjustments;
        private double? mNumberOfDaysOutOfStock;
        private double? mThisMonthPhysicalCount;
        private double? mAMC;
        private double? mMonthsOfStock;
        private double? mMaximumStockQuantity;
        private double? mQuantityRequired;
        private string mNotes;
        private double? mQuantity_Allocated;
        private string mRFSONotes;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ProductCode { get { return mProductCode; } set { mProductCode = value; } }
        public double? PhysicalCountAtBeginningOfReviewPeriod { get { return mPhysicalCountAtBeginningOfReviewPeriod; } set { mPhysicalCountAtBeginningOfReviewPeriod = value; } }
        public double? QuantityReceivedDuringTheTwoMonthsCycle { get { return mQuantityReceivedDuringTheTwoMonthsCycle; } set { mQuantityReceivedDuringTheTwoMonthsCycle = value; } }
        public double? QuantityUsedDuringTwoMonths { get { return mQuantityUsedDuringTwoMonths; } set { mQuantityUsedDuringTwoMonths = value; } }
        public double? LossesAndAdjustments { get { return mLossesAndAdjustments; } set { mLossesAndAdjustments = value; } }
        public double? NumberOfDaysOutOfStock { get { return mNumberOfDaysOutOfStock; } set { mNumberOfDaysOutOfStock = value; } }
        public double? ThisMonthPhysicalCount { get { return mThisMonthPhysicalCount; } set { mThisMonthPhysicalCount = value; } }
        public double? AMC { get { return mAMC; } set { mAMC = value; } }
        public double? MonthsOfStock { get { return mMonthsOfStock; } set { mMonthsOfStock = value; } }
        public double? MaximumStockQuantity { get { return mMaximumStockQuantity; } set { mMaximumStockQuantity = value; } }
        public double? QuantityRequired { get { return mQuantityRequired; } set { mQuantityRequired = value; } }
        public string Notes { get { return mNotes; } set { mNotes = value; } }
        public double? Quantity_Allocated { get { return mQuantity_Allocated; } set { mQuantity_Allocated = value; } }
        public string RFSONotes { get { return mRFSONotes; } set { mRFSONotes = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public Order_OI_STI_Detail GetRecordByKey(string order_no,int? product_code)
        {
            try
            {
                return context.Order_OI_STI_Detail.FirstOrDefault(s => s.OrderNumber == order_no && s.ProductCode==product_code);
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
                    var x = new Order_OI_STI_Detail
                    {
                        OrderNumber=mOrderNumber,
                        ProductCode = mProductCode,
                        PhysicalCountAtBeginningOfReviewPeriod = mPhysicalCountAtBeginningOfReviewPeriod,
                        QuantityReceivedDuringTheTwoMonthsCycle = mQuantityReceivedDuringTheTwoMonthsCycle,
                        QuantityUsedDuringTwoMonths = mQuantityUsedDuringTwoMonths,
                        LossesAndAdjustments = mLossesAndAdjustments,
                        NumberOfDaysOutOfStock = mNumberOfDaysOutOfStock,
                        ThisMonthPhysicalCount = mThisMonthPhysicalCount,
                        AMC = mAMC,
                        MonthsOfStock = mMonthsOfStock,
                        MaximumStockQuantity = mMaximumStockQuantity,
                        QuantityRequired = mQuantityRequired,
                        Notes = mNotes,
                        Quantity_Allocated = mQuantity_Allocated,
                        RFSONotes = mRFSONotes
                    };
                    context.Order_OI_STI_Detail.Add(x);
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
                    x.PhysicalCountAtBeginningOfReviewPeriod = mPhysicalCountAtBeginningOfReviewPeriod;
                    x.QuantityReceivedDuringTheTwoMonthsCycle = mQuantityReceivedDuringTheTwoMonthsCycle;
                    x.QuantityUsedDuringTwoMonths = mQuantityUsedDuringTwoMonths;
                    x.LossesAndAdjustments = mLossesAndAdjustments;
                    x.NumberOfDaysOutOfStock = mNumberOfDaysOutOfStock;
                    x.ThisMonthPhysicalCount = mThisMonthPhysicalCount;
                    x.AMC = mAMC;
                    x.MonthsOfStock = mMonthsOfStock;
                    x.MaximumStockQuantity = mMaximumStockQuantity;
                    x.QuantityRequired = mQuantityRequired;
                    x.Notes = mNotes;
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
