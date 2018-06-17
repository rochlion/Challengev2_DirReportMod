using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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
        public void CreateCompensationAndGetCompensationEmployeePresent_Returns_Created()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",  // Paul McCartney
                Salary = 15000000,
                EffectiveDate = DateTime.Now
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            /////////////////////////////////////////////////////////////////
            // Test the Create EndPoint with newly created Compensation GUID
            /////////////////////////////////////////////////////////////////
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();

            // Validate response results
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);

            /////////////////////////////////////////////////////////////////
            // Test the Get EndPoint with newly created Compensation GUID
            /////////////////////////////////////////////////////////////////
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{newCompensation.CompensationId}");
            var getResponse = getRequestTask.Result;
            var newCompensationGet = getResponse.DeserializeContent<Compensation>();

            // Validate Get results
            Assert.IsNotNull(newCompensationGet.CompensationId);
            Assert.AreEqual(compensation.Salary, newCompensationGet.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensationGet.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensationGet.EmployeeId);
        }

    }
}
