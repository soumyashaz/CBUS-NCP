using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using UI.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenQA.Selenium.Remote;

namespace UI
{
    
    public static class CBUSAWebApp
    {
        public static HomePage Open()
        {
            IWebDriver webDriver = null;

            if (ConfigurationManager.AppSettings["Browser"].ToLower() == "firefox")
            {
                FirefoxOptions firefoxProfile = new FirefoxOptions();
                firefoxProfile.SetPreference("browser.download.folderList", 2);
                firefoxProfile.SetPreference("browser.download.manager.showWhenStarting", false);
                firefoxProfile.SetPreference("browser.download.dir", Environment.CurrentDirectory);
                firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
                webDriver = new FirefoxDriver(firefoxProfile);
            }

            if (ConfigurationManager.AppSettings["Browser"].ToLower() == "chrome")
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddUserProfilePreference("download.default_directory", Environment.CurrentDirectory);
                //DesiredCapabilities dc = new DesiredCapabilities();
                //dc.SetCapability(CapabilityType.UnexpectedAlertBehavior, ChromerUnexpectedAlertBehavior.Accept);
                webDriver = new ChromeDriver(chromeOptions);
                // Console.WriteLine("Driver Found" + Environment.GetEnvironmentVariable("ChromeWebDriver"));
               // webDriver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"));
            }

            if (ConfigurationManager.AppSettings["Browser"].ToLower() == "internetexplorer")
            {
                webDriver = new InternetExplorerDriver();
            }

            if(ConfigurationManager.AppSettings["Browser"].ToLower() == "edge")
            {
                EdgeOptions edgeOptions = new EdgeOptions();
                webDriver = new EdgeDriver(edgeOptions);
            }

            webDriver.Manage().Window.Maximize();
            webDriver.Navigate().GoToUrl(ConfigurationManager.AppSettings["Environment"]);
            var homePage = PageFactory.Create<HomePage>(webDriver);
            homePage.WaitLoading();
            return homePage;
        }
    }
}
