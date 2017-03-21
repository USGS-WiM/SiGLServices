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
    [XmlInclude(typeof(organization_system))]
    public class OrganizationResource : organization_system
    {
        public string OrganizationName { get; set; }
        public string DivisionName { get; set; }
        public string SectionName { get; set; }
    }//end class OrganizationResource

    [XmlInclude(typeof(organization))]
    public class full_organization : organization
    {
        public Nullable<Int32> division_id { get; set; }
        public string division_name { get; set; }
        public Nullable<Int32> section_id { get; set; }
        public string section_name { get; set; }
    }//end class OrganizationResource
   
}