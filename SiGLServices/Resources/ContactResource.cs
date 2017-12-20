using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SiGLDB;

namespace SiGLServices.Resources
{
    [XmlInclude(typeof(contact))]
    public class contactResource : contact
    {
        public string ContactOrgName { get; set; }
        public string ContactDivName { get; set; }
        public string ContactSecName { get; set; }
        public Int32 org_id { get; set; }
        public Int32 div_id { get; set; }
        public Int32 sec_id { get; set; }
    }
}