using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Repository.Interface;
using CBUSA.Domain;

namespace CBUSA.Repository.Model
{
    class NonResponderReportRepository : Repository<Builder>, INonResponderReportRepository
    {
        public NonResponderReportRepository(CBUSADbContext Context) : base(Context)
        {
        }

        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }

        public IEnumerable<dynamic> GetNonResponderBuilderList(Int64 QuarterId)
        {
            //Get all builders who responded
            var ResponderBuilders = (from baqr in Context.DbBuilderQuaterAdminReport
                                     where baqr.QuaterId == QuarterId
                                     select baqr.BuilderId).Distinct();
                        
            //Get all non-responders along with Market and Contract Names
            var NonResponderBuilders = (from b in Context.DbBuilder
                                        where (!ResponderBuilders.Contains(b.BuilderId))
                                        join m in Context.DbMarket on b.MarketId equals m.MarketId into grp_market
                                        from grm in grp_market
                                        join baqr in Context.DbBuilderQuaterAdminReport on b.BuilderId equals baqr.BuilderId into grp_baqr
                                        select new
                                        {
                                            BuilderId = b.BuilderId,
                                            BuilderName = b.BuilderName,
                                            MarketId = grm.MarketId,
                                            MarketName = grm.MarketName,
                                            ContractList = (from cb in Context.DbContractBuilder
                                                            join c in Context.DbContract on cb.ContractId equals c.ContractId
                                                            where cb.BuilderId == b.BuilderId
                                                            select c.ContractName).ToList(),
                                            NumberOfReportingQuarters = grp_baqr.Count()
                                        });

            //Group NonResponderBuilders based on BuilderId to get Count of Participating Contracts
            var NRBuilderContracts = (from nr in NonResponderBuilders
                                      join cb in Context.DbContractBuilder on nr.BuilderId equals cb.BuilderId     
                                      where cb.RowStatusId == (int) RowActiveStatus.Active
                                      group 1 by cb.BuilderId into grouped
                                      select new
                                      {
                                          BuilderId = grouped.Key,
                                          CountOfParticipatingContracts = grouped.Count()
                                      });

            //Combine NonResponderBuilders with NRBuilderContracts to get entire data-set
            var FinalResultSet = (from r1 in NonResponderBuilders
                                 join r2 in NRBuilderContracts on r1.BuilderId equals r2.BuilderId
                                 select new
                                 {
                                     BuilderId = r1.BuilderId,
                                     BuilderName = r1.BuilderName,
                                     MarketId = r1.MarketId,
                                     MarketName = r1.MarketName,
                                     ContractList = r1.ContractList,
                                     NumberOfReportingQuarters = r1.NumberOfReportingQuarters,
                                     CountOfParticipatingContracts = r2.CountOfParticipatingContracts
                                 }).OrderBy(item => item.MarketName).ThenBy(item => item.BuilderName);
            
            List < dynamic > ObjResult = new List<dynamic>();
            foreach (var Item in FinalResultSet)
            {
                ObjResult.Add(new
                {
                    BuilderId = Item.BuilderId,
                    BuilderName = Item.BuilderName,
                    MarketId = Item.MarketId,
                    MarketName = Item.MarketName,
                    ContractList = String.Join(",", Item.ContractList).Replace(" ,", ", "),
                    CountOfParticipatingContracts = Item.CountOfParticipatingContracts,
                    NumberOfReportingQuarters = Item.NumberOfReportingQuarters
                });
            }

            return ObjResult;
        }
    }
}
