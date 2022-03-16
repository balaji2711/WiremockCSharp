using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiremockProject
{
    public class ExtentReportUtility
    {
        public static ExtentReports ExtentReports;
        static string folderName = "WiremockReports";
        public static ExtentHtmlReporter ExtentHtmlReporter;
        public static ExtentTest TestCase; //Should not be static for parallel run
        public static string CurrentRunFolderName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        public static string filePath = @"C:/" + folderName + "/" + Environment.MachineName + "/" + CurrentRunFolderName;

        public static void SetupExtentReport(string reportName, string docTitle)
        {
            //ExtentV3HtmlReporter
            ExtentHtmlReporter = new ExtentHtmlReporter(filePath + "\\index.html");
            ExtentHtmlReporter.Config.Theme = Theme.Dark;
            ExtentHtmlReporter.Config.DocumentTitle = docTitle;
            ExtentHtmlReporter.Config.ReportName = reportName;

            ExtentReports = new ExtentReports();
            ExtentReports.AttachReporter(ExtentHtmlReporter);
            ExtentReports.AddSystemInfo("Machine", Environment.MachineName);
            ExtentReports.AddSystemInfo("OS", Environment.OSVersion.VersionString);
            ExtentReports.AddSystemInfo("Author", "Balaji G");
        }

        public static void CreateTest(string testName)
        {
            TestCase = ExtentReports.CreateTest(testName);
        }

        public static void LogReport(Status status, string message)
        {
            TestCase.Log(status, message);
        }

        public static void TestStatus(string status)
        {
            if (status.Equals("Passed"))
                TestCase.Pass("Test Passed");
            else if (status.Equals("Failed"))
                ExtentReportUtility.SetTestStatusFail();
            else
                ExtentReportUtility.SetTestStatusSkipped();
        }

        public static void AssignCategory(string category)
        {
            TestCase.AssignCategory(category);
        }

        public static void SetStepStatusPass(string stepDescription)
        {
            TestCase.Log(Status.Pass, stepDescription);
        }

        public static void SetTestStatusFail(string message = null)
        {
            var printMessage = "<p><b>Test FAILED!</b></p>";
            if (!string.IsNullOrEmpty(message))
            {
                printMessage += $"Message: <br>{message}<br>";
            }
            //var mediaEntity=ExtentReportUtility.CaptureScreenShot("Image_"+TestLogger.CurrentRunFolderName);
            TestCase.Fail(printMessage);
            //TestCase.Fail(printMessage,mediaEntity);
            //TestCase.AddScreenCaptureFromBase64String(ExtentReportUtility.ScreenCaptureAsBase64String(), "Screenshot on Error:");
        }

        public static void SetTestStatusSkipped()
        {
            TestCase.Skip("Test skipped!");
        }

        public static void SetStepStatusWarning(string stepDescription)
        {
            TestCase.Log(Status.Warning, stepDescription);
        }

        //public static string ScreenCaptureAsBase64String()
        //{
        //    ITakesScreenshot ts = (ITakesScreenshot)Application.driver;
        //    Screenshot screenshot = ts.GetScreenshot();
        //    return screenshot.AsBase64EncodedString;
        //}

        //public static MediaEntityModelProvider CaptureScreenShot(string name)
        //{
        //    var screenshot = ((ITakesScreenshot)Application.driver).GetScreenshot().AsBase64EncodedString;
        //    return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, name).Build();
        //}

        public static void FlushReport()
        {
            ExtentReports.Flush();
        }

        public static void AttachScreenshot()
        {
            //TestCase.AddScreenCaptureFromBase64String(ExtentReportUtility.ScreenCaptureAsBase64String(), "Screenshot on Error:");
        }
    }
}