using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SiGLDB;

namespace SiGLServices.Resources
{
    //[XmlInclude(typeof(project))]
    //public class projectResource : project
    //{
    //    public List<objective_type> Objectives { get; set; }
    //    public List<keyword> Keywords { get; set; }
    //    public data_host datahost { get; set; }
    //    public List<OrganizationResource> Organizations { get; set; }
    //    public List<contactResource> Contacts { get; set; }
    //    public List<publication> publication { get; set; }
    //}//end class projectResource

    public class project_sitecount_view
    {
        public Int32 project_id { get; set; }
        public Nullable<Int32> data_manager_id { get; set; }
        public string name { get; set; }
        public Nullable<Int32> site_count { get; set; }
        public string lname { get; set; }
        public string fname {get;set;}
        public string organization_name { get; set; }
    }

    public class FullProject
    {
        public decimal ProjectId { get; set; }
        public string ScienceBaseId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal DataManagerId { get; set; }
        public string Description { get; set; }
        public List<objective_type> Objectives { get; set; }
        public List<keyword> Keywords { get; set; }
        public string ProjectWebsite { get; set; }
        public data_host DataHost { get; set; }
        public List<OrganizationResource> Organizations { get; set; }
        public List<contactResource> Contacts { get; set; }
        public List<publication> Publications { get; set; }
    }
}