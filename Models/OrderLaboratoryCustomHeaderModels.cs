using mascis.ScheduledTasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class OrderLaboratoryCustomHeaderModels
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
        private int? mProductCategoryId;//
        private Boolean? mOrderApproved;
        private int? mOrderStatusId;
        private string mOrderStatusIdChangedBy;
        private DateTime? mOrderStatusIdDateChanged;
        private Boolean mRFSO_SentBackTofacility;
        private int? mOrderTypeId;

        public int? OrderStatusId { get { return mOrderStatusId; } set { mOrderStatusId = value; } }
        public string OrderStatusIdChangedBy { get { return mOrderStatusIdChangedBy; } set { mOrderStatusIdChangedBy = value; } }
        public DateTime? OrderStatusIdDateChanged { get { return mOrderStatusIdDateChanged; } set { mOrderStatusIdDateChanged = value; } }
        public int? ProductCategoryId { get { return mProductCategoryId; } set { mProductCategoryId = value; } }
        public int? OrderTypeId { get { return mOrderTypeId; } set { mOrderTypeId = value; } }
        public DateTime? DateExpected { get { return mDateExpected; } set { mDateExpected = value; } }
        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime DatePrepared { get { return mDatePrepared; } set { mDatePrepared = value; } }
        public string EditedBy { get { return mEditedBy; } set { mEditedBy = value; } }
        public DateTime? EditedDate { get { return mEditedDate; } set { mEditedDate = value; } }
        public Boolean? EmergencyOrder { get { return mEmergencyOrder; } set { mEmergencyOrder = value; } }
        public Boolean FinalSubmission { get { return mFinalSubmission; } set { mFinalSubmission = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public Boolean RFSO_SentBackTofacility { get { return mRFSO_SentBackTofacility; } set { mRFSO_SentBackTofacility = value; } }
        public Order_Lab_Custom_Header GetRecordByKey(string order_no)
        {
            try
            {
                return context.Order_Lab_Custom_Header.FirstOrDefault(s => s.OrderNumber == order_no);
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
                    var x = new Order_Lab_Custom_Header
                    {
                        OrderNumber = mOrderNumber,
                        FacilityCode = mFacilityCode,
                        DatePrepared = mDatePrepared,
                        EditedBy = new UserManagement().getCurrentuser(),
                        EditedDate = DateTime.Now,
                        FinalSubmission = mFinalSubmission,
                        DateExpected = mDateExpected,
                        ProductCategoryId = mProductCategoryId,
                        OrderStatusId = mOrderStatusId,
                        OrderStatusIdChangedBy = mOrderStatusIdChangedBy,
                        OrderStatusIdDateChanged = mOrderStatusIdDateChanged,
                        RFSO_SentBackTofacility = mRFSO_SentBackTofacility,
                        OrderTypeId= mOrderTypeId
                    };
                    context.Order_Lab_Custom_Header.Add(x);
                    context.SaveChanges();
                    ////SaveLog(x);
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

        public Boolean SaveLog(Order_Lab_Custom_Header record)
        {
            try
            {
                var log = new Order_Lab_Custom_Header_Log
                {
                    OrderNumber = record.OrderNumber,
                    FacilityCode = record.FacilityCode,
                    OrderStatusId = System.Convert.ToInt32(record.OrderStatusId),
                    OrderStatusChangedBy = new UserManagement().getCurrentuser(),
                    OrderStatusDateChanged = DateTime.Now,
                    //StartDate = record.StartDate,
                    //EndDate = record.EndDate,
                    DatePrepared = record.DatePrepared,
                    OrderTypeId = record.OrderTypeId,
                    ProductCategoryId = 1
                };
                context.Order_Lab_Custom_Header_Log.Add(log);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw (e);
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
                    //x.EditedBy = new UserManagement().getCurrentuser();
                    //x.EditedDate = DateTime.Now;
                    x.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                    x.OrderStatusIdDateChanged = DateTime.Now;
                    x.DateExpected = mDateExpected;
                    x.OrderStatusId = mOrderStatusId;
                    x.FinalSubmission = mFinalSubmission;
                    x.RFSO_SentBackTofacility = mRFSO_SentBackTofacility;
                    x.OrderTypeId = mOrderTypeId;
                    x.DatePrepared = mDatePrepared;
                    context.SaveChanges();
                    ////SaveLog(x);
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
                    //x.OrderTypeId = mOrderTypeId;
                    //x.EditedBy = new UserManagement().getCurrentuser();
                    //x.EditedDate = DateTime.Now;
                    //x.DateExpected = mDateExpected;
                    x.OrderStatusId = mOrderStatusId;
                    x.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                    x.OrderStatusIdDateChanged = DateTime.Now;
                    context.SaveChanges();
                    ////SaveLog(x);
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
                    //////SaveLog(x);
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveCodinator()
        {
            try
            {
                var x = GetRecordByKey(mOrderNumber);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    x.FacilityCode = mFacilityCode;
                    x.DateExpected = mDateExpected;
                    x.ProductCategoryId = mProductCategoryId;
                    x.DatePrepared = mDatePrepared;
                    x.OrderTypeId = mOrderTypeId;
                    x.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                    x.OrderStatusIdDateChanged = DateTime.Now;
                    x.DateExpected = mDateExpected;
                    x.OrderStatusId = mOrderStatusId;
                    x.FinalSubmission = mFinalSubmission;
                    x.RFSO_SentBackTofacility = mRFSO_SentBackTofacility;
                    x.EmergencyOrder = mEmergencyOrder;
                    context.SaveChanges();
                    ////SaveLog(x);
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }

        }
        public Boolean UpdateSAP(string order_no)
        {
            try
            {
                var x = GetRecordByKey(order_no);
                if (x != null)
                {
                    x.OrderNumber = order_no;
                    //x.FacilityCode = mFacilityCode;
                    //x.DatePrepared = mDatePrepared;
                    //x.EditedBy = new UserManagement().getCurrentuser();
                    //x.EditedDate = DateTime.Now;
                    //x.DateExpected = mDateExpected;
                    x.OrderStatusId = 4;
                    //x.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                    //x.OrderStatusIdDateChanged = DateTime.Now;
                    context.Entry(x).State = EntityState.Modified;
                    context.SaveChanges();
                    ////SaveLog(x);
                }
                //Send Out Confirmation Notifications to the facility
                //EmailJob obx = new EmailJob();
                //if (x.OrderStatusId == 4)
                //{
                //    obx.SendEmailSAP(x.FacilityCode, x.OrderNumber);
                //}
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //update order tables with order status when ever SAP tables are updated...
        //Order Processed
        public Boolean UpdateOrderProcessed(string order_no)
        {
            try
            {
                var x = GetRecordByKey(order_no);
                if (x != null)
                {
                    x.OrderNumber = order_no;
                    //Order Processed
                    x.OrderStatusId = 7;
                    context.Entry(x).State = EntityState.Modified;
                    context.SaveChanges();
                    ////SaveLog(x);
                }

                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //update order tables with order status when ever SAP tables are updated...
        //Order Dispatched
        public Boolean UpdateOrderDispatched(string order_no)
        {
            try
            {
                var x = GetRecordByKey(order_no);
                if (x != null)
                {
                    x.OrderNumber = order_no;
                    //Order Dispatched
                    x.OrderStatusId = 8;
                    context.Entry(x).State = EntityState.Modified;
                    context.SaveChanges();
                    ////SaveLog(x);
                }

                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //update order tables with order status when ever SAP tables are updated...
        //Order Delivered
        public Boolean UpdateOrderDelivered(string order_no)
        {
            try
            {
                var x = GetRecordByKey(order_no);
                if (x != null)
                {
                    x.OrderNumber = order_no;
                    //Order Delivered
                    x.OrderStatusId = 9;
                    context.Entry(x).State = EntityState.Modified;
                    context.SaveChanges();
                    ////SaveLog(x);
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