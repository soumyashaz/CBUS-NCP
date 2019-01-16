using NUnit.Framework;
using System.Threading;
using UI.Pages;

namespace UI.Tests
{
    public class AddProjectTests : CBUSATestBase
    {
        [TestCase, Order(1)]
        public void AddProject()
        {
            Logger.Log.Info("NCP Rebate Quarter Rebate Report clicked.");
            var addproject = homepage.ClickNcpQuaterlyRebate("NCP Quarterly Rebate Report");           
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddProject();
            Thread.Sleep(3000);
            Logger.Log.Info("5 Projects inserted.");
            addproject.InsertNumberOfProjects("5");
            Thread.Sleep(3000);
            Logger.Log.Info("Add Project clicked.");
            addproject.ClickAdd();
            Thread.Sleep(3000);
            Assert.AreEqual(5, addproject.VerifyRows());
        }

        [TestCase, Order(2)]
        public void AddTwoProject()
        {
            Logger.Log.Info("NCP Rebate Quarter Rebate Report clicked.");
            var addproject = homepage.ClickNcpQuaterlyRebate("NCP Quarterly Rebate Report");
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            //Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(3000);
            //Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(3000);
            addproject.ClickAddProject();
            Thread.Sleep(3000);
            Logger.Log.Info("2 Projects inserted.");
            addproject.InsertNumberOfProjects("2");
            Thread.Sleep(3000);
            Logger.Log.Info("Add Project clicked.");
            addproject.ClickAdd();
            Thread.Sleep(3000);
            Logger.Log.Info("Test Project1 inserted.");
            addproject.InsertProjectDetails(6, "Test Project 1", "Test Lot1", "Test Address1", "Kolkata", "kol123");
            Logger.Log.Info("Test Project2 inserted.");
            addproject.InsertProjectDetails(7, "Test Project 2", "Test Lot2", "Test Address2", "Chicago", "kol12356");
            Thread.Sleep(3000);
            Logger.Log.Info("Projects saved.");
            addproject.SaveProject();
            Thread.Sleep(3000);
            Logger.Log.Info("Spinner disappeared.");
            addproject.WaitForSpinnerDisappear();
            Thread.Sleep(3000);
            Logger.Log.Info("Page Loaded.");
            addproject.WaitLoading();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            addproject.WaitLoading();
            addproject.WaitForSpinnerDisappear();
            Thread.Sleep(3000);
            Assert.AreEqual("Test Project 1", addproject.VerifyDataProjectRow("Project Name", 1));
            Assert.AreEqual("Test Lot1", addproject.VerifyDataProjectRow("Lot No.", 1));
            Assert.AreEqual("Test Address1", addproject.VerifyDataProjectRow("Address", 1));
            Assert.AreEqual("Kolkata", addproject.VerifyDataProjectRow("City", 1));
            Assert.AreEqual("kol123", addproject.VerifyDataProjectRow("Zip", 1)); 
            Assert.AreEqual("Test Project 2", addproject.VerifyDataProjectRow("Project Name", 2));
            Assert.AreEqual("Test Lot2", addproject.VerifyDataProjectRow("Lot No.", 2));
            Assert.AreEqual("Test Address2", addproject.VerifyDataProjectRow("Address", 2));
            Assert.AreEqual("Chicago", addproject.VerifyDataProjectRow("City", 2));
            Assert.AreEqual("kol12356", addproject.VerifyDataProjectRow("Zip", 2));          
        }

