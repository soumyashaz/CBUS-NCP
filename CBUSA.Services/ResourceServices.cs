using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public class ResourceServices : CBUSA.Services.IResourceServices
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public ResourceServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Resource> GetResource()
        {
            //var list = _ObjUnitWork.Resources.GetAll();
            //return list;
            return null;
        }

        public void SaveResource(Resource ObjResource)
        {
            //_ObjUnitWork.Resources.Add(ObjResource);
            //_ObjUnitWork.Complete();
            //_ObjUnitWork.Dispose();
        }

        public void EditResource(Resource ObjResource)
        {
        //    _ObjUnitWork.Resources.Update(ObjResource);
        //    _ObjUnitWork.Complete();
        //    _ObjUnitWork.Dispose();
        }

        public void DeleteResource(Resource ObjResource)
        {
            //_ObjUnitWork.Resources.Update(ObjResource);
            //_ObjUnitWork.Complete();
            //_ObjUnitWork.Dispose();
        }

       
       
    }
}
