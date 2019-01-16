using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System.Data.SqlClient;
namespace CBUSA.Repository.Model
{
    public class ConstructFormulaRepository : Repository<ConstructFormula>, IConstructFormulaRepository
    {
        public ConstructFormulaRepository(CBUSADbContext Context)
            : base(Context)
        {
        }
        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }
        public IEnumerable<ConstructFormula> GetAllFormula()
        {
            return Context.DbConstructFormula.Include("ConstructFormulaMarket").Include("ConstructFormulaMarket.Market").Where(n => n.RowStatusId == (int)RowActiveStatus.Active);
        }


        public IEnumerable<ConstructFormula> GetFormulaByFilters(Int64? ContractId, string QuarterName, string Year, Int64? MarketId)
        {
            // var FormulaData = Context.DbConstructFormula.Where(n => n.RowStatusId == (int)RowActiveStatus.Active);

            // var FormulaData = Context.DbConstructFormula.Join(Context.DbConstructFormulaMarket, x => x.ConstructFormulaId, y => y.ConstructFormulaId, (x, y) => new { x, y });
            //Where(n => n.RowStatusId == (int)RowActiveStatus.Active);
            var FormulaData = Context.DbConstructFormula.Include("ConstructFormulaMarket").Include("ConstructFormulaMarket.Market").Select(x => x);
            if (ContractId.HasValue)
            {
                if (ContractId.Value > 0)
                    FormulaData = FormulaData.Where(z => z.ContractId == ContractId);
            }
            if (!string.IsNullOrEmpty(QuarterName))
                FormulaData = FormulaData.Where(z => z.Quarter == QuarterName);
            if (!string.IsNullOrEmpty(Year))
                FormulaData = FormulaData.Where(z => z.Year == Year);
            /* Rabi*/
            if (MarketId.HasValue)
                FormulaData = FormulaData.Where(z => z.ConstructFormulaMarket.Select(x => x.MarketId).Contains(MarketId.Value));
            return FormulaData.Select(z => z);
        }
        public void SaveContractFormula(ConstructFormula ObjConsFormula, Int64[] MarketList)
        {
            if (ObjConsFormula.ConstructFormulaId > 0)
            {

                Context.DbConstructFormula.Attach(ObjConsFormula);
                Context.Entry(ObjConsFormula).State = System.Data.Entity.EntityState.Modified;
                ObjConsFormula.ModifiedOn = System.DateTime.Now;
                ObjConsFormula.ModifiedBy = 1;

                var HistoryMarketList = Context.DbConstructFormulaMarket.Where(x => x.ConstructFormulaId == ObjConsFormula.ConstructFormulaId).ToList();
                Context.DbConstructFormulaMarket.RemoveRange(HistoryMarketList);
                //foreach(var Item in HistoryMarketList)
                //{
                //    Context.DbConstructFormulaMarket.re(HistoryMarketList);
                //    Context.Entry(HistoryMarketList).State = System.Data.Entity.EntityState.Deleted;
                //}


                if (MarketList.Length > 0)
                {
                    List<ConstructFormulaMarket> List = new List<ConstructFormulaMarket>();
                    foreach (Int64 Obj in MarketList)
                    {
                        List.Add(new ConstructFormulaMarket { ConstructFormulaId = ObjConsFormula.ConstructFormulaId, MarketId = Obj });
                    }
                    Context.DbConstructFormulaMarket.AddRange(List);

                }
                Context.SaveChanges();
                Context.Dispose();
            }
            else
            {
                Context.DbConstructFormula.Add(ObjConsFormula);
                ObjConsFormula.ModifiedOn = System.DateTime.Now;
                ObjConsFormula.ModifiedBy = 1;
                if (MarketList.Length > 0)
                {
                    List<ConstructFormulaMarket> List = new List<ConstructFormulaMarket>();
                    foreach (Int64 Obj in MarketList)
                    {
                        List.Add(new ConstructFormulaMarket { ConstructFormulaId = ObjConsFormula.ConstructFormulaId, MarketId = Obj });
                    }
                    Context.DbConstructFormulaMarket.AddRange(List);

                }
                Context.SaveChanges();
                Context.Dispose();

            }


        }

        public List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater)
        {
            var MarketList = Context.DbConstructFormula.Where(x => x.ContractId == ContratctId && x.Quarter == Quater && x.Year == Year)
                .Join(Context.DbConstructFormulaMarket, x => x.ConstructFormulaId, y => y.ConstructFormulaId, (x, y) => y).Select(x => x.MarketId).ToList();

            return Context.DbMarket.Where(x => MarketList.Contains(x.MarketId)).ToList();

        }
        public List<Market> GetAllreadyBuildFormulaMarket(Int64 ContratctId, string Year, string Quater, Int64 ConstructFormulaId)
        {

            var MarketList = Context.DbConstructFormula.Where(x => x.ContractId == ContratctId && x.Quarter == Quater && x.Year == Year && x.ConstructFormulaId == ConstructFormulaId)
                .Join(Context.DbConstructFormulaMarket, x => x.ConstructFormulaId, y => y.ConstructFormulaId, (x, y) => y).Select(x => x.MarketId).ToList();

            return Context.DbMarket.Where(x => MarketList.Contains(x.MarketId)).ToList();

        }
    }
}
