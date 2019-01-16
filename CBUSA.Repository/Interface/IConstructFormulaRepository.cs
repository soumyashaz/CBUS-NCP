using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Interface
{
    public interface IConstructFormulaRepository : IRepository<ConstructFormula>
    {
        IEnumerable<ConstructFormula> GetAllFormula();
        IEnumerable<ConstructFormula> GetFormulaByFilters(Int64? ContractId, string QuarterName, string Year, Int64? MarketId);
        void SaveContractFormula(ConstructFormula ObjConsFormula, Int64[] MarketList);
        List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater);
        List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater, Int64 ConstructFormulaId);
    }
}
