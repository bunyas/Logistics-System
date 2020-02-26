using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class ProductModel
    {
        mascisEntities context = new mascisEntities();

        public List<spView_export_atomic_All_ProductsGetAll_Result> GetOrders(DateTime? StartDate, DateTime? EndDate, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_All_ProductsGetAll_Result>();
                var m = new List<spView_export_atomic_All_ProductsGetAll_Result>();
                var t = context.spView_export_atomic_All_ProductsGetAll(StartDate, EndDate).ToList();
                if (emergency == false)
                {
                    x = t.Where(e => (e.EmergencyOrder == false || e.EmergencyOrder == null) && e.OrderStatusId != 5).ToList();
                }
                if (emergency == true)
                {
                    x = t.Where(e => e.EmergencyOrder == true && e.OrderStatusId != 5).ToList();
                }
                if (emergency == null)
                {
                    x = t.Where(e=> e.OrderStatusId != 5).ToList();
                }
                //if (x.Count() > 0)
                //{
                //    foreach (var a in x)
                //    {
                //        var b = context.A_Sector_Change.OrderBy(e => e.date_of_change).FirstOrDefault(e => e.date_of_change > a.DatePrepared && e.facility_code == a.FacilityCode);
                //        if (b != null)
                //        {
                //            a.DeliveryZoneCode = b.Old_sector;
                //            a.ZoneDescription = "Sector " + b.Old_sector;
                //        }
                //        m.Add(a);
                //    }
                //}
                return x;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}