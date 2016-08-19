﻿using System;
using SiGLDB;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SiGLDB.Test
{
    [TestClass]
    public class SiGLDBTest
    {
        //test
       // private string connectionString = "***REMOVED***";
       // private string password = "***REMOVED***";
        //prod
        private string connectionString = "***REMOVED***";
        private string password = "***REMOVED***";

        [TestMethod]
        public void SiGLDBConnectionTest()
        {
            using (SiGLEntities context = new SiGLEntities(string.Format(connectionString, password)))
            {
                DbConnection conn = context.Database.Connection;
                try
                {
                    if (!context.Database.Exists()) throw new Exception("db does ont exist");
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
            using (SiGLEntities context = new SiGLEntities(string.Format(connectionString, password)))
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
