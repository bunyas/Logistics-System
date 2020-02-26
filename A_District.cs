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
    
    public partial class A_District
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_District()
        {
            this.A_Facilities = new HashSet<A_Facilities>();
        }
    
        public int District_Code { get; set; }
        public string District_Name { get; set; }
        public string Region_Id { get; set; }
        public Nullable<bool> Is_Urban { get; set; }
        public string District_Ministry_Code { get; set; }
        public Nullable<bool> Is_Municipality { get; set; }
        public string ISO_Code { get; set; }
        public Nullable<int> CDCRegionId { get; set; }
        public Nullable<int> ImplimentingPartnerCode { get; set; }
    
        public virtual A_CDCRegion A_CDCRegion { internal get; set; }
        public virtual A_ImplimentingPartners A_ImplimentingPartners { internal get; set; }
        public virtual A_Region A_Region { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A_Facilities> A_Facilities { get; set; }
    }
}