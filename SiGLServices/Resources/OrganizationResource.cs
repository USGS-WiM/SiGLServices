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
}