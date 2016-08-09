using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SiGLDB;

namespace SiGLServices.Resources
{
    public class dm_list_view
    {
        public Int32 data_manager_id { get; set; }
        public string username {get;set;}
        public string phone {get;set;}
        public string email {get;set;}
        public Nullable<Int32> role_id { get; set; }
        public Nullable<Int32> organization_system_id { get; set; }
        public string fullname { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string role_name { get; set; }
        public Nullable<Int32> project_count { get; set; }
    }
}