﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SiGLDB;

namespace SiGLServices.Resources
{
    public class project_list
    {
        public Int32 project_id { get; set; }
    }
    public class project_sitecount_view
    {
        public Int32 project_id { get; set; }
        public Nullable<Int32> data_manager_id { get; set; }
        public string name { get; set; }
        public Nullable<Int32> site_count { get; set; }
        public string lname { get; set; }
        public string fname {get;set;}
        public string organization_name { get; set; }
        public Nullable<DateTime> last_edited_stamp { get; set; }
        public Nullable<DateTime> created_stamp { get; set; }
    }

    public class FullProject
    {
        public decimal ProjectId { get; set; }
        public string ScienceBaseId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<Int32> status_id { get; set; }
        public string Status { get; set; }
        public Nullable<Int32> duration_id { get; set; }
        public string Duration { get; set; }
        public decimal DataManagerId { get; set; }
        public string Description { get; set; }
        public string AdditionalInfo { get; set; }
        public List<objective_type> Objectives { get; set; }
        public List<monitoring_coordination> MonitoringCoords { get; set; }
        public List<keyword> Keywords { get; set; }
        public string ProjectWebsite { get; set; }
        public List<data_host> DataHosts { get; set; }
        public List<OrganizationResource> Organizations { get; set; }
        public List<contactResource> Contacts { get; set; }
        public List<publication> Publications { get; set; }
        public Nullable<DateTime> last_edited_stamp { get; set; }
        public Nullable<DateTime> created_stamp { get; set; }
        public Nullable<Int32> ready_flag { get; set; }
        public List<SimpleSite> projectSites { get; set; }
    }

    public class FilteredProject
    {
        public string name {get; set;}
        public Int32 project_id { get; set; }
        public IEnumerable <string> Organizations { get; set; }
        public List<SimpleSite> projectSites { get; set; }
    }
}