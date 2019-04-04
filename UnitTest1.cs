using System;
using NUnit.Framework;
using System.Net;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using SeleniumADTest;
using System.Diagnostics;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private const int WebdriverWaitForMilliseconds = 3000;
        private readonly NetworkCredential _credentials = new NetworkCredential("boniato", "boniato","xavi");
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Run the SeleniumProcessImpersonator under the test users credentials
            using (var impersonation = new SeleniumProcessImpersonator(_credentials))
            {
                // Wait for the driver to start
                if (impersonation.WaitForWebDriverStarted(WebdriverWaitForMilliseconds))
                {
                    var options = new ChromeOptions(); 
                    // Use RemoteWebDriver to connect to the process listening to default port 5555
                    using (var driver = new ChromeDriver()) //new RemoteWebDriver(/* new Uri("http://127.0.0.1"), */options.ToCapabilities())) //, new TimeSpan(0, 0, 30)))
                    {
                        // Navigate to system under test
                        const string desiredUri = "http://apps-d73b00e5dc1325.xavi.cat/sites/xavi/TPGTeamManagerDev/Pages/Default.aspx?SPHostUrl=http%3A%2F%2Fxaviserver%2Fsites%2Fxavi&SPLanguage=en-US&SPClientTag=0&SPProductNumber=16%2E0%2E4327%2E1000&SPAppWebUrl=http%3A%2F%2Fapps-d73b00e5dc1325%2Exavi%2Ecat%2Fsites%2Fxavi%2FTPGTeamManagerDev";
                        driver.Navigate().GoToUrl(desiredUri);
 
                        // Run your awesome tests here
                        Assert.AreEqual(desiredUri, driver.Url, "Navigated URL not as expected");
 
                        // Closing up
                        driver.Close();
                        driver.Quit();
                    }
                }
                else
                {
                    Assert.Inconclusive("WebDriver did not start in a timely fashion");
                }
            }
            Assert.Pass();
        }
        [TearDown]
        public void Cleanup() {
            KillOrphanedInternetDrivers();
        }
        private void KillOrphanedInternetDrivers() {
            var processes = Process.GetProcessesByName("IEDriverServer").ToList();
            processes.ForEach(p => p.Kill());
        }
    }
}