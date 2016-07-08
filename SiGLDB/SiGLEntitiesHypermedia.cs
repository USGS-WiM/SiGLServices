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
    public partial class PROJECT : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class PROJECT_CONTACTS : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class PROJECT_COOPERATORS : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class PROJECT_KEYWORDS : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class PROJECT_OBJECTIVES : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class PROJECT_PUBLICATIONS : IHypermedia
    {
        public List<Link> Links { get; set; }
    }   
    public partial class PUBLICATION : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class RESOURCE_TYPE : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class ROLE : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class SECTION : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class SITE : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class SITE_FREQUENCY : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class SITE_MEDIA : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class SITE_PARAMETERS : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class SITE_RESOURCE : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
    public partial class STATUS_TYPE : IHypermedia
    {
        public List<Link> Links { get; set; }
    }
}