        [TestCase, Order(3)]
        public void AddOneProject()
        {
            Logger.Log.Info("NCP Rebate Quarter Rebate Report clicked.");
            var addproject = homepage.ClickNcpQuaterlyRebate("NCP Quarterly Rebate Report");
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(8000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddProject();
            Thread.Sleep(3000);
            Logger.Log.Info("1 Project inserted.");
            addproject.InsertNumberOfProjects("1");
            Thread.Sleep(3000);
            Logger.Log.Info("Add Project clicked.");
            addproject.ClickAdd();
            Thread.Sleep(3000);
            Logger.Log.Info("Test Project1 inserted.");
            addproject.InsertProjectDetails(6, "Test Project 1", string.Empty, string.Empty, string.Empty, string.Empty);
            Thread.Sleep(3000);
            Logger.Log.Info("Projects saved.");
            addproject.SaveProject();
            addproject.WaitLoading();
            Logger.Log.Info("Validators have been verified.");
            addproject.VerifyValidators();
        }

        [TestCase, Order(4)]
        public void NoProjectAdded()
        {
            Logger.Log.Info("NCP Rebate Quarter Rebate Report clicked.");
            var addproject = homepage.ClickNcpQuaterlyRebate("NCP Quarterly Rebate Report");
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(5000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddProject();
            Thread.Sleep(3000);
            Logger.Log.Info("1 Project inserted.");
            addproject.InsertNumberOfProjects("1");
            Thread.Sleep(3000);
            Logger.Log.Info("Add Project clicked.");
            addproject.ClickAdd();
            Thread.Sleep(3000);
            Logger.Log.Info("Empty Project Details Inserted.");
            addproject.InsertProjectDetails(6, string.Empty, "Test Lot1", "Test Address1", "Kolkata", "kol123");
            Thread.Sleep(3000);
            Logger.Log.Info("Projects saved.");
            addproject.SaveProject();
            Logger.Log.Info("Page Loaded.");
            addproject.WaitLoading();
            Logger.Log.Info("Spinner disappeared.");
            addproject.WaitForSpinnerDisappear();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(5000);
            Assert.AreEqual(false, addproject.NoProjectVerifyValidators("Test Lot1"), "Test Lot1 should not exist, but is being displayed.");
        }

        [TestCase, Order(5)]
        public void SaveButtonEnabled()
        {
            Logger.Log.Info("NCP Rebate Quarter Rebate Report clicked.");
            var addproject = homepage.ClickNcpQuaterlyRebate("NCP Quarterly Rebate Report");
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(5000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddProject();
            Thread.Sleep(3000);
            Logger.Log.Info("1 Project inserted.");
            addproject.InsertNumberOfProjects("1");
            Thread.Sleep(3000);
            Logger.Log.Info("Add Project clicked.");
            addproject.ClickAdd();
            Thread.Sleep(3000);
            Logger.Log.Info("Test Project1 inserted.");
            addproject.InsertProjectDetails(6, "Test Project 1", string.Empty, string.Empty, string.Empty, string.Empty);
            Thread.Sleep(3000);
            Logger.Log.Info("Verifying Save button enabled or not.");
            addproject.SaveButtonEnabled();
            Logger.Log.Info("Projects saved.");
            addproject.SaveProject();
            Logger.Log.Info("Validators have been verified.");
            addproject.VerifyValidators();
        }

        [TestCase, Order(6)]
        public void BlankRowEliminate()
        {
            Logger.Log.Info("NCP Rebate Quarter Rebate Report clicked.");
            var addproject = homepage.ClickNcpQuaterlyRebate("NCP Quarterly Rebate Report");
            Thread.Sleep(5000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(5000);
            //Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(5000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddProject();
            Thread.Sleep(3000);
            Logger.Log.Info("2 Projects inserted.");
            addproject.InsertNumberOfProjects("2");
            Thread.Sleep(3000);
            Logger.Log.Info("Add Project clicked.");
            addproject.ClickAdd();
            Thread.Sleep(3000);
            Logger.Log.Info("Test Project1 inserted.");
            addproject.InsertProjectDetails(6, "Test Project 1", "Test Lot1", "Test Address1", "Kolkata", "kol123");
            Thread.Sleep(3000);
            Logger.Log.Info("Empty Project detials inserted.");
            addproject.InsertProjectDetails(7, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            Thread.Sleep(3000);
            Logger.Log.Info("Projects saved.");
            addproject.SaveProject();
            Logger.Log.Info("Spinner disappeared.");
            addproject.WaitForSpinnerDisappear();
            Logger.Log.Info("Page Loaded.");
            addproject.WaitLoading();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            //Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(5000);
            Assert.AreEqual(6, addproject.VerifyProjectRows());
            Assert.AreEqual("Test Project 1", addproject.VerifyDataProjectRow("Project Name", 1));
        }

        [TestCase, Order(7)]
        public void CopyProject()
        {
            Logger.Log.Info("NCP Rebate Quarter Rebate Report clicked.");
            var addproject = homepage.ClickNcpQuaterlyRebate("NCP Quarterly Rebate Report");
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            //Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(5000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddProject();
            Thread.Sleep(3000);
            Logger.Log.Info("1 Project inserted.");
            addproject.InsertNumberOfProjects("1");
            Thread.Sleep(3000);
            Logger.Log.Info("Add Project clicked.");
            addproject.ClickAdd();
            Thread.Sleep(3000);
            Logger.Log.Info("Test Project inserted.");
            addproject.InsertProjectDetails(6, "Test Project", "Test Lot1", "Test Address1", "Kolkata", "kol123");
            Thread.Sleep(3000);
            Logger.Log.Info("Projects saved.");
            addproject.SaveProject();
            Logger.Log.Info("Spinner disappeared.");
            addproject.WaitForSpinnerDisappear();
            Logger.Log.Info("Page Loaded.");
            addproject.WaitLoading();
            Thread.Sleep(3000);
            Logger.Log.Info("Add or Manage Projects clicked.");
            addproject.ClickAddOrManageProjects();
            Thread.Sleep(3000);
            //Logger.Log.Info("Add or Manage Projects clicked.");
            //addproject.ClickAddOrManageProjects();
            //Thread.Sleep(5000);
            Logger.Log.Info("Scrolling down the page.");
            addproject.scrolldown();
            Logger.Log.Info("Test Project selected.");
            addproject.SelectProject("Test Project");
            Thread.Sleep(3000);
            Logger.Log.Info("Copy Projects clicked.");
            addproject.ClickCopyProject();
            Thread.Sleep(3000);
            Logger.Log.Info("Accept Dialog clicked.");
            addproject.AcceptDialog();
            Thread.Sleep(3000);
            Logger.Log.Info("Spinner disappeared.");
            addproject.WaitForSpinnerDisappear();
            Thread.Sleep(3000);
            Logger.Log.Info("Close Update Info clicked.");
            addproject.CloseUpdateInfo();
            Thread.Sleep(3000);
            Logger.Log.Info("Scrolling down the page.");
            addproject.scrolldown();
            Assert.AreEqual("Test Project_1", addproject.VerifyDataProjectRow("Project Name", 1));
            Thread.Sleep(3000);
            Logger.Log.Info("Test Project_1 selected.");
            addproject.SelectProject("Test Project_1");
            Thread.Sleep(3000);
            Logger.Log.Info("Copy Projects clicked.");
            addproject.ClickCopyProject();
            Thread.Sleep(3000);
            Logger.Log.Info("Accept Dialog clicked.");
            addproject.AcceptDialog();
            Thread.Sleep(3000);
            Logger.Log.Info("Spinner disappeared.");
            addproject.WaitForSpinnerDisappear();
            Logger.Log.Info("Close Update Info clicked.");
            addproject.CloseUpdateInfo();
            Thread.Sleep(3000);
            Logger.Log.Info("Scrolling down the page.");
            addproject.scrolldown();
            Assert.AreEqual("Test Project_1_1", addproject.VerifyDataProjectRow("Project Name", 1));
            Thread.Sleep(3000);
            Logger.Log.Info("Test Project_1 selected.");
            addproject.SelectProject("Test Project_1");
            Thread.Sleep(3000);
            Logger.Log.Info("Close project button clicked.");
            addproject.CloseProject();
            Thread.Sleep(3000);
            Logger.Log.Info("Accept Dialog clicked.");
            addproject.AcceptDialog();
            Thread.Sleep(3000);
            Logger.Log.Info("Close Update Info clicked.");
            addproject.CloseUpdateInfo();
            Thread.Sleep(3000);
            Logger.Log.Info("Scrolling down the page.");
            addproject.scrolldown();
            Assert.AreEqual(7, addproject.VerifyProjectRows());
            Thread.Sleep(3000);
            Logger.Log.Info("Closed Projects tab.");
            addproject.ClickClosedProjectsTab();
            addproject.WaitLoading();
            Thread.Sleep(3000);
            Assert.AreEqual("Test Project_1_1", addproject.VerifyDataProjectRow("Project Name", 1));
        }
    }
}