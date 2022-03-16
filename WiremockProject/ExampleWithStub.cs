using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace WiremockProject
{
    public class ExampleWithStub
    {
        private static WireMockServer stub;
        private static string baseUrl;

        [OneTimeSetUp]
        public static void StartServer()
        {
            ExtentReportUtility.SetupExtentReport("Regression Test Run", "Automation Testing Report");
            var port = new Random().Next(5000, 6000);
            baseUrl = "http://localhost:" + port;
            stub = WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] { "http://+:" + port },
                ReadStaticMappings = true
            });
        }

        [OneTimeTearDown]
        public static void StopServer()
        {
            stub.Stop();
        }

        [SetUp]
        public void CreateTest()
        {
            string name = TestContext.CurrentContext.Test.Name.ToString();
            ExtentReportUtility.CreateTest(TestContext.CurrentContext.Test.Name.ToString());
        }

        [TearDown]
        public void endTest()
        {
            try
            {
                var testStatus = TestContext.CurrentContext.Result.Outcome;
                //ExtentReportUtility.TestStatus(testStatus.ToString());
            }
            catch (Exception ex)
            {
                ExtentReportUtility.SetTestStatusFail(ex.Message.ToString());
            }
            finally
            {
                ExtentReportUtility.FlushReport();
            }
        }

        [Test]
        public void ValidateWiremock()
        {
            try
            {
                ExtentReportUtility.AssignCategory("Smoke");
                var bodyContent = new[]
                                    { new {id = 1 }, new {id = 2 } };
                stub.Given(
                    Request
                    .Create()
                        .WithPath("/api/products"))
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader("Content-Type", "aplication/json")
                            .WithBodyAsJson(bodyContent));

                var client = new RestClient(baseUrl);
                var request = new RestRequest("/api/products");
                var response = client.Execute(request);
                Assert.AreEqual(200, (int)response.StatusCode);
                Assert.AreEqual(JsonConvert.SerializeObject(bodyContent), response.Content, "Failed : Response is not matching");
                ExtentReportUtility.LogReport(AventStack.ExtentReports.Status.Pass, "Response is matching successfully - " + response.Content);
            }
            catch (Exception ex)
            {
                ExtentReportUtility.SetTestStatusFail(ex.Message.ToString());
            }
        }

        [Test]
        public void ValidateResponse()
        {
            try
            {
                ExtentReportUtility.AssignCategory("Regression");
                var bodyContent = new[]
                                    { new {id = 1 }, new {id = 2 } };
                stub.Given(
                    Request
                    .Create()
                        .WithPath("/api/products"))
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader("Content-Type", "aplication/json")
                            .WithBodyAsJson(bodyContent));

                var client = new RestClient(baseUrl);
                var request = new RestRequest("/api/products");
                var response = client.Execute(request);
                Assert.AreEqual(200, (int)response.StatusCode);
                Assert.AreEqual(JsonConvert.SerializeObject(bodyContent), response.Content, "Failed : Response is not matching");
                ExtentReportUtility.LogReport(AventStack.ExtentReports.Status.Pass, "Response is matching successfully - " + response.Content);
            }
            catch (Exception ex)
            {
                ExtentReportUtility.SetTestStatusFail(ex.Message.ToString());
            }
        }
    }
}