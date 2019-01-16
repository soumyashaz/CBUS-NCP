using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public class EulaServices : CBUSA.Services.IEulaServices
    {

        private readonly IUnitOfWork _ObjUnitWork;
        public EulaServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Eula> GetEula()
        {
            return _ObjUnitWork.Eulas.GetAll();
        }

        public Eula FindEula(Int64 Id)
        {
            return _ObjUnitWork.Eulas.Get(Id);
        }
        public void EditEula(Eula ObjEula)
        {
            _ObjUnitWork.Eulas.Update(ObjEula);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }
        public void SaveEula(Eula ObjEula)
        {
            _ObjUnitWork.Eulas.Add(ObjEula);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }
        public void DeleteEula(Eula ObjEula)
        {
            _ObjUnitWork.Eulas.Update(ObjEula);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public string GetRoleWiseEula(int RoleId)
        {
           return _ObjUnitWork.Eulas.GetRoleWiseEula(RoleId);
        }
    }
}
