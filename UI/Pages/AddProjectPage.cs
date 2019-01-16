using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;
using System;
using OpenQA.Selenium.Interactions;

namespace UI.Pages
{
    public class AddProjectPage : BasePage
    {     
        public AddProjectPage(IWebDriver webDriver) : base(webDriver)
        {

        }
        public void ClickAddOrManageProjects()
        {
            //Logger.Log.Debug("Add or Manage Projects not found.");
            //var clickelement = WebDriver.FindElement(By.XPath("//a[@title = 'Manage Projects']"));
            //Actions ac = new Actions(WebDriver);
            //ac.Click(clickelement).Perform();
            WebDriver.FindElement(By.XPath("//a[@title = 'Manage Projects']/div[@class = 'pull-left marginTop2']")).Click();
        }
        public void ClickAddProject()
        {
            Logger.Log.Debug("Add Projects not found.");
            WebDriver.FindElement(By.XPath("//a[@title = 'Add Projects']")).Click();                             
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
        public int VerifyRows()
        {
            Logger.Log.Debug("Id found.");
            var row = WebDriver.FindElement(By.Id("project_table"));
            Logger.Log.Debug("Elements counted");
            IList<IWebElement> element = row.FindElements(By.XPath(".//tr[@data-counter]"));         
            var rowcount = element.Count();
            return rowcount;
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
            var row = WebDriver.FindElement(By.Id("listViewProject"));
            Logger.Log.Debug("Elements counted");
            var element = row.FindElements(By.XPath(".//tr[@class='tbody-row']")).Where(x => x.Text.Trim() != "");
            var rowcount = element.Count();
            return rowcount;
        }
        public string VerifyDataProjectRow(string columnHeading, int id)
        {
            Logger.Log.Debug("Id found.");
            var projecttDiv = WebDriver.FindElement(By.Id("project_table"));
            Logger.Log.Debug("Headers found.");
            var allColumns = projecttDiv.FindElements(By.XPath(".//th"));
            Logger.Log.Debug("Elements counted");
            var projectrows = projecttDiv.FindElements(By.XPath(".//tr[@class='tbody-row']")).Where(x => x.Text.Trim() != "");
            Logger.Log.Debug("Column Index counted");
            int reqColumnIndex = allColumns.IndexOf(allColumns.First(x => x.Text == columnHeading));
            Logger.Log.Debug("Required row counted");
            var reqrow = projectrows.First(x =>x.FindElement(By.XPath("./td[@class = 'text-center']")).Text.Contains(id.ToString()));
            Logger.Log.Debug("Desired Cell Found.");
            string desiredCell = reqrow.FindElements(By.XPath(".//td[@class = 'text-center']")).ElementAt(reqColumnIndex).FindElement(By.XPath("./input")).GetAttribute("value");
            return desiredCell;
        }
        public void VerifyValidators()
        {
            Logger.Log.Debug("Id found.");
            var projecttDiv = WebDriver.FindElement(By.Id("project_table"));
            Assert.AreEqual(true, projecttDiv.FindElement(By.XPath(".//span[@data-for = 'lotno_6']")).Displayed, "lotno. validation doesn't exist");
            Assert.AreEqual(true, projecttDiv.FindElement(By.XPath(".//span[@data-for = 'address_6']")).Displayed, "address validation doesn't exist");
            Assert.AreEqual(true, projecttDiv.FindElement(By.XPath(".//span[@data-for = 'Txt_City6']")).Displayed, "city validation doesn't exist");
            Assert.AreEqual(true, projecttDiv.FindElement(By.XPath(".//span[@data-for = 'Txt_Zip6']")).Displayed, "zip validation doesn't exist");
        }
        public bool NoProjectVerifyValidators(string lotno)
        {
            Logger.Log.Debug("Id found.");
            var projecttDiv = WebDriver.FindElement(By.Id("listViewProject"));
            Logger.Log.Debug("All rows found.");
            var allrows = projecttDiv.FindElements(By.XPath(".//tr[@class='tbody-row']")).Where(x => x.Text.Trim() != "");
            Logger.Log.Debug("Fetching the attribute value.");
            bool lotnoexists = allrows.Any(x => x.FindElement(By.XPath(".//input[contains(@id, 'lotno')]")).GetAttribute("value").Contains(lotno));
            return lotnoexists;
        }
        public void SaveButtonEnabled()
        {
            Assert.AreEqual(true, WebDriver.FindElement(By.Id("btnSave")).Enabled, "Save button is disabled");
        }
        public void SelectProject(string projectname)
        {
            Logger.Log.Debug("Id found.");
            var projecttDiv = WebDriver.FindElement(By.Id("listViewProject"));
            Logger.Log.Debug("All rows found.");
            // code for chrome browser
            var allrows = projecttDiv.FindElements(By.XPath(".//tr[@class='tbody-row']")).Where(x => x.Text != "  ");
            // code for edge browser
            //var allrows = projecttDiv.FindElements(By.XPath(".//tr[@class='tbody-row']")).Where(x=> x.Text.Trim()!= "");
            Logger.Log.Debug("Fetching the attribute value.");
            var reqrow = allrows.First(x => x.FindElement(By.XPath(".//input[contains(@id, 'projectname')]")).GetAttribute("value").Contains(projectname));
            Logger.Log.Debug("Clicked.");
            reqrow.FindElement(By.XPath(".//input[@name = 'RadProject']")).Click();          
        }
        public void ClickCopyProject()
        {
            Logger.Log.Debug("Clicked.");
            WebDriver.FindElement(By.Id("BtnCopy")).Click();              
        }
        public void CloseUpdateInfo()
        {
            Logger.Log.Debug("Clicked.");
            WebDriver.FindElement(By.XPath("//div[@class = 'k-window-actions']")).FindElement(By.XPath("//a[@class = 'k-window-action k-link']")).Click();                 
        }
        public void CloseProject()
        {        
            Logger.Log.Debug("Clicked.");
            WebDriver.FindElement(By.Id("BtnClose")).Click();
            Thread.Sleep(3000);       
        }
        public void ClickClosedProjectsTab()
        {
            Logger.Log.Debug("Clicked.");
            WebDriver.FindElement(By.Id("ulTask")).FindElement(By.XPath("//a[@title = 'Closed Projects']")).Click();          
        }
        public void scrolldown()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;         
            var req_saverebate = WebDriver.FindElement(By.Id("btnSave"));
            js.ExecuteScript("arguments[0].scrollIntoView();", req_saverebate);
        }
    }
}
