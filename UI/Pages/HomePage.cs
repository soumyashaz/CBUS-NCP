using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver webDriver) : base(webDriver)
        {
            //WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings["Timeout"])));
            //wait.Until(ExpectedConditions.ElementExists(By.ClassName("headerMainMenu")));
            //WaitForSpinnerDisappear();
        }
        public AddProjectPage ClickNcpQuaterlyRebate(string headerrow)
        {
            WebDriver.FindElement(By.XPath("//div[@class = 'head-nav']/ul/li[3]")).Click();
            var addProject = PageFactory.Create<AddProjectPage>(WebDriver);
            addProject.WaitLoading();
            return addProject;
        }
        public ReportDashboardPage ClickNcp(string headerrow)
        {
            WebDriver.FindElement(By.XPath("//div[@class = 'head-nav']/ul/li[3]")).Click();
            var report = PageFactory.Create<ReportDashboardPage>(WebDriver);
            report.WaitLoading();
            return report;
        }
    }
}
