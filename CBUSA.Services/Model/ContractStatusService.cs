using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;
namespace CBUSA.Services.Model
{
    public class ContractStatusService : IContractStatusService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public ContractStatusService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<ContractStatus> GetContractStatusAll()
        {
            return _ObjUnitWork.ContractStatus.GetAll();
            // return list;
        }

        public ContractStatus GetContractStatus(Int64 ContractStatusId)
        {
            return _ObjUnitWork.ContractStatus.Get(ContractStatusId);
            // // return list;
            //  return _ObjUnitWork.ContractStatus.

        }

        public void SaveContractStatus(ContractStatus ObjContractStatus)
        {
            _ObjUnitWork.ContractStatus.Add(ObjContractStatus);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void EditContractStatus(ContractStatus ObjContractStatus)
        {
            _ObjUnitWork.ContractStatus.Update(ObjContractStatus);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteContractStatus(ContractStatus ObjContractStatus)
        {
            _ObjUnitWork.ContractStatus.Update(ObjContractStatus);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public IEnumerable<ContractStatus> GetUseContractStatus()
        {
            return _ObjUnitWork.ContractStatus.GetUseContractStatus();
            // return list;
        }


    }
}
