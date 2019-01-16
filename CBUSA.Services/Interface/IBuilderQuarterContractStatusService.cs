using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IBuilderQuarterContractStatusService
    {
        IEnumerable<BuilderQuarterContractStatus> CheckExistContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64? ContractId);
        IEnumerable<BuilderQuarterContractStatus> CheckExistingBuilderQuater(Int64 BuilderId, Int64 QuaterId);
        IEnumerable<BuilderQuarterContractStatus> CheckExistingBuilderRecord(Int64 BuilderId);
        void AddBuilderQuarterContractStatus(List<BuilderQuarterContractStatus> ObjBuilderQuarterContractStatus);
        void AddBuilderQuarterContractStatus(BuilderQuarterContractStatus ObjBuilderQuarterContractStatus);
         void UpdateBuilderQuarterContractStatus(BuilderQuarterContractStatus ObjBuilderQuarterContractStatus);
    }
}
