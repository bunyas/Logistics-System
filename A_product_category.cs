 
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_product_category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_product_category()
        {
            this.A_Product = new HashSet<A_Product>();
            this.Order_MAULT_Header = new HashSet<Order_MAULT_Header>();
            this.Order_Lab_Custom_Header = new HashSet<Order_Lab_Custom_Header>();
            this.LMIS_ExpectedReport = new HashSet<LMIS_ExpectedReport>();
        }
    
        public int category_code { get; set; }
        public string category_desc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A_Product> A_Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_MAULT_Header> Order_MAULT_Header { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Lab_Custom_Header> Order_Lab_Custom_Header { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LMIS_ExpectedReport> LMIS_ExpectedReport { get; set; }
    }
}
