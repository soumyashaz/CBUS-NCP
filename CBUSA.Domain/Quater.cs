using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Quater
    {
        public Int64 QuaterId { get; set; }
        public string QuaterName { get; set; }
        public Int64 Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReportingStartDate { get; set; }
        public DateTime ReportingEndDate { get; set; }
        public DateTime BuilderReportingEndDate { get; set; }
    }
}
