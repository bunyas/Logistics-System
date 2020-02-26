using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class OrderMAULTHeaderModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mDatePrepared;
        private string mEditedBy;
        private DateTime? mEditedDate;
        private Boolean? mEmergencyOrder;
        private Boolean mFinalSubmission;
        private string mOrderNumber;
        private DateTime? mDateExpected;
        private int? mProductCategoryId;
        private Boolean? mOrderApproved;
        private int? mOrderStatusId;
        private string mOrderStatusIdChangedBy;
        private DateTime? mOrderStatusIdDateChanged;
        private Boolean mRFSO_SentBackTofacility;

        public int? OrderStatusId { get { return mOrderStatusId; } set { mOrderStatusId = value; } }
        public string OrderStatusIdChangedBy { get { return mOrderStatusIdChangedBy; } set { mOrderStatusIdChangedBy = value; } }
        public DateTime? OrderStatusIdDateChanged { get { return mOrderStatusIdDateChanged; } set { mOrderStatusIdDateChanged = value; } }
        public int? ProductCategoryId { get { return mProductCategoryId; } set { mProductCategoryId = value; } }
        public DateTime? DateExpected { get { return mDateExpected; } set { mDateExpected = value; } }
        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime DatePrepared { get { return mDatePrepared; } set { mDatePrepared = value; } }
        public string EditedBy { get { return mEditedBy; } set { mEditedBy = value; } }
        public DateTime? EditedDate { get { return mEditedDate; } set { mEditedDate = value; } }
        public Boolean? EmergencyOrder { get { return mEmergencyOrder; } set { mEmergencyOrder = value; } }
        public Boolean FinalSubmission { get { return mFinalSubmission; } set { mFinalSubmission = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public Boolean RFSO_SentBackTofacility { get { return mRFSO_SentBackTofacility; } set { mRFSO_SentBackTofacility = value; } }
        public Order_MAULT_Header GetRecordByKey(string order_no)
        {
            try
            {
                return context.Order_MAULT_Header.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean Save()
        {
            try
            {
                if (GetRecordByKey(mOrderNumber) == null)
                {
                    var x = new Order_MAULT_Header
                    {
                        OrderNumber = mOrderNumber,
                        FacilityCode = mFacilityCode,
                        DatePrepared = mDatePrepared,
                        EditedBy = new UserManagement().getCurrentuser(),
                        EditedDate = DateTime.Now,
                        FinalSubmission = mFinalSubmission, 
                        DateExpected = mDateExpected,
                        ProductCategoryId=mProductCategoryId,
                        OrderStatusId = mOrderStatusId,
                        OrderStatusIdChangedBy = mOrderStatusIdChangedBy,
                        OrderStatusIdDateChanged = mOrderStatusIdDateChanged,
                        RFSO_SentBackTofacility = mRFSO_SentBackTofacility
                    };
                    context.Order_MAULT_Header.Add(x);
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
                var x = GetRecordByKey(mOrderNumber);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    x.FacilityCode = mFacilityCode;
                    x.EditedBy = new UserManagement().getCurrentuser();
                    x.EditedDate = DateTime.Now;
                    x.DateExpected = mDateExpected;
                    x.OrderStatusId = mOrderStatusId;
                    x.FinalSubmission = mFinalSubmission;
                    x.RFSO_SentBackTofacility = mRFSO_SentBackTofacility;
                    //x.OrderApprovedBy = mOrderApprovedBy;
                    //x.OrderDateOfApproval = mOrderDateOfApproval;
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean Update2()
        {
            try
            {
                var x = GetRecordByKey(mOrderNumber);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    //x.FacilityCode = mFacilityCode;
                    //x.EditedBy = new UserManagement().getCurrentuser();
                    //x.EditedDate = DateTime.Now;
                    //x.DateExpected = mDateExpected;
                    x.OrderStatusId = mOrderStatusId;
                    x.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                    x.OrderStatusIdDateChanged = DateTime.Now;
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean Update(Boolean xRecord_status, DateTime xStartDate, DateTime xEndDate, int xFacilityCode, string xEditedBy)
        {
            try
            {
                var x = GetRecordByKey(mOrderNumber);
                if (x != null)
                {
                    x.EditedBy = xEditedBy;
                    x.EditedDate = DateTime.Now;
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