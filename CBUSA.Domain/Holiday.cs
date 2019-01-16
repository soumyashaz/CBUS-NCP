using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Holiday
    {
        public Int64 HolidayID { get; set; }        
        public DateTime HolidayDate { get; set; }
        public Int64 Year { get; set; }
    }
}
