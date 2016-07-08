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
            : this("pgadmin", new EasySecureString("***REMOVED***"), include)
        {
        }
        internal SiGLAgent(string username, EasySecureString password, Boolean include = false)
            : base(ConfigurationManager.ConnectionStrings["SiGLEntities"].ConnectionString)
        {
            this.context = new SiGLEntities(string.Format(connectionString, "pgadmin", new EasySecureString("***REMOVED***").decryptString()));
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
                    return @"SELECT * FROM lampadmin.dm_list_view;";
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
