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
    
    public partial class A_DrugRegimen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_DrugRegimen()
        {
            this.Treatment_PatientSummary = new HashSet<Treatment_PatientSummary>();
        }
    
        public short RegimenCode { get; set; }
        public string RegimenDesc { get; set; }
        public string ExtraInfo { get; set; }
        public string DrugCombination { get; set; }
        public Nullable<short> RegimenCategoryCode { get; set; }
        public Nullable<int> RegimenClassification { get; set; }
        public Nullable<bool> StandardRegimen { get; set; }
        public Nullable<bool> IsExtra { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Treatment_PatientSummary> Treatment_PatientSummary { get; set; }
        public virtual A_DrugRegimenCategory A_DrugRegimenCategory { get; set; }
        public virtual A_DrugRegimenClassification A_DrugRegimenClassification { get; set; }
    }
}
