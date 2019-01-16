using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UI.Pages
{
    public class ReportDashboardPage : BasePage
    {
        public ReportDashboardPage(IWebDriver webDriver) : base(webDriver)
        {

        }
        public void ClickNoReport(string contractname)
        {
            Logger.Log.Debug("Fetching Elements");
            var allrows = WebDriver.FindElement(By.Id("listViewContractActiveAsc")).FindElements(By.XPath("./tr"));
            Logger.Log.Info("Fetching the Contract name.");
            var reqcontract = allrows.First(x => x.FindElement(By.XPath(".//td[3]")).Text.Contains(contractname));
            Logger.Log.Info("Report button clicked..");
            reqcontract.FindElement(By.Id("btnNoReport")).Click();
        }
        public string CheckStatus(string contractname)
        {
            Logger.Log.Debug("Fetching Elements");
            var allrows = WebDriver.FindElement(By.Id("listViewContractActiveAsc")).FindElements(By.XPath("./tr"));
            Logger.Log.Info("Fetching the Contract name.");
            var reqcontract = allrows.First(x => x.FindElement(By.XPath(".//td[3]")).Text.Trim().Contains(contractname));
            Logger.Log.Info("Fetching the Required Status.");
            var reqstatus = reqcontract.FindElement(By.XPath(".//div"));
            return reqstatus.Text;
        }
        public void VerifyEditButton(string contractname)
        {
            Logger.Log.Debug("Fetching Elements");
            var allrows = WebDriver.FindElement(By.Id("listViewContractActiveAsc")).FindElements(By.XPath("./tr"));
            Logger.Log.Info("Fetching the Contract name.");
            var reqcontract = allrows.First(x => x.FindElement(By.XPath(".//td[3]")).Text.Contains(contractname));
            Assert.AreEqual("EDIT", reqcontract.FindElement(By.XPath(".//td[6]/button[contains(@class, 'btnreport')]")).Text);
        }
        public string VerifyProgressBar()
        {
            Logger.Log.Debug("Fetching Element..");
            var reqprogress = WebDriver.FindElement(By.Id("prgbarReportStatus")).FindElement(By.XPath(".//span[@class = 'k-progress-status']"));
            return reqprogress.Text;
        }
        public void ClickEditOrContinue(string contractname)
        {
            Logger.Log.Debug("Fetching Element..");
            var allrows = WebDriver.FindElement(By.Id("listViewContractActiveAsc")).FindElements(By.XPath("./tr"));
            Logger.Log.Debug("Fetching the Contract name.");
            var reqcontract = allrows.First(x => x.FindElement(By.XPath(".//td[3]")).Text.Contains(contractname));
            Logger.Log.Debug("Report Button Clicked.");
            reqcontract.FindElement(By.XPath(".//td[6]/button[contains(@class, 'btnreport')]")).Click();
        }
        public void SelectProject(string projectname)
        {
            Logger.Log.Debug("Element Clicked.");
            WebDriver.FindElement(By.XPath("//span[@aria-owns = 'ContractManufacturer_listbox']")).Click();
            Logger.Log.Debug("Fetching the proper option.");
            var allrows = WebDriver.FindElement(By.Id("ContractManufacturer_listbox")).FindElements(By.XPath("./li[@role = 'option']"));
            Logger.Log.Debug("Element Clicked.");
            allrows.First(x => x.Text == projectname).Click();
        }
        public bool GetProjectRadioStatus(string projectname, string radiotext)
        {
            Logger.Log.Debug("Element Found..");
            var projecttDiv = WebDriver.FindElement(By.Id("project_table"));
            Logger.Log.Debug("Fetching the options..");
            var projectrows = projecttDiv.FindElements(By.XPath(".//tr[@role='option']"));
            Logger.Log.Debug("Fetching the project name..");
            var reqrow = projectrows.First(x => x.FindElement(By.XPath(".//td[3][contains(@class, 'text-center')]")).Text.Contains(projectname));
            Logger.Log.Debug("Fetching the desired desired element which contains radio text..");
            var reqcell = reqrow.FindElements(By.XPath(".//td")).First(x => x.Text.Contains(radiotext));
            Logger.Log.Debug("Fetching the radio button..");
            var reqradio = reqcell.FindElement(By.XPath(".//input[@type = 'radio']"));
            Logger.Log.Debug("Checking whether the radio button is checked or not..");
            if (reqradio.GetAttribute("checked") != null)
                return true;
            else
                return false;
        }
        public void ClickAddProject()
        {
            Logger.Log.Debug("Clicking the Add project button..");
            WebDriver.FindElement(By.Id("addproj")).Click();
        }
        public void ClickAddOrManageProjects()
        {
            Logger.Log.Debug("Clicking the Add or Manage projects button..");
            // WebDriver.FindElement(By.XPath("//div[@class = 'pull-left marginTop2']")).Click();
            WebDriver.FindElement(By.XPath("//a[@title = 'Add Projects']/div[@class = 'pull-left marginTop2']")).Click();
        }
        public void ClickNcpRebateReportHistory()
        {
            Logger.Log.Debug("Clicking the Rebate report history button..");
            WebDriver.FindElement(By.XPath("//a[@title = 'NCP Rebate Report History']/div[@class = 'pull-left marginTop2']")).Click();
        }
        public void ClickAddProjects()
        {
            WebDriver.FindElement(By.XPath("//div[@class = 'pull-left maginRight20']")).FindElement(By.XPath("/a[@title = 'Add Projects']")).Click();
        }
        public void InsertNumberOfProjects(string value)
        {
            Logger.Log.Debug("Element Located.");
            var numericinput = WebDriver.FindElement(By.XPath("//div[@class = 'col-sm-7']"));
            Logger.Log.Debug("numeric input clicked.");
            numericinput.Click();
            Logger.Log.Debug("Value cleared");
            numericinput.FindElement(By.XPath(".//input[@style = 'display: inline-block;']")).Clear();
            numericinput.FindElement(By.XPath(".//input[@style = 'display: inline-block;']")).SendKeys(value);
        }
        public void ClickAdd()
        {
            Logger.Log.Debug("Projects are not being added.");
            WebDriver.FindElement(By.Id("SaveTable")).Click();
        }
        public void InsertProjectDetails(int id, string projectvalue, string lotno, string address, string city, string zip)
        {
            Logger.Log.Debug("Id found.");
            var row = WebDriver.FindElement(By.Id("project_table"));
            Logger.Log.Debug("Elements counted");
            IList<IWebElement> element = row.FindElements(By.XPath(".//tr[@data-counter]"));
            Logger.Log.Debug("Element found");
            var reqrow = element.First(x => x.FindElement(By.XPath("./td")).Text == id.ToString());
            Logger.Log.Debug("Project value inserted.");
            reqrow.FindElement(By.XPath(".//input[contains(@class, 'projectname')]")).SendKeys(projectvalue);
            Logger.Log.Debug("Lotno value inserted.");
            reqrow.FindElement(By.XPath(".//input[contains(@class, 'lotno')]")).SendKeys(lotno);
            Logger.Log.Debug("Address value inserted.");
            reqrow.FindElement(By.XPath(".//input[contains(@class, 'address')]")).SendKeys(address);
            Logger.Log.Debug("City value inserted.");
            reqrow.FindElement(By.XPath(".//input[contains(@class, 'city')]")).SendKeys(city);
            Logger.Log.Debug("Zip value inserted.");
            reqrow.FindElement(By.XPath(".//input[contains(@class, 'zip')]")).SendKeys(zip);
        }
        public void SaveProject()
        {
            Logger.Log.Debug("Save button clicked.");
            WebDriver.FindElement(By.Id("btnSave")).Click();
            Thread.Sleep(3000);
        }
        public int VerifyProjectRows()
        {
            Logger.Log.Debug("Id found.");
            var row = WebDriver.FindElement(By.Id("project_table"));
            Logger.Log.Debug("Elements counted");
            IList<IWebElement> element = row.FindElements(By.XPath(".//tr[@role='option']"));
            var rowcount = element.Count();
            return rowcount;
        }
        public bool VerifyProjects(string projectname)
        {
            Logger.Log.Debug("Id found.");
            var allprojects = WebDriver.FindElement(By.Id("listViewActiveProject"));
            var projectrows = allprojects.FindElements(By.XPath(".//tr[@role = 'option']/td[3]"));
            if(projectrows!= null && projectrows.ToList().FindAll(x => x.Text == projectname).Count() > 0)
                return true;
            else
                return false;
        }
        public string VerifyContinueButton(string contractname)
        {
            var allrows = WebDriver.FindElement(By.Id("listViewContractActiveAsc")).FindElements(By.XPath("./tr"));
            var reqcontract = allrows.First(x => x.FindElement(By.XPath(".//td[3]")).Text.Contains(contractname));
            var reqbutton =  reqcontract.FindElement(By.XPath(".//td[6]/button[contains(@class, 'btnreport')]"));
            return reqbutton.Text;
        }
        public void ClickProjectStatus(string projectname, string radiotext)
        {
            var projecttDiv = WebDriver.FindElement(By.Id("project_table"));
            var projectrows = projecttDiv.FindElements(By.XPath(".//tr[@role='option']"));
            var reqrow = projectrows.First(x => x.FindElement(By.XPath(".//td[3][contains(@class, 'text-center')]")).Text == projectname);
            var reqcell = reqrow.FindElements(By.XPath(".//td")).First(x => x.Text.Contains(radiotext));
            var reqradio = reqcell.FindElement(By.XPath(".//input[@type = 'radio']"));
            reqradio.Click();
        }
        public SelectElement Option(string projectname)
        {
            var allrows = WebDriver.FindElement(By.Id("TblAdminReport"));
            var reqrow = allrows.FindElements(By.XPath(".//tbody/tr"));
            var reqproj = reqrow.First(x => x.FindElement(By.XPath("./td[@class = 'text-left']")).Text.Contains(projectname));
            SelectElement selectElement = new SelectElement(reqproj.FindElement(By.XPath(".//select[@name = 'ddlquestion']")));
            return selectElement;
        }
        public void InsertTextInFullResponse(string projectname, string value)
        {
            var allrows = WebDriver.FindElement(By.Id("TblAdminReport"));
            var reqrow = allrows.FindElements(By.XPath(".//tbody/tr"));
            var reqproj = reqrow.First(x => x.FindElement(By.XPath("./td[@class = 'text-left']")).Text.Contains(projectname));
            reqproj.FindElement(By.XPath(".//input[@type = 'text']")).SendKeys(value);
        }
        public void SaveResponse()
        {
            WebDriver.FindElement(By.Id("BtnSave")).Click();
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
        public void ClickPreviouslyReportedProjects()
        {
            WebDriver.FindElement(By.Id("prevreportedProj")).Click();
        }
        public void CheckReopenProjects(string projectname)
        {
            var allprojects = WebDriver.FindElement(By.Id("listViewPrevRptProject"));
            var projectrows = allprojects.FindElements(By.XPath(".//tr[@role = 'option']"));
            var reqrow = projectrows.First(x => x.FindElement(By.XPath(".//td[3][contains(@class, 'text-center')]")).Text.Contains(projectname));
            var reqchkbox = reqrow.FindElement(By.XPath(".//input[@type = 'checkbox']"));
            reqchkbox.Click();
        }
        public void ClickRepoen()
        {
            WebDriver.FindElement(By.Id("ReopenPrevProj")).Click();
        }
        public void ClickNCPQuaterlyRebate()
        {
            WebDriver.FindElement(By.XPath("//div[@class = 'head-nav']/ul/li[3]")).Click();
        }
        public void ClickNothingToReport()
        {
            WebDriver.FindElement(By.Id("btnNTRTQ")).Click();
        }
        public void VerifySubmitRebateReportEnabled()
        {
            Assert.AreEqual(true, WebDriver.FindElement(By.Id("submitreport")).Enabled, "Final Submit Report Button is Disabled");
        }
        public void VerifyUndefinedMessage()
        {
            Assert.AreEqual(false, WebDriver.FindElement(By.Id("DataUpdateMessage")).Displayed, "Undefined Error message is displayed.");
        }
        public void ClickFinalSubmitRebateReport()
        {
            WebDriver.FindElement(By.Id("submitreport")).Click();
        }
        public void ClickConfirmReport()
        {
            WebDriver.FindElement(By.Id("confirmreport")).Click();
        }
        public void VerifyEditButtonDisabled(string contractname)
        {
            var allrows = WebDriver.FindElement(By.Id("listViewContractActiveAsc")).FindElements(By.XPath("./tr"));
            var reqcontract = allrows.First(x => x.FindElement(By.XPath(".//td[3]")).Text.Contains(contractname));
            Assert.AreEqual(false, reqcontract.FindElement(By.XPath(".//td[6]/button[contains(@class, 'btnreport')]")).Enabled);
        }
        public void ClickViewHistoricalReports(string contractname)
        {
            var allrows = WebDriver.FindElement(By.Id("listViewContractActiveAsc")).FindElements(By.XPath("./tr"));
            var reqcontract = allrows.First(x => x.FindElement(By.XPath(".//td[3]")).Text.Contains(contractname));
            reqcontract.FindElement(By.Id("report")).Click();
        }
        public string VerifyClickPreview(string quartername)
        {
            var allrows = WebDriver.FindElement(By.Id("listViewreporthistory")).FindElements(By.XPath("./tr"));
            var reqquarter = allrows.First(x => x.FindElement(By.XPath(".//td[2]")).Text.Contains(quartername));
            var clickpreview = reqquarter.FindElement(By.XPath(".//a")).GetAttribute("title");
            return clickpreview;
        }
       public ResponsePage ClickPreview(string quartername)
        {
            var allrows = WebDriver.FindElement(By.Id("listViewreporthistory")).FindElements(By.XPath("./tr"));
            var reqquarter = allrows.First(x => x.FindElement(By.XPath(".//td[2]")).Text.Contains(quartername));
            reqquarter.FindElement(By.XPath(".//a")).Click();
            var response = PageFactory.Create<ResponsePage>(WebDriver);
            response.WaitLoading();
            return response;
        }
        public void ClickSetNoProjectStatus()
        {
            WebDriver.FindElement(By.Id("divConfirmReportedProjectStatus")).FindElement(By.XPath(".//div[@class ='modal-footer']/button[contains(text(), 'NO')]")).Click();
        }
        public void ClickSetYesProjectStatus()
        {
            WebDriver.FindElement(By.Id("divConfirmReportedProjectStatus")).FindElement(By.XPath(".//div[@class ='modal-footer']/button[contains(text(), 'YES')]")).Click();
        }

        public void HandleAlert()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            IAlert alert = WebDriver.SwitchTo().Alert();
            alert.Accept();
            js.ExecuteScript("arguments[0].click();", alert);
        }
    }
}
