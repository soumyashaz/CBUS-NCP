using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
using System.Collections.Generic;

namespace CBUSA.Services.Model
{
    public class StateService : IStateService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public StateService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<State> GetState()
        {
            return _ObjUnitWork.State.GetAll();
        }
        public IEnumerable<State> GetStateByName(string State)
        {
            return _ObjUnitWork.State.Search(x => x.StateName == State&&x.IsActive==(int)RowActiveStatus.Active);
        }
    }
}
