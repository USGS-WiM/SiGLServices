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
    
    public partial class publication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public publication()
        {
            this.project_publication = new HashSet<project_publication>();
        }
    
        public int publication_id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string science_base_id { get; set; }
        public string description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_publication> project_publication { get; set; }
    }
}
