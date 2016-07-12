using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiM.Hypermedia;

namespace SiGLDB
{
    public partial class contacts : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class data_host : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class data_manager : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class division : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class frequency_type : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class keyword : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class lake_type : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class media_type : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class objective_type : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class organization : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class organization_system : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class parameter_type : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class proj_status : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class proj_duration : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class project : IHypermedia
    {
        public List<Link> Links { get; set; }
    }  
    public partial class publication : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class resource_type : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class role : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class section : IHypermedia
    {
        public List<Link> Links { get; set; }
    }  
    public partial class site : IHypermedia
    {
        public List<Link> Links { get; set; }
    }   
    public partial class status_type : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
}
