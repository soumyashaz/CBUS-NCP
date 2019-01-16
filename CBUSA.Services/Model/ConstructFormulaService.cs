using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
using System.Data.Entity;

namespace CBUSA.Services.Model
{
    public class ConstructFormulaService : IConstructFormulaService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        private readonly CBUSADbContext dbContext = new CBUSADbContext();
        public ConstructFormulaService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public void SaveConstructFormula(ConstructFormula ObjConsFormula, Int64[] MarketId)
        {
            //ObjQuestion.SurveyOrder = _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
            //    RowActiveStatus.Active).Count() == 0 ? 1 : _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
            //    RowActiveStatus.Active).Max(x => x.SurveyOrder) + 1;
            //int DataExists = _ObjUnitWork.ConstructFormula.Find(a => a.ConstructFormulaId == ObjConsFormula.ConstructFormulaId).Count();
            //if (DataExists > 0)
            //{
            //    if (dbContext.DbConstructFormula.Any(e => e.ConstructFormulaId == ObjConsFormula.ConstructFormulaId))
            //    {
            //        dbContext.DbConstructFormula.Attach(ObjConsFormula);
            //        dbContext.Entry(ObjConsFormula).State = System.Data.Entity.EntityState.Modified;
            //        ObjConsFormula.ModifiedOn = System.DateTime.Now;
            //        ObjConsFormula.ModifiedBy = 1;
            //    }
            //    dbContext.SaveChanges();
            //}
            //else
            //{
            //    if (ObjConsFormula.ContractId > 0)
            //    {
            //        _ObjUnitWork.ConstructFormula.Add(ObjConsFormula);
            //    }
            //}
            //_ObjUnitWork.Complete();
            //if (CloseConnection)
            //{
            //    _ObjUnitWork.Dispose();
            //}

            _ObjUnitWork.ConstructFormula.SaveContractFormula(ObjConsFormula, MarketId);
        }
        public IEnumerable<ConstructFormula> GetAllFormula()
        {
            return _ObjUnitWork.ConstructFormula.GetAllFormula();
        }
        public IEnumerable<ConstructFormula> GetFormulaByFilters(Int64? ContractId, string QuarterName, string Year, Int64? MarketId)
        {
            //var ObjList = _ObjUnitWork.ConstructFormula.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId);            
            return _ObjUnitWork.ConstructFormula.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId);
        }
        public ConstructFormula GetConstructFormulaById(Int64 ConstructFormulaId)
        {
            return _ObjUnitWork.ConstructFormula.Get(ConstructFormulaId);
        }
        public IEnumerable<ConstructFormulaMarket> GetConstructFormulaMarketByFormulaId(Int64 ConstructFormulaId)
        {
            return _ObjUnitWork.ConstructFormulaMarket.Find(a => a.ConstructFormulaId == ConstructFormulaId);
        }

        public List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater)
        {
            return _ObjUnitWork.ConstructFormula.GetAllreadyBuildFormulaMarket(ContratctId, Year, Quater);
        }

        public List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater, Int64 ConstructFormulaId)
        {
            return _ObjUnitWork.ConstructFormula.GetAllreadyBuildFormulaMarket(ContratctId, Year, Quater, ConstructFormulaId);
        }
    }
}
