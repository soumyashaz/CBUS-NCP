using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;


namespace CBUSA.Services.Interface
{
    public interface IConstructFormulaService
    {
        void SaveConstructFormula(ConstructFormula ObjConsFormula, Int64[] MarketId);
        IEnumerable<ConstructFormula> GetAllFormula();
        IEnumerable<ConstructFormula> GetFormulaByFilters(Int64? ContractId, string QuarterName, string Year, Int64? MarketId);
        ConstructFormula GetConstructFormulaById(Int64 ConstructFormulaId);
        IEnumerable<ConstructFormulaMarket> GetConstructFormulaMarketByFormulaId(Int64 ConstructFormulaId);
        List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater);
        List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater, Int64 ConstructFormulaId);
    }
}
