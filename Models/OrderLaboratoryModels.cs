using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mascis.Models
{
    class OrderLaboratoryModels
    {
        mascisEntities context = new mascisEntities();

        private int mFacilityCode;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mitem_code;
        private double? mopening_balance;
        private double? mquantity_recieved;
        private double? mconsumption;
        private double? mlosses_adjustment;
        private double? mclosing_balance;
        private double? mquantity_to_order;
        private int? mquantity_allocated;
        private double? mtotal_cost;
        private string mcomments;
        private string mRFSONotes;
        private string mOrderNumber;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public DateTime EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public int ItemCode { get { return mitem_code; } set { mitem_code = value; } }
        public double? opening_balance { get { return mopening_balance; } set { mopening_balance = value; } }
        public double? quantity_recieved { get { return mquantity_recieved; } set { mquantity_recieved = value; } }
        public double? consumption { get { return mconsumption; } set { mconsumption = value; } }
        public double? losses_adjustment { get { return mlosses_adjustment; } set { mlosses_adjustment = value; } }
        public double? closing_balance { get { return mclosing_balance; } set { mclosing_balance = value; } }
        public double? quantity_to_order { get { return mquantity_to_order; } set { mquantity_to_order = value; } }
        public int? quantity_allocated { get { return mquantity_allocated; } set { mquantity_allocated = value; } }
        public double? total_cost { get { return mtotal_cost; } set { mtotal_cost = value; } }
        public string comments { get { return mcomments; } set { mcomments = value; } }
        public string RFSONotes { get { return mRFSONotes; } set { mRFSONotes = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }

        public Order_Lab GetRecordByKey(string order_no, int? product_code)
        {
            try
            {
                return context.Order_Lab.FirstOrDefault(s => s.OrderNumber == order_no  && s.item_code == product_code);
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
                if (GetRecordByKey(mOrderNumber, mitem_code) == null)
                {
                    var x = new Order_Lab
                    {
                        OrderNumber=mOrderNumber,
                        //FacilityCode = mFacilityCode,
                        //start_date = mStartDate,
                        //end_date = mEndDate,
                        item_code = mitem_code,
                        opening_balance = mopening_balance,
                        quantity_recieved = mquantity_recieved,
                        consumption = mconsumption,
                        losses_adjustment = mlosses_adjustment,
                        closing_balance = mclosing_balance,
                        quantity_to_order = mquantity_to_order,
                        quantity_allocated = mquantity_allocated,
                        total_cost = mtotal_cost,
                        comments = mcomments,
                        RFSONotes = mRFSONotes
                    };
                    context.Order_Lab.Add(x);
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
                var x = GetRecordByKey(mOrderNumber, mitem_code);
                if (x != null)
                {
                    x.OrderNumber = mOrderNumber;
                    //x.FacilityCode = mFacilityCode;
                    //x.start_date = mStartDate;
                    //x.end_date = mEndDate;
                    x.item_code = mitem_code;
                    x.opening_balance = mopening_balance;
                    x.quantity_recieved = mquantity_recieved;
                    x.consumption = mconsumption;
                    x.losses_adjustment = mlosses_adjustment;
                    x.closing_balance = mclosing_balance;
                    x.quantity_to_order = mquantity_to_order;
                    x.quantity_allocated = mquantity_allocated;
                    x.total_cost = mtotal_cost;
                    x.comments = mcomments;
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
                var x = GetRecordByKey(mOrderNumber, mitem_code);
                if (x != null)
                {
                    x.item_code = mitem_code;
                    x.quantity_allocated = mquantity_allocated;
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
