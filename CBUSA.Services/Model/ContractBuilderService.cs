using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
using System.Runtime.Remoting.Contexts;
namespace CBUSA.Services.Model
{
    public class ContractBuilderService : IContractBuilderService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ContractBuilderService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<ContractBuilder> GetBuilderofContract(Int64 ContractId)
        {
            return _ObjUnitWork.ContractBuilder.Search(x => x.ContractId == ContractId && x.RowStatusId == (Int32)RowActiveStatus.Active)
                                    .OrderBy(x => x.Builder.BuilderName);
        }



        public IEnumerable<ContractBuilder> GetBuilderContractInformation(long ContractId, long BuilderId)
        {
            return _ObjUnitWork.ContractBuilder.Search(x => (x.ContractId == ContractId && x.BuilderId == BuilderId && x.RowStatusId == (Int32)RowActiveStatus.Active));
        }

        #region prasenjit

        public IEnumerable<ContractBuilder> GetActiveContractsofBuilder(long BuilderId)
        {
            return _ObjUnitWork.ContractBuilder.Search(x => x.BuilderId == BuilderId && x.Contract.ContractStatusId == (int)ContractActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        public IEnumerable<ContractBuilder> GetActiveContractsofBuilder(long BuilderId,long QuaterId)
        {
            var Quarter=_ObjUnitWork.Quater.Find(f => f.QuaterId == QuaterId).FirstOrDefault();
            return _ObjUnitWork.ContractBuilder.Search(x => x.BuilderId == BuilderId && x.Contract.ContractStatusId == (int)ContractActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        public IEnumerable<ContractBuilder> GetArchiveContractsofBuilder(long BuilderId)
        {
            /* return _ObjUnitWork.ContractBuilder.Search(x => x.BuilderId == BuilderId && 
             x.Contract.ContractStatusId == (int)ContractActiveStatus.Active 
             && x.RowStatusId == (int)RowActiveStatus.Archived);*/// close by Rabi on 17 jan
            return _ObjUnitWork.ContractBuilder.GetBuilderArchiveContract(BuilderId);
        }

        public IEnumerable<Contract> GetDeclinedContractsofBuilder(long BuilderId, long ContractStatus)
        {
            /* return _ObjUnitWork.ContractBuilder.Search(x => x.BuilderId == BuilderId && 
             x.Contract.ContractStatusId == (int)ContractActiveStatus.Active 
             && x.RowStatusId == (int)RowActiveStatus.Archived);*/// close by Rabi on 17 jan
            return _ObjUnitWork.ContractBuilder.GetBuilderDeclinedContract(BuilderId, ContractStatus);
        }

        public IEnumerable<ContractBuilder> GetPendingContractsofBuilder(long BuilderId)
        {
            return _ObjUnitWork.ContractBuilder.Search(x => x.BuilderId == BuilderId && x.Contract.ContractStatusId != (int)ContractActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        
        public IEnumerable<ContractBuilder> GetAllContractofBuilder(Int64 BuilderId)
        {
            return _ObjUnitWork.ContractBuilder.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        public IEnumerable<Contract> GeBuildContract()
        {
            return _ObjUnitWork.ContractBuilder.GetUseContractList();
        }
        public IEnumerable<Contract> GetActiveContractsRegularReporting(long BuilderId)
        {
            return _ObjUnitWork.ContractBuilder.GetActiveContractsRegularReporting(BuilderId);
        }
        public IEnumerable<Contract> GetActiveOnlyContractsRegularReporting(Int64 BuilderId)
        {
            return _ObjUnitWork.ContractBuilder.GetActiveOnlyContractsRegularReporting(BuilderId);
        }
        #endregion

        public IEnumerable<Contract> GetActiveOnlyContractsRegularReportingBybuilderJoining(Int64 BuilderId, Int64 QuarterId)
        {
            var Quarter = _ObjUnitWork.Quater.Find(f => f.QuaterId == QuarterId).FirstOrDefault();
            var Data = _ObjUnitWork.ContractBuilder.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuarterId).Where(w => Quarter.StartDate <= Convert.ToDateTime(w.ContrctTo).AddDays(30));
            return Data;
        }
        
    }
}
