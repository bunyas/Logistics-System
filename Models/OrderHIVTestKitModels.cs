using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mascis.Models
{
    class OrderHIVTestKitModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mitem_code;
        private double? mNo_Test_Start_2Months;
        private double? mTest_Recieved_2Months;
        private double? mTest_Used_2Months;
        private double? mLoss_Adjustment;
        private double? mTest_Remaining;
        private double? mMaximum_Stock;
        private double? mQunatity_On_Order;
        private double? mQuantity_Required;
        private string mQuantity_To_Ship;
        private int? mQuantity_Allocated;
        private string mRFSONotes;
        private string mNotes;
        private double? mDaysOutOfStockDuring2Months;
        private double? mAdjustedAMC;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ProductCode { get { return mitem_code; } set { mitem_code = value; } }
        public double? No_Test_Start_2Months { get { return mNo_Test_Start_2Months; } set { mNo_Test_Start_2Months = value; } }
        public double? Test_Recieved_2Months { get { return mTest_Recieved_2Months; } set { mTest_Recieved_2Months = value; } }
        public double? Test_Used_2Months { get { return mTest_Used_2Months; } set { mTest_Used_2Months = value; } }
        public double? Loss_Adjustment { get { return mLoss_Adjustment; } set { mLoss_Adjustment = value; } }
        public double? Test_Remaining { get { return mTest_Remaining; } set { mTest_Remaining = value; } }
        public double? Maximum_Stock { get { return mMaximum_Stock; } set { mMaximum_Stock = value; } }
        public double? Qunatity_On_Order { get { return mQunatity_On_Order; } set { mQunatity_On_Order = value; } }
        public double? Quantity_Required { get { return mQuantity_Required; } set { mQuantity_Required = value; } }
        public string Quantity_To_Ship { get { return mQuantity_To_Ship; } set { mQuantity_To_Ship = value; } }
        public int? Quantity_Allocated { get { return mQuantity_Allocated; } set { mQuantity_Allocated = value; } }
        public string RFSONotes { get { return mRFSONotes; } set { mRFSONotes = value; } }
        public string Notes { get { return mNotes; } set { mNotes = value; } }
        public double? DaysOutOfStockDuring2Months { get { return mDaysOutOfStockDuring2Months; } set { mDaysOutOfStockDuring2Months = value; } }
        public double? AdjustedAMC { get { return mAdjustedAMC; } set { mAdjustedAMC = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public order_hiv_rapid_test_kit GetRecordByKey(string order_no, int? product_code)
        {
            try
            {
                return context.order_hiv_rapid_test_kit.FirstOrDefault(s => s.OrderNumber == order_no && s.item_code == product_code);
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
                var x = new order_hiv_rapid_test_kit
                {
                    OrderNumber = mOrderNumber,
                    //Facility_Code = mFacilityCode,
                    //Start_Date = mStartDate,
                    //End_Date = mEndDate,
                    item_code = mitem_code,
                    No_Test_Start_2Months = mNo_Test_Start_2Months,
                    Test_Recieved_2Months = mTest_Recieved_2Months,
                    Test_Used_2Months = mTest_Used_2Months,
                    Test_Remaining = mTest_Remaining,
                    Loss_Adjustment = mLoss_Adjustment,
                    Maximum_Stock = mMaximum_Stock,
                    Qunatity_On_Order = mQunatity_On_Order,
                    Quantity_Required = mQuantity_Required,
                    Quantity_To_Ship = mQuantity_To_Ship,
                    Quantity_Allocated = mQuantity_Allocated,
                    RFSONotes = mRFSONotes,
                    Notes = mNotes,
                    DaysOutOfStockDuring2Months = mDaysOutOfStockDuring2Months,
                    AdjustedAMC = mAdjustedAMC
                };
                context.order_hiv_rapid_test_kit.Add(x);
                context.SaveChanges();
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
                var x = GetRecordByKey(mOrderNumber, mitem_code);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    //x.Facility_Code = mFacilityCode;
                    //x.Start_Date = mStartDate;
                    //x.End_Date = mEndDate;
                    x.item_code = mitem_code;
                    x.No_Test_Start_2Months = mNo_Test_Start_2Months;
                    x.Test_Recieved_2Months = mTest_Recieved_2Months;
                    x.Test_Used_2Months = mTest_Used_2Months;
                    x.Test_Remaining = mTest_Remaining;
                    x.Loss_Adjustment = mLoss_Adjustment;
                    x.Maximum_Stock = mMaximum_Stock;
                    x.Qunatity_On_Order = mQunatity_On_Order;
                    x.Quantity_Required = mQuantity_Required;
                    x.Quantity_To_Ship = mQuantity_To_Ship;
                    x.Quantity_Allocated = mQuantity_Allocated;
                    x.RFSONotes = mRFSONotes;
                    x.Notes = mNotes;
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
                var x = GetRecordByKey(mOrderNumber, mitem_code);
                if (x != null)
                {
                    x.item_code = mitem_code;
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
