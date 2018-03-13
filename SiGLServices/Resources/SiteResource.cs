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
        public string name { get; set; }
        public Nullable<Int32> project_id { get; set; }
        public Nullable<double> latitude { get; set; }
        public Nullable<double> longitude { get; set; }
        public dynamic geom { get; set; }
        public Nullable<Int32> lake_type_id { get; set; }
        public string state_province { get; set; }
        public string proj_duration_id { get; set; }
        public string proj_status_id { get; set; }
        public string project_name { get; set; }
        public string organization_system_id { get; set; }
        public string objective_id { get; set; }
        public string monitoring_coordination_id { get; set; }
        public string media_type_id { get; set; }
        public string resource_type_id { get; set; }
        public string parameter_type_id { get; set; }
    }


    public class FullSite
    {
        public decimal SiteId { get; set; }
        public Nullable<Int32> ProjectId { get; set; }
        public string SamplePlatform { get; set; }
        public string AdditionalInfo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<double> latitude { get; set; }
        public Nullable<double> longitude { get; set; }
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

        public FullSite() { }

        public FullSite(site s)
        {
            this.SiteId = s.site_id;
            this.StartDate = s.start_date.ToString();
            this.EndDate = s.end_date.ToString();
            this.ProjectId = s.project_id;
            this.SamplePlatform = s.sample_platform;
            this.AdditionalInfo = s.additional_info;
            this.Name = s.name;
            this.Description = s.description;
            this.latitude = s.latitude;
            this.longitude = s.longitude;
            this.Waterbody = s.waterbody;
            this.status_type_id = s.status_type_id;
            this.lake_type_id = s.lake_type_id;
            this.Country = s.country;
            this.State = s.state_province;
            this.Watershed = s.watershed_huc8;
            this.url = s.url;
        }
    }

    public class SimpleSite
    {
        public Int32 site_id { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Int32 project_id { get; set; }
		public string Lake { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
	}


}