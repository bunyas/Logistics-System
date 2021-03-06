 
namespace mascis
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_Product()
        {
            this.Order_DrugDetails = new HashSet<Order_DrugDetails>();
            this.order_hiv_rapid_test_kit = new HashSet<order_hiv_rapid_test_kit>();
            this.Order_MAULT = new HashSet<Order_MAULT>();
            this.Order_OI_STI_Detail = new HashSet<Order_OI_STI_Detail>();
            this.Order_SMC_SLM = new HashSet<Order_SMC_SLM>();
            this.Order_ViralLoadReagents_Detail = new HashSet<Order_ViralLoadReagents_Detail>();
            this.Order_DrugDetails_PhysicalCount = new HashSet<Order_DrugDetails_PhysicalCount>();
            this.order_hiv_rapid_test_kit_PhysicalCount = new HashSet<order_hiv_rapid_test_kit_PhysicalCount>();
            this.Order_OI_STI_Detail_Physical_Count = new HashSet<Order_OI_STI_Detail_Physical_Count>();
            this.Order_SMC_SLM_Physical_Count = new HashSet<Order_SMC_SLM_Physical_Count>();
            this.order_hiv_rapid_test_kit_summary = new HashSet<order_hiv_rapid_test_kit_summary>();
            this.Order_Lab_Custom = new HashSet<Order_Lab_Custom>();
            this.Order_RUTF_Details = new HashSet<Order_RUTF_Details>();
            this.Order_TB = new HashSet<Order_TB>();
        }
    
        public int product_code { get; set; }
        public string product_description { get; set; }
        public string strength { get; set; }
        public string packsize { get; set; }
        public Nullable<int> carton_size_pkt { get; set; }
        public Nullable<double> carton_weight_kg { get; set; }
        public Nullable<int> product_category { get; set; }
        public Nullable<short> Basic_Unit { get; set; }
        public Nullable<bool> approved_MOH_NTG { get; set; }
        public Nullable<bool> approved_WHO { get; set; }
        public Nullable<bool> approved_FDA { get; set; }
        public Nullable<int> nda_registration { get; set; }
        public string SAP_code { get; set; }
        public Nullable<bool> ConsummableProduct { get; set; }
        public Nullable<int> ART_product_classification { get; set; }
        public string NMS_Codes { get; set; }
        public Nullable<int> ProductStatusId { get; set; }
        public Nullable<bool> ProductExpires { get; set; }
        public Nullable<bool> TracerCommodity { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductClassification { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_DrugDetails> Order_DrugDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_hiv_rapid_test_kit> order_hiv_rapid_test_kit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_MAULT> Order_MAULT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_OI_STI_Detail> Order_OI_STI_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_SMC_SLM> Order_SMC_SLM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_ViralLoadReagents_Detail> Order_ViralLoadReagents_Detail { get; set; }
        public virtual A_product_category A_product_category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_DrugDetails_PhysicalCount> Order_DrugDetails_PhysicalCount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_hiv_rapid_test_kit_PhysicalCount> order_hiv_rapid_test_kit_PhysicalCount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_OI_STI_Detail_Physical_Count> Order_OI_STI_Detail_Physical_Count { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_SMC_SLM_Physical_Count> Order_SMC_SLM_Physical_Count { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_hiv_rapid_test_kit_summary> order_hiv_rapid_test_kit_summary { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Lab_Custom> Order_Lab_Custom { get; set; }
        public virtual A_DrugBasicUnit A_DrugBasicUnit { get; set; }
        public virtual A_ProductStatus A_ProductStatus { get; set; }
        public virtual A_ProductType A_ProductType { get; set; }
        public virtual A_Product_Classification A_Product_Classification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_RUTF_Details> Order_RUTF_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_TB> Order_TB { get; set; }
    }
}
