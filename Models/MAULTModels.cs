using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class MAULTModels
    {
        mascisEntities context = new mascisEntities();

        public ObjectResult<spView_WebTemplate_MAULTGetAll_Result> GetOrder(string order_no,int product_category)
        {
            try
            {
                return context.spView_WebTemplate_MAULTGetAll(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_allocation_n_MAULTGetAll1_Result> GetAllocation(int? product_category, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_MAULTGetAll1(product_code, product_category);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

    }
}