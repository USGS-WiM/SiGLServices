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
    
    public partial class project_objectives
    {
        public int project_objectives_id { get; set; }
        public Nullable<int> project_id { get; set; }
        public Nullable<int> objective_id { get; set; }
    
        public virtual objective_type objective_type { get; set; }
        public virtual project project { get; set; }
    }
}
