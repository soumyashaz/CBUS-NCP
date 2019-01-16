using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UI.Wrappers
{
    public static class Browser
    {
        public static void TryUntil(IWebDriver scope, Action action, Func<bool> until, TimeSpan waitBeforeRetry, TimeSpan timeoutToProcessing)
        {
            bool successfulMeasure = false;
            action.Invoke();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (watch.Elapsed < timeoutToProcessing)
            {
                if (!until.Invoke())
                {
                    Thread.Sleep(waitBeforeRetry);
                    action.Invoke();
                }
                else
                {
                    successfulMeasure = true;
                    break;
                }
            }

            if (!successfulMeasure)
            {
                throw new Exception(string.Format("{0} never reached required state.", action.Method.Name));
            }
        }

        public static IJavaScriptExecutor Scripts(this IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;

        }
    }
}
