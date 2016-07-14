using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SiGLDB;

namespace SiGLServices.Resources
{
    public class site_list_view
    {
        public Int32 site_id { get; set; }
        public Nullable<DateTime> start_date { get; set; }
        public Nullable<DateTime> end_date { get; set; }
        public Nullable<Int32> project_id { get; set; }
        public string sample_platform { get; set; }
        public string additional_info { get; set; }
        public string name { get; set; }
        public string description { get; set; }       
        public Nullable<double> latitude { get; set; }
        public Nullable<double> longitude { get; set; }
        public string waterbody { get; set; }
        public Nullable<Int32> status_type_id { get; set; }
        public string status { get; set; }
        public Nullable<Int32> lake_type_id { get; set; }
        public string lake { get; set; }
        public string country { get; set; }
        public string state_province { get; set; }
        public string watershed_huc8 { get; set; }
        public string url { get; set; }
    }


    public class FullSite
    {
        public decimal SiteId { get; set; }
        public Nullable<Int32> ProjectId { get; set; }
        public string SamplePlatform { get; set; }
        public string AdditionalInfo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Waterbody { get; set; }
        public Nullable<Int32> status_type_id { get; set; }
        public string Status { get; set; }
        public Nullable<Int32> lake_type_id { get; set; }
        public string Lake { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Watershed { get; set; }
        public string url { get; set; }
        public List<resource_type> Resources { get; set; }
        public List<media_type> Media { get; set; }
        public List<frequency_type> Frequencies { get; set; }
        public List<parameter_type> Parameters { get; set; }
    }
}