using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
namespace mascis.Models
{
     class PC_Model
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mDateOfPhysicalCount;
        private int mproduct_code;
        private string mBatchNo;
        private double? mQuantity_Dispensary;
        private double? mQuantity_Store;
        private DateTime? mExpiryDate;
        private double? mTotal;
        private string mComment;
        private string mAddedBy;
        private DateTime mDateAdded;
        private Boolean mrecord_status;
       

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime DateOfPhysicalCount { get { return mDateOfPhysicalCount; } set { mDateOfPhysicalCount = value; } }
        public int ProductCode { get { return mproduct_code; } set { mproduct_code = value; } }
        public string BatchNo { get { return mBatchNo; } set { mBatchNo = value; } }
        public double? Quantity_Dispensary { get { return mQuantity_Dispensary; } set { mQuantity_Dispensary = value; } }
        public double? Quantity_Store { get { return mQuantity_Store; } set { mQuantity_Store = value; } }
        public DateTime? ExpiryDate { get { return mExpiryDate; } set { mExpiryDate = value; } }
        public double? Total { get { return mTotal; } set { mTotal = value; } }
        public string Comment { get { return mComment; } set { mComment = value; } }
        public string AddedBy { get { return mAddedBy; } set { mAddedBy = value; } }
        public DateTime DateAdded { get { return mDateAdded; } set { mDateAdded = value; } }
        public Boolean record_status { get { return mrecord_status; } set { mrecord_status = value; } }
        
        public order_hiv_rapid_test_kit_PhysicalCount GetRecordByKey(int facilityID, DateTime DateOfPC, int product_code, string BatchNo)
        {
            try
            {
                return context.order_hiv_rapid_test_kit_PhysicalCount.FirstOrDefault(s => s.FacilityCode == facilityID && s.DateOfPhysicalCount == DateOfPC && s.product_code==product_code && s.BatchNo==BatchNo);
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
                var x = new order_hiv_rapid_test_kit_PhysicalCount
                {
                    FacilityCode = mFacilityCode,
                    DateOfPhysicalCount = mDateOfPhysicalCount,
                    // string Expiry_Date = temp.ExpiryDate.ToString();
                    //DateTime ExpiryTime = DateTime.Parse(Expiry_Date);
                   //obj.ExpiryDate = ExpiryTime;
                    product_code = mproduct_code,
                    BatchNo = mBatchNo,
                    Quantity_Dispensary = mQuantity_Dispensary,
                    Quantity_Store = mQuantity_Store,
                    ExpiryDate = mExpiryDate,
                    Total = (mQuantity_Dispensary + mQuantity_Store),
                    Comment = mComment,
                    AddedBy = new UserManagement().getCurrentuser(),
                    DateAdded = DateTime.Now,
                    record_status = true
                };
                context.order_hiv_rapid_test_kit_PhysicalCount.Add(x);
                context.SaveChanges();
                return true;
            }
            //catch (Exception x)
            //{
            //    throw (x);
            //}
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        public Boolean Update()
        {
            try
            {
                var x = GetRecordByKey(mFacilityCode, mDateOfPhysicalCount,mproduct_code,mBatchNo);
                if (x != null)
                {
                    x.FacilityCode = mFacilityCode;
                    x.DateOfPhysicalCount = mDateOfPhysicalCount;
                    x.product_code = mproduct_code;
                    x.BatchNo = mBatchNo;
                    x.Quantity_Dispensary = mQuantity_Dispensary;
                    x.Quantity_Store = mQuantity_Store;
                    x.ExpiryDate = mExpiryDate;
                    x.Total = (mQuantity_Dispensary + mQuantity_Store);
                    x.Comment = mComment;
                    x.AddedBy = new UserManagement().getCurrentuser();
                    x.DateAdded = DateTime.Now;
                    x.record_status =  true;
                    
                    context.SaveChanges();
                }
                else
                {
                    Save();
                }
                return true;
            }
            //catch (Exception x)
            //{
            //    throw (x);
            //}
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
    }
}

 