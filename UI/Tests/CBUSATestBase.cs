using NUnit.Framework;
using UI.Pages;

namespace UI.Tests
{
    public class CBUSATestBase : CBUSASqlActions
    {
        public HomePage homepage = null;
        public CBUSASqlActions cbsqlactions = null;

        [SetUp]
        public void OneTimeTestSetup()
        {
            cbsqlactions = new CBUSASqlActions();
            cbsqlactions.CreateBuilderData();
            homepage = CBUSAWebApp.Open();
        }

        [TearDown]
        public void OneTimeTestTearDown()
        {
            cbsqlactions.DeleteBuilderData();
            homepage.Quit();
        }
    }
}
