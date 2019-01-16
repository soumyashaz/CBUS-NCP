using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    public class SurveyRepository : Repository<Survey>, ISurveyRepository
    {
        public SurveyRepository(CBUSADbContext Context)
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

        //public IEnumerable<Survey> GetBuilderAssocaitedSurvey(Int64 SurveyId)
        //{
        //    return Context.DbSurvey.Where(x => (x.SurveyId == (int)SurveyId));

        //}
        public int GetCompleteedBuilderNCP(Int64 SurveyId)
        {

            var Survey = Context.DbSurvey.Where(x => x
              .SurveyId == SurveyId).FirstOrDefault();

            // ======== Neyaz -- On 13-oct-2017 ------ #8373 ============== Start
            // changed on 22-may-2017 - angshuman as the complete and incomplete response count was showing wrong
            //var data1 = Context.DbBuilderQuaterContractProjectReport.
            //    Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId),
            //    x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
            //    .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == true),
            //    t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
            //    .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
            //    .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
            //    .GroupBy(z => z.m.t.x.BuilderId).Count();

            var data1 = Context.DbBuilderQuaterContractProjectReport.
                Join(Context.DbBuilderQuaterContractProjectDetails,
                a => a.BuilderQuaterContractProjectReportId, b=> b.BuilderQuaterContractProjectReportId,(a,b) => new {a,b}).                
                Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId),
                x => x.a.ContractId, y => y.ContractId, (x, y) => new { x, y })
                .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == true),
                t => t.x.a.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
                .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
                .GroupBy(z => z.m.t.x.a.BuilderId).Count();
            // ======== Neyaz -- On 13-oct-2017 ------ #8373 ============== End

            //var data = Context.DbBuilderQuaterContractProjectReport.Where(a => a.IsComplete == true).
            //    Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId),
            //    x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
            //    //.Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == false),
            //    //t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
            //    .Join(Context.DbQuater, m => m.x.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
            //    .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
            //    .GroupBy(z => z.m.x.BuilderId).Count();
            // changed on 22-may-2017 - angshuman as the complete and incomplete response count was showing wrong





            return data1;
        }
        public int GetInCompleteedBuilderNCP(Int64 SurveyId)
        {
            //var data1 = Context.DbBuilderQuaterContractProjectReport.Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }) //.Select( d=> d.x.BuilderId).SingleOrDefault()
            //        .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == false), t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
            //        .GroupBy(z => z.t.x.BuilderId).Count();

            //return data1;

            var Survey = Context.DbSurvey.Where(x => x
              .SurveyId == SurveyId).FirstOrDefault();


            //var data2 = Context.DbBuilderQuaterContractProjectReport.
            //    Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId),
            //    x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
            //    .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == false),
            //    t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
            //    .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
            //    .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year).Select(t=>t);//.Distinct()
            //    .GroupBy(z => z.m.t.x.BuilderId);


            // ======== Neyaz -- On 13-oct-2017 ------ #8373 ============== Start
            // changed on 22-may-2017 - angshuman as the complete and incomplete response count was showing wrong
            //var data1 = Context.DbBuilderQuaterContractProjectReport.
            //    Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId),
            //    x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
            //    .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == false),
            //    t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
            //    .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
            //    .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
            //    .GroupBy(z => z.m.t.x.BuilderId).Count();

            var data1 = Context.DbBuilderQuaterContractProjectReport.
                Join(Context.DbBuilderQuaterContractProjectDetails,
                a => a.BuilderQuaterContractProjectReportId, b => b.BuilderQuaterContractProjectReportId, (a, b) => new { a, b }).
                Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId),
                x => x.a.ContractId, y => y.ContractId, (x, y) => new { x, y })
                .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == false),
                t => t.x.a.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
                .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
                .GroupBy(z => z.m.t.x.a.BuilderId).Count();

            // ======== Neyaz -- On 13-oct-2017 ------ #8373 ============== End

            //var data = Context.DbBuilderQuaterContractProjectReport.Where(a => a.IsComplete == false).
            //    Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId),
            //    x => x.ContractId, y => y.ContractId, (x, y) => new { x, y })
            //    //.Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == false),
            //    //t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
            //    .Join(Context.DbQuater, m => m.x.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
            //    .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
            //    .GroupBy(z => z.m.x.BuilderId).Count();
            // changed on 22-may-2017 - angshuman as the complete and incomplete response count was showing wrong
            return data1;

        }

        public IEnumerable<Builder> BuilderAllreadyGetInvitationForSurvey(Int64 SurveyId, bool IsEnrollmentSurvey)
        {

            if (IsEnrollmentSurvey)  //enrollment survey then we have to consider the builder who have allready join the contract
            {
                List<Builder> AllreadyJoinBuilder = Context.DbSurvey.Where(n => n.SurveyId == SurveyId)
                   .Join(Context.DbContractBuilder,
                   x => x.ContractId, y => y.ContractId,
                   (x, y) => y).Join(Context.DbBuilder,
                   z => z.BuilderId, u => u.BuilderId, (z, u) => u).ToList();

                //   .Select(x => x).ToList();

                List<Builder> AllreadySendBuilder = Context.DbSurvey.Where(n => n.SurveyId == SurveyId)
                   .Join(Context.DbBuilderSurveyEmailSent.Where(n => n.IsMailSent == true),
                   x => x.SurveyId, y => y.SurveyId,
                   (x, y) => y).Join(Context.DbBuilder,
                   //  Where(x => !AllreadyJoinBuilder.Contains(x.BuilderId)),
                   z => z.BuilderId, u => u.BuilderId, (z, u) => u).ToList();

                AllreadySendBuilder.AddRange(AllreadyJoinBuilder);
                return AllreadySendBuilder;

            }
            else
            {

                return Context.DbSurvey.Where(n => n.SurveyId == SurveyId)
                   .Join(Context.DbBuilderSurveyEmailSent.Where(n => n.IsMailSent == true),
                   x => x.SurveyId, y => y.SurveyId,
                   (x, y) => y).Join(Context.DbBuilder, z => z.BuilderId, u => u.BuilderId, (z, u) => u);

            }
        }

    }
}
