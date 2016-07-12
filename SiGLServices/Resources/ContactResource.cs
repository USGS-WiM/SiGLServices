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
    }
}