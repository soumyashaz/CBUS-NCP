using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IHolidayService
    {
        IEnumerable<Holiday> GetEntireHolidayList();

        IEnumerable<Holiday> GetHolidayListForYear(Int64 Year);
    }
}
