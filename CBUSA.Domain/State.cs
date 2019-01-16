using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class State
    {
        public Int32 StateId { get; set; }
        public string StateName { get; set; }
        public Int32 IsActive { get; set; }
    }

    public class City
    {
        public Int32 CityId { get; set; }
        public string CityName { get; set; }
        public Int32 StateId { get; set; }
        public Int32 IsActive { get; set; }
    }
}
