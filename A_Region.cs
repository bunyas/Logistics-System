 
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Region
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_Region()
        {
            this.A_District = new HashSet<A_District>();
        }
    
        public string Region_Id { get; set; }
        public string Region_Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A_District> A_District { get; set; }
    }
}
