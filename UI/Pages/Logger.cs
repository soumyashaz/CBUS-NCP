using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages
{
    public static class Logger
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Environment.MachineName);
    }
}
