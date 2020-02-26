using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Controllers
{
    public class Formulas
    {
        public int Allocated(double? value)
        {
            double x = Math.Round(Convert.ToDouble(value), 1);
            int result = 0;
            if (x.ToString().Contains('.'))
            {
                string[] b = x.ToString().Split('.');
                if (Convert.ToInt32(b[1]) > 0)
                {
                    int a = Convert.ToInt32(b[0]);
                    result = a + 1;
                }
                else
                {
                    result = Convert.ToInt32(value);
                }
            }
            else
            {
                result = Convert.ToInt32(value);
            }
            return result;
        }
    }
}