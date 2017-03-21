using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using System.IO;

using WiM.Utilities;
using WiM.Utilities.ServiceAgent;
using WiM.Security;
using WiM.Exceptions;

using WiM.Utilities.Storage;
using WiM.Resources;

using SiGLDB;

using Newtonsoft.Json;
using OpenRasta.IO;
using OpenRasta.Web;

//using SiGLServices.Resources;
using Ionic.Zip;


namespace SiGLServices.Utilities.ServiceAgent
{
    internal class SiGLAgent : DBAgentBase
    {
        #region "Events"
        #endregion
        #region "Properties"
        #endregion
        #region "Collections & Dictionaries"

        #endregion
        #region "Constructor and IDisposable Support"
        #region Constructors
        internal SiGLAgent(Boolean include = false)
            : this(ConfigurationManager.AppSettings["Username"], new EasySecureString(ConfigurationManager.AppSettings["Password"]), include)            
        {
        }
        internal SiGLAgent(string username, EasySecureString password, Boolean include = false)
            : base(ConfigurationManager.AppSettings["SiGLEntities"])
        {
            this.context = new SiGLEntities(string.Format(connectionString, ConfigurationManager.AppSettings["Username"], new EasySecureString(ConfigurationManager.AppSettings["Password"]).decryptString()));
            this.context.Configuration.ProxyCreationEnabled = include;
        }
        #endregion
        #region IDisposable Support
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (disposing)
                {
                    // TODO:Dispose managed resources here.
                    //ie component.Dispose();

                }//EndIF

                // TODO:Call the appropriate methods to clean up
                // unmanaged resources here.
                //ComRelease(Extent);

                // Note disposing has been done.
                disposed = true;


            }//EndIf
        }//End Dispose
        #endregion
        #endregion
        #region "Methods"

        #endregion
        #region "Helper Methods"
        public IQueryable<T> getTable<T>(object[] args) where T : class,new()
        {
            try
            {
                string sql = String.Format(getSQLStatement(typeof(T).Name), args);
                return context.Database.SqlQuery<T>(sql).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string getSQLStatement(string type)
        {
            string sql = string.Empty;
            switch (type)
            {
                case "dm_list_view":
                    return @"SELECT dm.data_manager_id, dm.username, dm.phone, dm.email, dm.role_id, dm.organization_system_id, (dm.fname::text || ' '::text) || dm.lname::text AS fullname, dm.fname, dm.lname, 
                                r.role_name, count(p.data_manager_id) AS project_count
                            FROM lampadmin.data_manager dm
                            LEFT JOIN lampadmin.project p ON p.data_manager_id::numeric = dm.data_manager_id::numeric
                            LEFT JOIN lampadmin.role r ON r.role_id::numeric = dm.role_id::numeric
                            GROUP BY dm.data_manager_id, dm.organization_system_id, dm.fname, dm.lname, p.data_manager_id, r.role_name;";
                case "project_sitecount_view":
                    return @"SELECT p.project_id, p.data_manager_id, p.last_edited_stamp, p.created_stamp, p.name, count(s.project_id) AS site_count, dm.lname, dm.fname, o.organization_name
                            FROM lampadmin.project p
                            LEFT JOIN lampadmin.site s ON p.project_id = s.project_id
                            LEFT JOIN lampadmin.data_manager dm ON p.data_manager_id = dm.data_manager_id
                            LEFT JOIN lampadmin.organization_system orgS ON dm.organization_system_id = orgS.organization_system_id
                            LEFT JOIN lampadmin.organization o ON orgS.org_id = o.organization_id
                            GROUP BY p.project_id, p.name, p.data_manager_id, dm.lname, dm.fname, o.organization_name;";
                case "site_list_view":
                    return @"SELECT s.site_id, s.start_date, s.end_date, s.project_id, s.sample_platform, s.additional_info, s.name, s.description, s.latitude, s.longitude, s.waterbody, 
                            s.status_type_id, st.status, s.lake_type_id, l.lake, s.country, s.state_province, s.watershed_huc8, s.url
                            FROM lampadmin.site s
                            LEFT JOIN lampadmin.status_type st ON st.status_id = s.status_type_id
                            LEFT JOIN lampadmin.lake_type l ON l.lake_type_id = s.lake_type_id;";
                case "full_organization":
                    return @"SELECT o.organization_id, o.organization_name, d.division_id, d.division_name, s.section_id, s.section_name
                            FROM lampadmin.organization o
                            LEFT JOIN lampadmin.division d on o.organization_id = d.org_id 
                            LEFT JOIN lampadmin.section s on d.division_id = s.div_id
                            ORDER BY o.organization_name, d.division_name, s.section_name;";
                default:
                    throw new Exception("No sql for table " + type);
            }//end switch;

        }

        #endregion

        #region "Structures"
        //A structure is a value type. When a structure is created, the variable to which the struct is assigned holds
        //the struct's actual data. When the struct is assigned to a new variable, it is copied. The new variable and
        //the original variable therefore contain two separate copies of the same data. Changes made to one copy do not
        //affect the other copy.

        //In general, classes are used to model more complex behavior, or data that is intended to be modified after a
        //class object is created. Structs are best suited for small data structures that contain primarily data that is
        //not intended to be modified after the struct is created.
        #endregion
        #region "Asynchronous Methods"

        #endregion
        #region "Enumerated Constants"
        #endregion




    }//end class
}//end namespace
