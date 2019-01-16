using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace UI.Pages
{
    public class ResponsePage : BasePage
    {
        public ResponsePage(IWebDriver webDriver) : base(webDriver)
        {
            webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
        }
        public string VerifyPartialResponse(string projectname)
        {
            var allrows = WebDriver.FindElement(By.Id("TblAdminReport"));
            var reqrow = allrows.FindElements(By.XPath(".//tbody/tr"));
            var reqproj = reqrow.First(x => x.FindElement(By.XPath("./td[@class = 'text-left']")).Text.Contains(projectname));
            SelectElement selectElement = new SelectElement(reqproj.FindElement(By.XPath(".//select[@name = 'ddlquestion']")));
            var selectvalue = selectElement.SelectedOption.Text;
            return selectvalue;
        }
        public string VerifyFullResponse(string projectname)
        {
            var allrows = WebDriver.FindElement(By.Id("TblAdminReport"));
            var reqrow = allrows.FindElements(By.XPath(".//tbody/tr"));
            var reqproj = reqrow.First(x => x.FindElement(By.XPath("./td[@class = 'text-left']")).Text.Contains(projectname));
            var input = reqproj.FindElement(By.XPath(".//input[@type = 'text']")).GetAttribute("value");
            return input;
        }
    }
}
