using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace UI.Pages
{
    public static class PageFactory
    {
        public static T Create<T>(IWebDriver webDriver) where T : IPage
        {
            return (T)Activator.CreateInstance(typeof(T), webDriver);
        }
    }
}
