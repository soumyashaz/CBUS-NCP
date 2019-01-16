using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public interface IResourceServices
    {
        IEnumerable<CBUSA.Domain.Resource> GetResource();
        void SaveResource(CBUSA.Domain.Resource ObjResource);
        void EditResource(CBUSA.Domain.Resource ObjResource);
        void DeleteResource(CBUSA.Domain.Resource ObjResource);
     
    }
}
