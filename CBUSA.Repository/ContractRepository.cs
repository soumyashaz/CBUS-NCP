using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        public ContractRepository(CBUSADbContext Context)
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



        public bool IsContractNameAvailable(string ContractName)
        {
            return !Context.DbContract.Where(x => x.ContractName == ContractName).Any();
        }

        public bool IsContractLabelAvailable(string ContractLabelName)
        {
            return !Context.DbContract.Where(x => x.Label == ContractLabelName).Any();
        }


        public IEnumerable<Contract> GetActiveContract()
        {
            return Context.DbContract.Where(x => (x.ContractStatusId == (int)ContractActiveStatus.Active
                && x.RowStatusId == (int)RowActiveStatus.Active));
        }
        public IEnumerable<Contract> GetActiveContractDescending()
        {
            return Context.DbContract.Where(x => (x.ContractStatusId == (int)ContractActiveStatus.Active
                && x.RowStatusId == (int)RowActiveStatus.Active)).OrderBy(x => x.ContractName);
        }

        public IEnumerable<Contract> GetArchivedontractDescending()
        {
            return Context.DbContract.Where(x => (x.ContractStatusId == (int)ContractActiveStatus.Active
                && x.RowStatusId == (int)RowActiveStatus.Archived)).OrderBy(x => x.ContractName);
        }

        public IEnumerable<Contract> GetPendingContract()
        {
            return Context.DbContract.Where(x => (x.ContractStatusId != (int)ContractActiveStatus.Active
                && x.RowStatusId == (int)RowActiveStatus.Active));
        }

        public IEnumerable<Builder> GetAssocaitedBuilderWithContract(Int64 ContractId)
        {
            return Context.DbContractBuilder.Where(z => z.ContractId == ContractId).Join(Context.DbBuilder, x => x.BuilderId, y => y.BuilderId,
                  (x, y) => y
                );
        }
        public IEnumerable<Contract> GetNonAssociatedContract(Int64 BuilderId, string Flag)
        {
            var Builder = Context.DbBuilder.Where(x => x.BuilderId == BuilderId).FirstOrDefault();
            var AssociatedContrcat = Context.DbContractBuilder.Where(x => x.BuilderId == BuilderId).Select(x => x.ContractId).ToList();
            //  var data;
            if (Flag == "act")
            {
                var data = Context.DbContract.Join(Context.DbSurvey, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
                     .Join(Context.DbSurveyMarket, s => s.y.SurveyId, p => p.SurveyId, (s, p) => new { s, p })
                     .Where(n => n.s.y.IsEnrolment == true && n.p.MarketId == Builder.MarketId && n.s.x.RowStatusId ==
                         (int)RowActiveStatus.Active && n.s.x.ContractStatusId == (int)ContractActiveStatus.Active
                         && !AssociatedContrcat.Contains(n.s.x.ContractId)
                         && n.s.y.IsPublished == true
                         && n.s.y.EndDate > DateTime.Now
                     )
                     .GroupBy(z => z.s.x.ContractId).
                     SelectMany(m => m.Select(x => x.s.x));

                return data;
            }
            else if (Flag == "pen")
            {
                var data = Context.DbContract.Join(Context.DbSurvey, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
                     .Join(Context.DbSurveyMarket, s => s.y.SurveyId, p => p.SurveyId, (s, p) => new { s, p })
                     .Where(n => n.s.y.IsEnrolment == true 
                            && n.s.y.EndDate > DateTime.Now                     //Modified by Apala for VSTS#14321
                            && n.p.MarketId == Builder.MarketId 
                            && n.s.x.RowStatusId == (int)RowActiveStatus.Active 
                            && n.s.x.ContractStatusId != (int)ContractActiveStatus.Active
                            && !AssociatedContrcat.Contains(n.s.x.ContractId)
                            && n.s.y.IsPublished == true
                           )
                     .GroupBy(z => z.s.x.ContractId).
                     SelectMany(m => m.Select(x => x.s.x));

                return data;
            }
            return null;

        }

    }
}
