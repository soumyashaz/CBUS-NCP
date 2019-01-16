using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Model
{
    public class VendorMappingService : IVendorMappingService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public VendorMappingService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        IEnumerable<VendorMapping> GetAllMapping(Int64 BuilderId)
        {
            return _ObjUnitWork.VendorMapping.Search(a => a.BuilderId == BuilderId);
        }
        IEnumerable<BuilderVendorRemoved> GetBuilderVendorRemoved(Int64 BuilderId)
        {
            return _ObjUnitWork.BuilderVendorRemoved.Search(a => a.BuilderId == BuilderId);
        }
    }
}
