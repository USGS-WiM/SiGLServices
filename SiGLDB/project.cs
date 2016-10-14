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
    
    public partial class project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public project()
        {
            this.sites = new HashSet<site>();
            this.project_objectives = new HashSet<project_objectives>();
            this.project_contacts = new HashSet<project_contacts>();
            this.project_cooperators = new HashSet<project_cooperators>();
            this.data_host = new HashSet<data_host>();
            this.project_publication = new HashSet<project_publication>();
            this.project_keywords = new HashSet<project_keywords>();
            this.project_monitor_coord = new HashSet<project_monitor_coord>();
        }
    
        public int project_id { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public string url { get; set; }
        public string additional_info { get; set; }
        public int data_manager_id { get; set; }
        public string science_base_id { get; set; }
        public string description { get; set; }
        public Nullable<int> proj_status_id { get; set; }
        public Nullable<int> proj_duration_id { get; set; }
        public Nullable<int> ready_flag { get; set; }
        public Nullable<System.DateTime> created_stamp { get; set; }
        public Nullable<System.DateTime> last_edited_stamp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<site> sites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_objectives> project_objectives { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_contacts> project_contacts { get; set; }
        public virtual proj_duration proj_duration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_cooperators> project_cooperators { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<data_host> data_host { get; set; }
        public virtual proj_status proj_status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_publication> project_publication { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_keywords> project_keywords { get; set; }
        public virtual data_manager data_manager { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_monitor_coord> project_monitor_coord { get; set; }
    }
}
