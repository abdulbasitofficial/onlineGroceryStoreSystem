//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Online_Grocery_Order
{
    using System;
    using System.Collections.Generic;
    
    public partial class product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product()
        {
            this.attaches = new HashSet<attach>();
            this.carts = new HashSet<cart>();
            this.invoices = new HashSet<invoice>();
        }
    
        public int p_id { get; set; }
        public string p_name { get; set; }
        public Nullable<int> p_quantity { get; set; }
        public Nullable<int> p_weight { get; set; }
        public string p_description { get; set; }
        public Nullable<int> p_calories { get; set; }
        public byte[] p_img { get; set; }
        public Nullable<int> p_price { get; set; }
        public string p_img_type { get; set; }
        public Nullable<int> c_id { get; set; }
        public Nullable<System.DateTime> i_date { get; set; }
        public Nullable<int> p_unit { get; set; }
        public string p_offer { get; set; }
        public Nullable<int> p_discount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<attach> attaches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart> carts { get; set; }
        public virtual category category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoice> invoices { get; set; }
    }
}
