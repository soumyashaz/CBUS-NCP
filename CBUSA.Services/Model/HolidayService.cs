using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
    public class HolidayService: IHolidayService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public HolidayService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Holiday> GetEntireHolidayList()
        {
            return _ObjUnitWork.Holiday.GetAll();
        }

        public IEnumerable<Holiday> GetHolidayListForYear(Int64 Year)
        {
            return _ObjUnitWork.Holiday.Search(x => x.Year == Year);
        }
    }
}
