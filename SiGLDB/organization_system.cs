//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiGLDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class organization_system
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public organization_system()
        {
            this.contacts = new HashSet<contact>();
            this.project_cooperators = new HashSet<project_cooperators>();
        }
    
        public int organization_system_id { get; set; }
        public int org_id { get; set; }
        public Nullable<int> div_id { get; set; }
        public Nullable<int> sec_id { get; set; }
        public string science_base_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contact> contacts { get; set; }
        public virtual organization organization { get; set; }
        public virtual section section { get; set; }
        public virtual division division { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_cooperators> project_cooperators { get; set; }
    }
}
