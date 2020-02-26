
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Sector_Change
    {
        public int facility_code { get; set; }
        public System.DateTime date_of_change { get; set; }
        public int Old_sector { get; set; }
        public Nullable<int> New_sector { get; set; }
    
        public virtual A_Facilities A_Facilities { get; set; }
    }
}
