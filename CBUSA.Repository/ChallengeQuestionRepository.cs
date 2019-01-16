using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public class ChallengeQuestionRepository: Repository<ChallengeQuestion>, IChallengeQuestionRepository
    {
        public ChallengeQuestionRepository(CBUSADbContext Context)
            : base(Context)
        {
        }

        public IEnumerable<dynamic> GetChallengeQuestionDetails()
        {
            //IEnumerable<dynamic> ObjVirtualReportQuestionCount = (from p in Context.DbsOffenseType.Where(w => w.RowStatusId == (Int32)RowActiveStatus.Active)
            //                                     join c in Context.DbsVirtualReport on new { a = p.OffenseTypeId, b = (Int32)RowActiveStatus.Active }
            //                                               equals new { a = c.OffenseTypeId, b = c.RowStatusId }
            //                                               into j1
            //                                     from j2 in j1.DefaultIfEmpty()
            //                                     group j2 by new { p.OffenseTypeId, p.OffenseTypeName } into grouped
            //                                     select new { OffenseTypeName = grouped.Key.OffenseTypeName, OffenseTypeId = grouped.Key.OffenseTypeId, QuestionCount = grouped.Where(t => t.VirtualReportId != null).Count() }).ToList();
            IEnumerable<dynamic> ObjChallengeQuestionDetails = (from cq in Context.DbsChallengeQuestion.Where(w=>w.RowStatusId==(Int32)RowActiveStatus.Active)
                                                                  join ucq in Context.DbsUserChallangeQuestion on cq.ChallengeQuestionId equals ucq.ChallengeQuestionId
                                                                  into j1
                                                                  from j2 in j1.DefaultIfEmpty()
                                                                  group j2 by new { cq.ChallengeQuestionId, cq.ChallengeQuestionDescription, cq.RowStatusId } into grouped
                                                                  select new
                                                                  {
                                                                      ChallengeQuestionDescription = grouped.Key.ChallengeQuestionDescription,
                                                                      ChallengeQuestionId = grouped.Key.ChallengeQuestionId,
                                                                      RowStatusId = grouped.Key.RowStatusId,
                                                                      ChallengeQuestionUsedCount = grouped.Where(t => t.UserChallangeQuestionId != null).Count()
                                                                  }).ToList();


            return ObjChallengeQuestionDetails;
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
