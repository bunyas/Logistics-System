 
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Product_Lab_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_Product_Lab_Category()
        {
            this.Order_Lab = new HashSet<Order_Lab>();
            this.Order_Lab_PhysicalCount = new HashSet<Order_Lab_PhysicalCount>();
        }
    
        public int product_category_code { get; set; }
        public Nullable<int> product_code { get; set; }
        public Nullable<int> category_2_code { get; set; }
        public Nullable<int> category_3_code { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Lab> Order_Lab { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Lab_PhysicalCount> Order_Lab_PhysicalCount { get; set; }
    }
}
