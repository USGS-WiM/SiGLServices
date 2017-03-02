using System;
using SiGLDB;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Configuration;

namespace SiGLDB.Test
{
    [TestClass]
    public class SiGLDBTest
    {
        //test      
        private string connectionString = String.Format(ConfigurationManager.AppSettings["SiGLEntities"], ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
        
        [TestMethod]
        public void SiGLDBConnectionTest()
        {
            using (SiGLEntities context = new SiGLEntities(connectionString))
            {
                DbConnection conn = context.Database.Connection;
                try
                {
                    if (!context.Database.Exists()) throw new Exception("db does not exist");
                    conn.Open();
                    Assert.IsTrue(true);

                }
                catch (Exception ex)
                {
                    Assert.IsTrue(false, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                }

            }
        }//end NSSDBConnectionTest

        [TestMethod]
        public void SiGLDBQueryTest()
        {
            using (SiGLEntities context = new SiGLEntities(connectionString))
            {
                try
                {
                    var testQuery = context.divisions.ToList();
                    Assert.IsNotNull(testQuery, testQuery.Count.ToString());
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(false, ex.Message);
                }

            }
        }//end NSSDBConnectionTest
    }
}
