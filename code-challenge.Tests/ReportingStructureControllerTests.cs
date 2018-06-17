using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Collections.Generic;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetReportingStructureById_Returns_CorrectCount()
        {
            /////////////////////////////////////////////
            // Top Node, multiple levels of Direct Report
            /////////////////////////////////////////////
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f"; // JohnLennon
            var expectedReportCount = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            //Assert
            var reportStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReportCount, reportStructure.NumberOfReports);

            ////////////////////////////////
            // Bottom Node, no Direct Report
            ////////////////////////////////
            employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3"; // Paul McCartney
            expectedReportCount = 0;

            // Execute
            getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            response = getRequestTask.Result;

            //Assert
            reportStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReportCount, reportStructure.NumberOfReports);
        }

        [TestMethod]
        public void GetReportingStructureById_Returns_CorrectReports()
        {
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f"; // JohnLennon
            List<string> expectedReports = new List<string>();
            expectedReports.Add("b7839309-3348-463b-a7e3-5de1c168beb3"); // Paul
            expectedReports.Add("03aa1462-ffa9-4978-901b-7c001562cf6f"); // Ringo
            expectedReports.Add("62c1084e-6e34-4630-93fd-9153afb65309"); // Pete
            expectedReports.Add("c0c2293d-16bd-4603-8e08-638a9d18b22c"); // George


            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            //Assert
            var reportStructure = response.DeserializeContent<ReportingStructure>();
            foreach(string report in expectedReports)
            {
                Assert.IsTrue(ReportInEmployeeStructure(reportStructure.Employee, report));
            }
        }

        private bool ReportInEmployeeStructure(Employee employee, string reportId)
        {
            bool inStructure = false;

            if (employee.DirectReports == null)
                return inStructure;
            else
            {
                foreach (Employee report in employee.DirectReports)
                {
                    if (report.EmployeeId == reportId)
                    {
                        inStructure = true;
                        break;
                    }

                    inStructure = ReportInEmployeeStructure(report, reportId);
                    if (inStructure)
                    {
                        break;
                    }
                }
            }

            return inStructure;
        }
    }
}
