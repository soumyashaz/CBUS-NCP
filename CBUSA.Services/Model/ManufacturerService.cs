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
    public class ManufacturerService : IManufacturerService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ManufacturerService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }


        public IEnumerable<Manufacturer> GetManufacturer()
        {
            return _ObjUnitWork.Manufacturer.GetAll();
        }
    }
}
