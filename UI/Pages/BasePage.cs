using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using UI.Wrappers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UI.Pages
{
    public class BasePage : IPage
    {
        protected IWebDriver WebDriver { get; set; }

        public static log4net.ILog Log { get; set; }

        public BasePage(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }

        public string Title
        {
            get { return WebDriver.Title; }
        }

        public string Url
        {
            get { return WebDriver.Url; }
        }

        public void AcceptDialog()
        {
            WebDriver.SwitchTo().Alert().Accept();
        }

        public void DismissAlert()
        {
            WebDriver.SwitchTo().Alert().Dismiss();
        }
        public void BrowserRefresh()
        {
            WebDriver.Navigate().Refresh();
        }
        public void CancelDialog()
        {
            WebDriver.SwitchTo().Alert().Dismiss();
        }
        public T NavigateTo<T>(string path) where T : IPage
        {
            var items = path.Split(new[] { " > " }, StringSplitOptions.None).Select(i => i.Trim()).ToList();

            var topMenu = WebDriver.FindElements(By.XPath("//div[@class='headMainMenu']/ul/li"));

            var parentDropDown = topMenu.First(x => x.Text != "" && x.FindElement(By.XPath("./a")).Text == items[0].ToUpper());

            parentDropDown.Click();

            if (items.Count == 2)

                parentDropDown.FindElements(By.XPath(".//div[@class = 'dropdown-content']/a")).First(x => x.Text == items[1].ToUpper()).Click();

            var page = PageFactory.Create<T>(WebDriver);

            page.WaitLoading();

            return page;
        }
        public void Quit()
        {
            WebDriver.Quit();
        }
        public void WaitLoading()
        {
            WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings["Timeout"])));
            wait.Until(driver => (bool)driver.Scripts().ExecuteScript("return document.readyState == 'complete'"));
        }
        public void Close()
        {

        }
        public void SwitchToDefaultWindow()
        {
            WebDriver.SwitchTo().DefaultContent();
        }

        public void WaitForSpinnerDisappear()
        {
            WebDriverWait waitForSpinner = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings["Timeout"])));
            waitForSpinner.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@class='k-loading-image']")));
        }
    }
}
