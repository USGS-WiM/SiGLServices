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
}

