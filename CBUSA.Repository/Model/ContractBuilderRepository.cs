using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System.Data.Entity;

namespace CBUSA.Repository.Model
{
  public  class ContractBuilderRepository: Repository<ContractBuilder>, IContractBuilderRepository
    {

      public ContractBuilderRepository(CBUSADbContext Context)
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
        public IEnumerable<Contract> GetUseContractList()
        {
            return Context.DbContract.Join(Context.DbContractBuilder, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }
               ).Where(m => m.x.RowStatusId == (int)RowActiveStatus.Active && m.y.RowStatusId == (int)RowActiveStatus.Active)
               .Select(m => m.x);
        }
        public IEnumerable<Contract> GetActiveContractsRegularReporting(Int64 BuilderId)
        {
            return Context.DbContract.Join(Context.DbContractBuilder, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }
               ).Where(m => m.x.RowStatusId == (int)RowActiveStatus.Active && m.y.RowStatusId == (int)RowActiveStatus.Active && m.y.BuilderId==BuilderId && m.x.IsReportable == true)
               .Select(m => m.x);
        }
        public IEnumerable<Contract> GetActiveOnlyContractsRegularReporting(Int64 BuilderId)
        {
            return Context.DbContract.Join(Context.DbContractBuilder, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }
               ).Where(m => m.x.RowStatusId == (int) RowActiveStatus.Active && m.y.RowStatusId == (int) RowActiveStatus.Active && m.y.BuilderId == BuilderId && m.x.ContractStatusId == (int)ContractActiveStatus.Active && m.x.IsReportable == true)
               .Select(m => m.x);
        }

        // Created by Neyaz on 22-Nov-2017 ==== VSO#7109
        public IEnumerable<Contract> GetActiveOnlyContractsRegularReportingBybuilderJoining(Int64 BuilderId, Int64 QuarterId)
        {
            var Quarter = Context.DbQuater.Where(x => x
              .QuaterId == QuarterId).FirstOrDefault();

            var data = Context.DbContract.Join(Context.DbContractBuilder.Where( p=> DbFunctions.TruncateTime(p.JoiningDate) <= Quarter.ReportingStartDate), x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }
              ).Where(m => m.x.RowStatusId == (int)RowActiveStatus.Active && m.y.RowStatusId == (int)RowActiveStatus.Active && m.y.BuilderId == BuilderId && m.x.ContractStatusId == (int)ContractActiveStatus.Active && m.x.IsReportable == true)
              .Select(m => m.x);

            return data;
        }
        public IEnumerable<ContractBuilder> GetBuilderArchiveContract(Int64 BuilderId)
        {
            return Context.DbContractBuilder.Join(Context.DbContract, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
                .Where(x => x.x.BuilderId == BuilderId && x.x.RowStatusId == (int)RowActiveStatus.Active && x.y.RowStatusId == (int)RowActiveStatus.Archived)
                .Select(m => m.x);
        }

        public IEnumerable<Contract> GetBuilderDeclinedContract(Int64 BuilderId, Int64 ContractStatus)
        {
            if (ContractStatus == 1)
            {
                return Context.DbContractBuilder.Join(Context.DbContract, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
                .Where(x => x.x.BuilderId == BuilderId && x.x.ContractStatusId == 1
                && x.x.RowStatusId == (int) RowActiveStatus.Archived && x.y.RowStatusId == (int) RowActiveStatus.Active)
                .Select(m => m.x.Contract);
            }
            else
            {
                return Context.DbContractBuilder.Join(Context.DbContract, x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
                .Where(x => x.x.BuilderId == BuilderId && x.x.ContractStatusId != 1
                && x.x.RowStatusId == (int) RowActiveStatus.Archived && x.y.RowStatusId == (int) RowActiveStatus.Active)
                .Select(m => m.x.Contract);
            }            
        }
    }
}