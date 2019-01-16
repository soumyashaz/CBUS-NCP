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
    public class ZoneService : IZoneService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ZoneService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }


        public IEnumerable<Zone> GetZoneAll()
        {
            return _ObjUnitWork.Zone.GetAll();
        }
    }
}
