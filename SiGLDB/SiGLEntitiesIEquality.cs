using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGLDB
{
    public partial class contact : IEquatable<contact>
    {
        public bool Equals(contact other)
        {
            return (string.IsNullOrEmpty(other.name) || string.Equals(this.name, other.name, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(other.email) || string.Equals(this.email, other.email, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(other.phone) || string.Equals(this.phone, other.phone, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(other.science_base_id) || string.Equals(this.science_base_id, other.science_base_id, StringComparison.OrdinalIgnoreCase)) &&
                    (!other.organization_system_id.HasValue || other.organization_system_id <= 0 || this.organization_system_id == other.organization_system_id);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as contact);
        }
        public override int GetHashCode()
        {
            return (this.name + this.email + this.phone + this.science_base_id + this.organization_system_id).GetHashCode();

        }
    }
    public partial class data_manager : IEquatable<data_manager>
    {
        public bool Equals(data_manager other)
        {
            return string.Equals(this.username, other.username, StringComparison.OrdinalIgnoreCase) &&
                  (string.Equals(this.fname, other.fname, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.lname, other.lname, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.email, other.email, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.phone, other.phone, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as data_manager);
        }

        public override int GetHashCode()
        {
            return (this.username + this.fname + this.lname + this.email + this.phone).GetHashCode();
        }
    }
    public partial class data_host : IEquatable<data_host>
    {
        public bool Equals(data_host other)
        {
            return string.Equals(this.host_name, other.host_name, StringComparison.OrdinalIgnoreCase) &&
                  (string.Equals(this.portal_url, other.portal_url, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.description, other.description, StringComparison.OrdinalIgnoreCase)) &&
                  (!other.project_id.HasValue || other.project_id <= 0 || this.project_id == other.project_id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as data_host);
        }

        public override int GetHashCode()
        {
            return (this.host_name + this.description + this.portal_url + this.project_id).GetHashCode();
        }
    }
    public partial class division : IEquatable<division>
    {
        public bool Equals(division other)
        {
            return string.Equals(this.division_name, other.division_name, StringComparison.OrdinalIgnoreCase) &&                  
                  (!other.org_id.HasValue || other.org_id <= 0 || this.org_id == other.org_id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as division);
        }

        public override int GetHashCode()
        {
            return (this.division_name + this.org_id).GetHashCode();
        }
    }
    public partial class frequency_type : IEquatable<frequency_type>
    {
        public bool Equals(frequency_type other)
        {
            return string.Equals(this.frequency, other.frequency, StringComparison.OrdinalIgnoreCase); 
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as frequency_type);
        }

        public override int GetHashCode()
        {
            return (this.frequency).GetHashCode();
        }
    }
    public partial class keyword : IEquatable<keyword>
    {
        public bool Equals(keyword other)
        {
            return string.Equals(this.term, other.term, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as keyword);
        }

        public override int GetHashCode()
        {
            return (this.term).GetHashCode();
        }
    }
    public partial class lake_type : IEquatable<lake_type>
    {
        public bool Equals(lake_type other)
        {
            return string.Equals(this.lake, other.lake, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as lake_type);
        }

        public override int GetHashCode()
        {
            return (this.lake).GetHashCode();
        }
    }
    public partial class media_type : IEquatable<media_type>
    {
        public bool Equals(media_type other)
        {
            return string.Equals(this.media, other.media, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as media_type);
        }

        public override int GetHashCode()
        {
            return (this.media).GetHashCode();
        }
    }
    public partial class objective_type : IEquatable<objective_type>
    {
        public bool Equals(objective_type other)
        {
            return string.Equals(this.objective, other.objective, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as objective_type);
        }

        public override int GetHashCode()
        {
            return (this.objective).GetHashCode();
        }
    }
    public partial class organization : IEquatable<organization>
    {
        public bool Equals(organization other)
        {
            return string.Equals(this.organization_name, other.organization_name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as organization);
        }

        public override int GetHashCode()
        {
            return (this.organization_name).GetHashCode();
        }
    }
    public partial class organization_system : IEquatable<organization_system>
    {
        public bool Equals(organization_system other)
        {
            return (other.org_id <= 0 || this.org_id == other.org_id) &&
                    (!other.div_id.HasValue || other.div_id <= 0 || this.div_id == other.div_id) &&
                    (!other.sec_id.HasValue || other.sec_id <= 0 || this.sec_id == other.sec_id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as organization_system);
        }

        public override int GetHashCode()
        {
            return (this.org_id).GetHashCode();
        }
    }
    public partial class parameter_type : IEquatable<parameter_type>
    {
        public bool Equals(parameter_type other)
        {
            return (string.Equals(this.parameter, other.parameter, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(this.parameter_group, other.parameter_group, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as parameter_type);
        }

        public override int GetHashCode()
        {
            return (this.parameter + this.parameter_group).GetHashCode();
        }
    }
    public partial class proj_status : IEquatable<proj_status>
    {
        public bool Equals(proj_status other)
        {
            return (string.Equals(this.status_value, other.status_value, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as proj_status);
        }

        public override int GetHashCode()
        {
            return (this.status_value).GetHashCode();
        }
    }
    public partial class proj_duration : IEquatable<proj_duration>
    {
        public bool Equals(proj_duration other)
        {
            return (string.Equals(this.duration_value, other.duration_value, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as proj_duration);
        }

        public override int GetHashCode()
        {
            return (this.duration_value).GetHashCode();
        }
    }
    public partial class project : IEquatable<project>
    {
        public bool Equals(project other)
        {
            return string.Equals(this.name, other.name, StringComparison.OrdinalIgnoreCase) &&
                  (!other.proj_duration_id.HasValue || other.proj_duration_id <= 0 || this.proj_duration_id == other.proj_duration_id) &&
                  (!other.proj_status_id.HasValue || other.proj_status_id <= 0 || this.proj_status_id == other.proj_status_id) &&
                  (!other.start_date.HasValue || DateTime.Equals(this.start_date.Value, other.start_date.Value)) &&
                  (!other.end_date.HasValue || DateTime.Equals(this.end_date.Value, other.end_date.Value)) &&
                  string.Equals(this.description, other.description, StringComparison.OrdinalIgnoreCase) &&
                  string.Equals(this.science_base_id, other.science_base_id, StringComparison.OrdinalIgnoreCase) &&
                  string.Equals(this.url, other.url, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as project);
        }

        public override int GetHashCode()
        {
            return (this.name + this.proj_duration_id + this.proj_status_id + this.start_date + this.end_date + this.description + this.science_base_id + this.url).GetHashCode();
        }
    }
    public partial class publication : IEquatable<publication>
    {
        public bool Equals(publication other)
        {
            return string.Equals(this.description, other.description, StringComparison.OrdinalIgnoreCase) &&
                  (string.Equals(this.title, other.title, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.url, other.url, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.science_base_id, other.science_base_id, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as publication);
        }

        public override int GetHashCode()
        {
            return (this.description + this.title + this.url + this.science_base_id).GetHashCode();
        }
    }
    public partial class resource_type : IEquatable<resource_type>
    {
        public bool Equals(resource_type other)
        {
            return string.Equals(this.resource_name, other.resource_name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as resource_type);
        }

        public override int GetHashCode()
        {
            return (this.resource_name).GetHashCode();
        }
    }
    public partial class role : IEquatable<role>
    {
        public bool Equals(role other)
        {
            return string.Equals(this.role_name, other.role_name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(this.role_description, other.role_description, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as role);
        }

        public override int GetHashCode()
        {
            return (this.role_name + this.role_description).GetHashCode();
        }
    }
    public partial class section : IEquatable<section>
    {
        public bool Equals(section other)
        {
            return string.Equals(this.section_name, other.section_name, StringComparison.OrdinalIgnoreCase) &&
                  (!other.div_id.HasValue || other.div_id <= 0 || this.div_id == other.div_id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as section);
        }

        public override int GetHashCode()
        {
            return (this.section_name + this.div_id).GetHashCode();
        }
    }
    public partial class site : IEquatable<site>
    {
        public bool Equals(site other)
        {
            return string.Equals(this.name, other.name, StringComparison.OrdinalIgnoreCase) &&
                  (!other.project_id.HasValue || this.project_id == other.project_id) &&
                  (other.latitude <= 0 || this.latitude == other.latitude) &&
                  (other.longitude <= 0 || this.longitude == other.longitude) &&
                  (string.Equals(this.country, other.country, StringComparison.OrdinalIgnoreCase)) &&
                  (!other.start_date.HasValue || DateTime.Equals(this.start_date.Value, other.start_date.Value)) &&
                  (!other.end_date.HasValue || DateTime.Equals(this.end_date.Value, other.end_date.Value)) &&
                  (string.Equals(this.sample_platform, other.sample_platform, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.additional_info, other.additional_info, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.waterbody, other.waterbody, StringComparison.OrdinalIgnoreCase)) &&
                  (string.Equals(this.state_province, other.state_province, StringComparison.OrdinalIgnoreCase)) &&
                  (!other.lake_type_id.HasValue || other.lake_type_id <= 0 || this.lake_type_id == other.lake_type_id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as site);
        }

        public override int GetHashCode()
        {
            return (this.name + this.latitude + this.longitude + this.country + this.start_date + this.end_date + this.sample_platform + this.additional_info + this.waterbody 
                + this.state_province + this.lake_type_id).GetHashCode();
        }
    }
}

