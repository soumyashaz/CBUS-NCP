using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Services.Interface;
using EntityFramework.Batch;

namespace CBUSA.Services.Model
{
    public class BuilderQuarterContractStatusService:IBuilderQuarterContractStatusService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public BuilderQuarterContractStatusService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<BuilderQuarterContractStatus> CheckExistContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64? ContractId)
        {
            if(ContractId >0)
            {
                return _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active);
            }
            else
            {
                return _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active);
            }
        }
        public IEnumerable<BuilderQuarterContractStatus> CheckExistingBuilderQuater(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        public IEnumerable<BuilderQuarterContractStatus> CheckExistingBuilderRecord(Int64 BuilderId)
        {
            return _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.BuilderId == BuilderId  && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public void AddBuilderQuarterContractStatus(List<BuilderQuarterContractStatus> ObjBuilderQuarterContractStatus)
        {
            try
            {
                _ObjUnitWork.BuilderQuarterContractStatus.AddRange(ObjBuilderQuarterContractStatus);
                _ObjUnitWork.Complete();
            }
            catch (Exception Ex)
            {
                //throw Ex;
            }
            //throw new NotImplementedException();
        }

        public void AddBuilderQuarterContractStatus(BuilderQuarterContractStatus ObjBuilderQuarterContractStatus)
        {
            try
            {
                _ObjUnitWork.BuilderQuarterContractStatus.Add(ObjBuilderQuarterContractStatus);
                _ObjUnitWork.Complete();
                //_ObjUnitWork.Dispose();
            }
            catch (Exception Ex)
            {
                //throw Ex;
            }
        }

        public void UpdateBuilderQuarterContractStatus(BuilderQuarterContractStatus ObjBuilderQuarterContractStatus)
        {
            try
            {
                _ObjUnitWork.BuilderQuarterContractStatus.UpdateAsync(ObjBuilderQuarterContractStatus);
                _ObjUnitWork.Complete();
               // _ObjUnitWork.Dispose();
            }
            catch (Exception Ex)
            {
                //throw Ex;
            }
        }
    }
}
