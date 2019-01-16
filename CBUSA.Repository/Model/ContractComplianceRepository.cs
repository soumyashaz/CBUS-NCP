using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    class ContractComplianceRepository : Repository<ContractCompliance>, IContractComplianceRepository
    {

        public ContractComplianceRepository(CBUSADbContext Context)
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



        public decimal GetBuilderTotalActualValue(Int64 BuilderId, Int64 ContractId, List<Int64> QuestionidList)
        {
           //  Context.DbBuilderQuaterContractProjectReport.Join(Context.DbBuilderQuaterContractProjectDetails,y=>y. )
            var AnswareList = Context.DbBuilderQuaterContractProjectReport.
           Join(Context.DbBuilderQuaterContractProjectDetails, x => x.BuilderQuaterContractProjectReportId, y => y.BuilderQuaterContractProjectReportId, (x, y) =>
             new { x, y }).Where(z => z.x.ContractId == ContractId && z.x.BuilderId == BuilderId && QuestionidList.Contains(z.y.QuestionId)).Select(m => m.y).ToList();

            //  .Join(Context.DbTextBoxType, m => m.y.TextBoxTypeId, n => n.TextBoxTypeId, (m, n) => new { m, n })
            //  .Where(z => z.n.TextBoxTypeName == "Number").Select(Ques => Ques.m.x);

            decimal TotalActulas = 0;
            foreach (var Item in AnswareList)
            {
                decimal ChildActuals = 0;
                if (decimal.TryParse(Item.Answer, out ChildActuals))
                {
                    TotalActulas = TotalActulas + ChildActuals;
                }
            }
            return TotalActulas;
        }

       public decimal GetBuilderActualValueWithQuater(Int64 BuilderId, Int64 ContractId, List<Int64> QuestionidList, List<Int64> QuaterIdList)
        {
            var AnswareList = Context.DbBuilderQuaterContractProjectReport.
          Join(Context.DbBuilderQuaterContractProjectDetails, x => x.BuilderQuaterContractProjectReportId, y => y.BuilderQuaterContractProjectReportId, (x, y) =>
            new { x, y }).Where(z => z.x.ContractId == ContractId && z.x.BuilderId == BuilderId && QuestionidList.Contains(z.y.QuestionId)
            && QuaterIdList.Contains(z.x.QuaterId)
            ).Select(m => m.y).ToList();

            //  .Join(Context.DbTextBoxType, m => m.y.TextBoxTypeId, n => n.TextBoxTypeId, (m, n) => new { m, n })
            //  .Where(z => z.n.TextBoxTypeName == "Number").Select(Ques => Ques.m.x);

            decimal TotalActulas = 0;
            foreach (var Item in AnswareList)
            {
                decimal ChildActuals = 0;
                if (decimal.TryParse(Item.Answer, out ChildActuals))
                {
                    TotalActulas = TotalActulas + ChildActuals;
                }
            }
            return TotalActulas;
        }
    }

    class ContractComplianceBuilderRepository : Repository<ContractComplianceBuilder>, IContractComplianceBuilderRepository
    {

        public ContractComplianceBuilderRepository(CBUSADbContext Context)
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
    }
}