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
    
    public partial class A_yes_no
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_yes_no()
        {
            this.fo_complaint_investigation = new HashSet<fo_complaint_investigation>();
            this.fo_complaint_investigation_Qual_Issue = new HashSet<fo_complaint_investigation_Qual_Issue>();
            this.fo_Certificate_Release_frm_Quarantine = new HashSet<fo_Certificate_Release_frm_Quarantine>();
            this.fo_ClientFeedback = new HashSet<fo_ClientFeedback>();
            this.fo_ClientFeedback1 = new HashSet<fo_ClientFeedback>();
            this.fo_ClientFeedback2 = new HashSet<fo_ClientFeedback>();
            this.fo_complaint = new HashSet<fo_complaint>();
            this.fo_complaint1 = new HashSet<fo_complaint>();
        }
    
        public int yes_no_id { get; set; }
        public string yes_no_desc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_complaint_investigation> fo_complaint_investigation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_complaint_investigation_Qual_Issue> fo_complaint_investigation_Qual_Issue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_Certificate_Release_frm_Quarantine> fo_Certificate_Release_frm_Quarantine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_ClientFeedback> fo_ClientFeedback { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_ClientFeedback> fo_ClientFeedback1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_ClientFeedback> fo_ClientFeedback2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_complaint> fo_complaint { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fo_complaint> fo_complaint1 { get; set; }
    }
}