using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface ICategoryMappingRepository : IRepository<CategoryMapping>
    {
        IEnumerable<dynamic> GetAllCategoryMappingList(Int64 BuilderId);
        IEnumerable<dynamic> GetCBUSACategoryListUnMapped(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderCategoryListUnMapped(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderCategoryRemovedList(Int64 BuilderId);
    }
}
