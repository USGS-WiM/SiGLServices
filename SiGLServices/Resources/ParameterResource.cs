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
    public class ParameterGroups
    {
        public List<parameter_type> Biological { get; set; }
        public List<parameter_type> Chemical { get; set; }
        public List<parameter_type> Microbiological { get; set; }
        public List<parameter_type> Physical { get; set; }
        public List<parameter_type> Toxicological { get; set; }
    }
}