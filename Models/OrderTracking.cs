using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class OrderTracking
    {
        mascisEntities context = new mascisEntities();
        public ObjectResult<spOrder_TrackingModule_LogGetAll_Result> GetTracking(int? productCateory, string order_no)
        {
            try
            {
                return context.spOrder_TrackingModule_LogGetAll(productCateory,order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spOrder_TrackingModuleGetAll_Result> GetCurrentTracking(int? productCateory, string order_no, DateTime? sdate, DateTime? edate)
        {
            try
            {
                return context.spOrder_TrackingModuleGetAll(productCateory, order_no, sdate,edate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}