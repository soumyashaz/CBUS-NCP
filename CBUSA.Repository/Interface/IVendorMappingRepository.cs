using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface IVendorMappingRepository : IRepository<VendorMapping>
    {
        IEnumerable<dynamic> GetAllVendorMappingList(Int64 BuilderId);
        IEnumerable<dynamic> GetCBUSAVendorListUnMapped(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderVendorListUnMapped(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderVendorRemovedList(Int64 BuilderId);
    }
}
