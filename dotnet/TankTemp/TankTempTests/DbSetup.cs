using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NDbUnit.Core.SqlClient;
using NDbUnit.Core;
using System.Configuration;

namespace TankTempTests
{
    [Binding]
    public class DbSetup
    {

        protected static SqlDbUnitTest SqlDbUnitTest;
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            SqlDbUnitTest = new SqlDbUnitTest(ConfigurationManager.ConnectionStrings["tanktemp"].ConnectionString);
            SqlDbUnitTest.ReadXmlSchema(@"schema.xsd");

            SqlDbUnitTest.ReadXml(@"testdata.xml");
            SqlDbUnitTest.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            SqlDbUnitTest.PerformDbOperation(DbOperationFlag.DeleteAll);
        }
    }
}
