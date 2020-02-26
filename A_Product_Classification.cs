 
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Product_Classification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_Product_Classification()
        {
            this.A_Product = new HashSet<A_Product>();
        }
    
        public int ID { get; set; }
        public string ProductType { get; set; }
        public string Classification { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A_Product> A_Product { get; set; }
    }
}
