//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_OrderStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_OrderStatus()
        {
            this.Order_Header = new HashSet<Order_Header>();
            this.Order_Lab_Header = new HashSet<Order_Lab_Header>();
            this.Order_MAULT_Header = new HashSet<Order_MAULT_Header>();
            this.Order_OI_STI_Header = new HashSet<Order_OI_STI_Header>();
            this.Order_ViralLoadReagents_Header = new HashSet<Order_ViralLoadReagents_Header>();
            this.order_hiv_rapid_test_kit_header = new HashSet<order_hiv_rapid_test_kit_header>();
            this.Order_Lab_Custom_Header = new HashSet<Order_Lab_Custom_Header>();
            this.Order_SMC_SLM_Header = new HashSet<Order_SMC_SLM_Header>();
            this.Order_RUTF_Header = new HashSet<Order_RUTF_Header>();
            this.Order_TB_Header = new HashSet<Order_TB_Header>();
        }
    
        public int StatusId { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Header> Order_Header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Lab_Header> Order_Lab_Header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_MAULT_Header> Order_MAULT_Header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_OI_STI_Header> Order_OI_STI_Header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_ViralLoadReagents_Header> Order_ViralLoadReagents_Header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_hiv_rapid_test_kit_header> order_hiv_rapid_test_kit_header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Lab_Custom_Header> Order_Lab_Custom_Header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_SMC_SLM_Header> Order_SMC_SLM_Header { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_RUTF_Header> Order_RUTF_Header { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_TB_Header> Order_TB_Header { get; set; }
    }
}
