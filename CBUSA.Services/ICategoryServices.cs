using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public interface ICategoryServices
    {
        IEnumerable<CBUSA.Domain.Category> GetCategory();
        void SaveCategory(CBUSA.Domain.Category ObjStatus);
        void EditCategory(CBUSA.Domain.Category ObjStatus);
        void DeleteCategory(CBUSA.Domain.Category ObjStatus);
    }
}
