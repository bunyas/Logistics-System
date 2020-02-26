 
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_ProductType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_ProductType()
        {
            this.A_Product = new HashSet<A_Product>();
        }
    
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A_Product> A_Product { get; set; }
    }
}
