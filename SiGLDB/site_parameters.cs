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
    
    public partial class site_parameters
    {
        public int site_parameters_id { get; set; }
        public int site_id { get; set; }
        public int parameter_type_id { get; set; }
    
        public virtual site site { get; set; }
        public virtual parameter_type parameter_type { get; set; }
    }
}
