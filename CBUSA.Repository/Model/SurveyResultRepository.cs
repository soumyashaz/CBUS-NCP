using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Dynamic;


namespace CBUSA.Repository.Model
{
    class SurveyResultRepository : Repository<SurveyResult>, ISurveyResultRepository
    {

        public SurveyResultRepository(CBUSADbContext Context)
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

        public IEnumerable<dynamic> GetSurveyResponse(Int64 SurveyId)
        {
            var data = Context.DbSurveyBuilder.Join(Context.DbSurveyResult, x => x.SurveyId, y => y.SurveyId, (x, y) => new { x, y })
                .Join(Context.DbQuestion, s => s.y.QuestionId, p => p.QuestionId, (s, p) => new { s, p })
                .Where(n => n.s.y.SurveyId == SurveyId && n.s.x.RowStatusId==(int)RowActiveStatus.Active)
                .GroupBy(z => z.s.x.BuilderId).
                Select(m => new { BuilderId = m.Key, Result = m });
            List<dynamic> ObjResult = new List<dynamic>();
            foreach (var Item in data)
            {
                ObjResult.Add(new
                {
                    BuilderId = Item.BuilderId,
                    IsSurveyComplete = Item.Result.FirstOrDefault().s.x.IsSurveyCompleted,
                    Result = Item.Result
                });
            }
            return data;
        }


        public IEnumerable<SurveyResponse> GetBuilderSurveyResponse(Int64 SurveyId, Int64 BuilderId)
        {
            IQueryable<SurveyResult> SR = Context.DbSurveyResult.Where(sr => sr.SurveyId == SurveyId && sr.BuilderId == BuilderId && sr.RowStatusId == (int)RowActiveStatus.Active);
            IQueryable<Question> Q = Context.DbQuestion.Where(q => q.SurveyId == SurveyId && q.RowStatusId == (int)RowActiveStatus.Active);

            //var Data = Q.GroupJoin(SR, q => q.QuestionId, sr => sr.QuestionId, (q, sr) => new { questionset = q, resultset = sr })
            //            .SelectMany(m => m.resultset.DefaultIfEmpty(), (m, n) => new { QuestionSet2 = m.questionset, ResultSet2 = n })
            //            .GroupBy(x => new { x.QuestionSet2.QuestionId, x.QuestionSet2.QuestionTypeId, x.QuestionSet2.SurveyOrder })
            //            .Select(x => new SurveyResponse
            //            {
            //                QuestionId = x.Key.QuestionId,
            //                QuestionTypeId = x.Key.QuestionTypeId,
            //                SurveyOrder = x.Key.SurveyOrder,
            //                RowIndex = x.Select(g => g.ResultSet2.RowNumber).FirstOrDefault(),
            //                ColIndex = x.Select(g => g.ResultSet2.ColumnNumber).FirstOrDefault(),
            //                Result = x.Select(y => y.ResultSet2).OrderBy(z => new { z.SurveyResultId, z.RowNumber, z.ColumnNumber })
            //            }).OrderBy(x => x.SurveyOrder).ToList();

            var Data = Q.Join(SR, q => q.QuestionId, sr => sr.QuestionId, (q, sr) => new { questionset = q, resultset = sr })
                        .Select(m => new {
                                    QuestionSet2 = m.questionset,
                                    ResultSet2 = m.resultset
                                    })
                        .GroupBy(x => new { x.QuestionSet2.QuestionId, x.QuestionSet2.QuestionTypeId, x.QuestionSet2.SurveyOrder })
                        .Select(x => new SurveyResponse
                        {
                            QuestionId = x.Key.QuestionId,
                            QuestionTypeId = x.Key.QuestionTypeId,
                            SurveyOrder = x.Key.SurveyOrder,
                            RowIndex = x.Select(g => g.ResultSet2.RowNumber).FirstOrDefault(),
                            ColIndex = x.Select(g => g.ResultSet2.ColumnNumber).FirstOrDefault(),
                            Result = x.Select(y => y.ResultSet2).OrderBy(z => new { z.SurveyResultId, z.RowNumber, z.ColumnNumber })
                        }).OrderBy(x => x.SurveyOrder).ToList();

            //var Data = Context.DbQuestion.GroupJoin(Context.DbSurveyResult.Where(g => g.BuilderId == BuilderId && g.RowStatusId == (int)RowActiveStatus.Active), x => x.QuestionId, y => y.QuestionId,
            //(x, y) => new { QuestionSet = x, ResultSet = y }
            //).Where(x => x.QuestionSet.RowStatusId == (Int32)RowActiveStatus.Active && x.QuestionSet.SurveyId == SurveyId)

            //.SelectMany(m => m.ResultSet.DefaultIfEmpty(), (m, n) => new { Question = m.QuestionSet, Resultset = n })
            //    //  .Where(x => x.Resultset.BuilderId == BuilderId)
            //.GroupBy(x => new { x.Question.QuestionId, x.Question.QuestionTypeId, x.Question.SurveyOrder })
            //.Select(x => new SurveyResponse
            //{
            //    QuestionId = x.Key.QuestionId,
            //    QuestionTypeId = x.Key.QuestionTypeId,
            //    SurveyOrder = x.Key.SurveyOrder,
            //    RowIndex = x.Select(g => g.Resultset.RowNumber).FirstOrDefault(),
            //    ColIndex = x.Select(g => g.Resultset.ColumnNumber).FirstOrDefault(),
            //    Result = x.Select(y => y.Resultset).OrderBy(z => new {z.SurveyResultId, z.RowNumber, z.ColumnNumber })
            //}).OrderBy(x => x.SurveyOrder ).ToList();

            return Data;
        }
        public IEnumerable<SurveyResponse> GetBuilderSurveyResponseFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter)
        {
            var Data = Context.DbQuestion.GroupJoin(Context.DbSurveyResult.Where(g => g.BuilderId == BuilderId && g.RowStatusId == (int)RowActiveStatus.Active), x => x.QuestionId, y => y.QuestionId,
                (x, y) => new { QuestionSet = x, ResultSet = y }
                ).Where(x => QuestionFilter.Contains(x.QuestionSet.QuestionId) && x.QuestionSet.RowStatusId == (Int32)RowActiveStatus.Active && x.QuestionSet.SurveyId == SurveyId)

                .SelectMany(m => m.ResultSet.DefaultIfEmpty(), (m, n) => new { Question = m.QuestionSet, Resultset = n })
                .GroupBy(x => new { x.Question.QuestionId, x.Question.QuestionTypeId, x.Question.SurveyOrder })
                .Select(x => new SurveyResponse
                {
                    QuestionId = x.Key.QuestionId,
                    QuestionTypeId = x.Key.QuestionTypeId,
                    SurveyOrder = x.Key.SurveyOrder,
                    RowIndex = x.Select(g => g.Resultset.RowNumber).FirstOrDefault(),
                    ColIndex = x.Select(g => g.Resultset.ColumnNumber).FirstOrDefault(),
                    Result = x.Select(y => y.Resultset).OrderBy(z => new {z.SurveyResultId, z.RowNumber, z.ColumnNumber })
                }).OrderBy(x => x.SurveyOrder).ToList();


            return Data;
        }
        // added start newly - angshuman on 17/09/2016 
        public bool CheckNCPSurvey(Int64 SurveyId)
        {
            return Context.DbSurvey.Where(x => x.SurveyId == SurveyId).Select(a => a.IsNcpSurvey == true ? true : false).SingleOrDefault();
        }
        public IEnumerable<dynamic> GetParticipatedBuilderNCP(Int64 SurveyId, int SurveyCompleteFilter)
        {
            //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== Start
            var Survey = Context.DbSurvey.Where(x => x.SurveyId == SurveyId).FirstOrDefault();
            //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== End
            switch (SurveyCompleteFilter)
            {
                case 1:
                    //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== Start
                    //var data1 = Context.DbBuilderQuaterContractProjectReport.Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }) //.Select( d=> d.x.BuilderId).SingleOrDefault()
                    //    .Join(Context.DbBuilderQuaterAdminReport, t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                    //    .GroupBy(z => z.t.x.BuilderId)
                    //    .Select(m => new { BuilderId = m.Key, Result = m });
                    this.Context.Database.CommandTimeout = 600;
                    var data1 = Context.DbBuilderQuaterContractProjectReport
                        .Join(Context.DbBuilderQuaterContractProjectDetails, p => p.BuilderQuaterContractProjectReportId, q => q.BuilderQuaterContractProjectReportId, (p, q) => new { p, q })
                        .Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.p.ContractId, y => y.ContractId, (x, y) => new { x, y }) 
                        .Join(Context.DbBuilderQuaterAdminReport, t => t.x.p.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                        .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
                        .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
                        .GroupBy(z => z.m.t.x.p.BuilderId)
                        .Select(m => new { BuilderId = m.Key, Result = m });
                    //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== End
                    List<dynamic> ObjResult1 = new List<dynamic>();
                    foreach (var Item in data1)
                    {
                        ObjResult1.Add(new
                        {
                            SurveyId = SurveyId,
                            BuilderId = Item.BuilderId,
                            BuilderQuaterAdminReportId = Item.Result.FirstOrDefault().m.h.BuilderQuaterAdminReportId,
                            IsSurveyCompleted = Item.Result.FirstOrDefault().m.h.IsSubmit,
                            //ProjectId = Item.Result.FirstOrDefault().t.x.ProjectId,
                            //ContractId = Item.Result.FirstOrDefault().t.x.ContractId,
                            //QuaterId = Item.Result.FirstOrDefault().t.x.QuaterId,
                            ProjectId = Item.Result.FirstOrDefault().m.t.x.p.ProjectId,
                            ContractId = Item.Result.FirstOrDefault().m.t.x.p.ContractId,
                            QuaterId = Item.Result.FirstOrDefault().m.t.x.p.QuaterId,
                            Quater = Item.Result.FirstOrDefault().m.t.y.Quater,
                            Year = Item.Result.FirstOrDefault().m.t.y.Year
                        });
                    }
                    
                    return ObjResult1;
                case 2:
                    //var data2 = Context.DbBuilderQuaterContractProjectReport.Where(p => p.IsComplete == true).Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }) //.Select( d=> d.x.BuilderId).SingleOrDefault()
                    //    .Join(Context.DbBuilderQuaterAdminReport, t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                    //    .Where(a=> a.h.IsSubmit == true)
                    //    .GroupBy(z => z.t.x.BuilderId)
                    //    .Select(m => new { BuilderId = m.Key, Result = m });

                    //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== Start
                    //var data2 = Context.DbBuilderQuaterContractProjectReport.Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }) //.Select( d=> d.x.BuilderId).SingleOrDefault()
                    //    .Join(Context.DbBuilderQuaterAdminReport, t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                    //    .Where(a => a.h.IsSubmit == true)
                    //    .GroupBy(z => z.t.x.BuilderId)
                    //    .Select(m => new { BuilderId = m.Key, Result = m });

                    var data2 = Context.DbBuilderQuaterContractProjectReport
                       .Join(Context.DbBuilderQuaterContractProjectDetails, p => p.BuilderQuaterContractProjectReportId, q => q.BuilderQuaterContractProjectReportId, (p, q) => new { p, q })
                       .Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.p.ContractId, y => y.ContractId, (x, y) => new { x, y }) //.Select( d=> d.x.BuilderId).SingleOrDefault()
                       .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == true), t => t.x.p.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                       .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
                       .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
                       .GroupBy(z => z.m.t.x.p.BuilderId)
                       .Select(m => new { BuilderId = m.Key, Result = m });

                    //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== End

                    List<dynamic> ObjResult2 = new List<dynamic>();
                    foreach (var Item in data2)
                    {
                        ObjResult2.Add(new
                        {
                            SurveyId = SurveyId,
                            BuilderId = Item.BuilderId,
                            BuilderQuaterAdminReportId = Item.Result.FirstOrDefault().m.h.BuilderQuaterAdminReportId,
                            IsSurveyCompleted = Item.Result.FirstOrDefault().m.h.IsSubmit,
                            ProjectId = Item.Result.FirstOrDefault().m.t.x.p.ProjectId,
                            ContractId = Item.Result.FirstOrDefault().m.t.x.p.ContractId,
                            QuaterId = Item.Result.FirstOrDefault().m.t.x.p.QuaterId,
                            Quater = Item.Result.FirstOrDefault().m.t.y.Quater,
                            Year = Item.Result.FirstOrDefault().m.t.y.Year
                        });
                    }
                    return ObjResult2;
                case 3:
                    //var data3 = Context.DbBuilderQuaterContractProjectReport.Where(p => p.IsComplete == false).Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }) //.Select( d=> d.x.BuilderId).SingleOrDefault()
                    //    .Join(Context.DbBuilderQuaterAdminReport, t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                    //    .Where(a => a.h.IsSubmit == false)
                    //    .GroupBy(z => z.t.x.BuilderId)
                    //    .Select(m => new { BuilderId = m.Key, Result = m });

                    //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== Start
                    //var data3 = Context.DbBuilderQuaterContractProjectReport.Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.ContractId, y => y.ContractId, (x, y) => new { x, y }) //.Select( d=> d.x.BuilderId).SingleOrDefault()
                    //    .Join(Context.DbBuilderQuaterAdminReport, t => t.x.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                    //    .Where(a => a.h.IsSubmit == false)
                    //    .GroupBy(z => z.t.x.BuilderId)
                    //    .Select(m => new { BuilderId = m.Key, Result = m });

                    var data3 = Context.DbBuilderQuaterContractProjectReport
                        .Join(Context.DbBuilderQuaterContractProjectDetails, p => p.BuilderQuaterContractProjectReportId, q => q.BuilderQuaterContractProjectReportId, (p, q) => new { p, q })
                        .Join(Context.DbSurvey.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.p.ContractId, y => y.ContractId, (x, y) => new { x, y }) 
                        .Join(Context.DbBuilderQuaterAdminReport.Where(h => h.IsSubmit == false), t => t.x.p.BuilderQuaterAdminReportId, h => h.BuilderQuaterAdminReportId, (t, h) => new { t, h })
                        .Join(Context.DbQuater, m => m.h.QuaterId, n => n.QuaterId, (m, n) => new { m, n })
                        .Where(x => x.n.QuaterName == Survey.Quater && x.n.Year.ToString() == Survey.Year)
                        .GroupBy(z => z.m.t.x.p.BuilderId)
                        .Select(m => new { BuilderId = m.Key, Result = m });
                    
                    //=========== Cnahged by Neyaz on 24-Oct-2017 ==== VSO#8565 ==== End

                    List<dynamic> ObjResult3 = new List<dynamic>();
                    foreach (var Item in data3)
                    {
                        ObjResult3.Add(new
                        {
                            SurveyId = SurveyId,
                            BuilderId = Item.BuilderId,
                            BuilderQuaterAdminReportId = Item.Result.FirstOrDefault().m.h.BuilderQuaterAdminReportId,
                            IsSurveyCompleted = Item.Result.FirstOrDefault().m.h.IsSubmit,
                            ProjectId = Item.Result.FirstOrDefault().m.t.x.p.ProjectId,
                            ContractId = Item.Result.FirstOrDefault().m.t.x.p.ContractId,
                            QuaterId = Item.Result.FirstOrDefault().m.t.x.p.QuaterId,
                            Quater = Item.Result.FirstOrDefault().m.t.y.Quater,
                            Year = Item.Result.FirstOrDefault().m.t.y.Year
                        });
                    }
                    return ObjResult3;
            }
            return null;
        }
        public IEnumerable<dynamic> GetParticipatedBuilderActive(Int64 SurveyId, int SurveyCompleteFilter)
        {
            switch (SurveyCompleteFilter)
            {
                case 1:
                    var data1 = Context.DbSurveyBuilder.Join(Context.DbSurveyResult.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.BuilderId, y => y.BuilderId, (x, y) => new { x, y }) 
                        .GroupBy(z => z.x.BuilderId)
                        .Select(m => new { BuilderId = m.Key, Result = m });
                    List<dynamic> ObjResult1 = new List<dynamic>();
                    foreach (var Item in data1)
                    {
                        ObjResult1.Add(new
                        {
                            SurveyBuilderId = Item.Result.FirstOrDefault().x.SurveyBuilderId,
                            SurveyStartDate = Item.Result.FirstOrDefault().x.SurveyStartDate,
                            SurveyId = Item.Result.FirstOrDefault().x.SurveyId,
                            BuilderId = Item.Result.FirstOrDefault().x.BuilderId,
                            IsSurveyCompleted = Item.Result.FirstOrDefault().x.IsSurveyCompleted,
                            SurveyCompleteDate = Item.Result.FirstOrDefault().x.SurveyCompleteDate,
                            RowStatusId = Item.Result.FirstOrDefault().x.RowStatusId,
                            CreatedOn = Item.Result.FirstOrDefault().x.CreatedOn,
                            CreatedBy = Item.Result.FirstOrDefault().x.CreatedBy,
                            ModifiedOn = Item.Result.FirstOrDefault().x.ModifiedOn,
                            ModifiedBy = Item.Result.FirstOrDefault().x.ModifiedBy,
                            RowGUID = Item.Result.FirstOrDefault().x.RowGUID

                        });
                    }

                    return ObjResult1;
                case 2:
                    var data2 = Context.DbSurveyBuilder.Join(Context.DbSurveyResult.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.BuilderId, y => y.BuilderId, (x, y) => new { x, y }) 
                        .Where(a=> a.x.IsSurveyCompleted == true)
                        .GroupBy(z => z.x.BuilderId)
                        .Select(m => new { BuilderId = m.Key, Result = m });
                    List<dynamic> ObjResult2 = new List<dynamic>();
                    foreach (var Item in data2)
                    {
                        ObjResult2.Add(new
                        {
                            SurveyBuilderId = Item.Result.FirstOrDefault().x.SurveyBuilderId,
                            SurveyStartDate = Item.Result.FirstOrDefault().x.SurveyStartDate,
                            SurveyId = Item.Result.FirstOrDefault().x.SurveyId,
                            BuilderId = Item.Result.FirstOrDefault().x.BuilderId,
                            IsSurveyCompleted = Item.Result.FirstOrDefault().x.IsSurveyCompleted,
                            SurveyCompleteDate = Item.Result.FirstOrDefault().x.SurveyCompleteDate,
                            RowStatusId = Item.Result.FirstOrDefault().x.RowStatusId,
                            CreatedOn = Item.Result.FirstOrDefault().x.CreatedOn,
                            CreatedBy = Item.Result.FirstOrDefault().x.CreatedBy,
                            ModifiedOn = Item.Result.FirstOrDefault().x.ModifiedOn,
                            ModifiedBy = Item.Result.FirstOrDefault().x.ModifiedBy,
                            RowGUID = Item.Result.FirstOrDefault().x.RowGUID

                        });
                    }
                    return ObjResult2;
                case 3:
                    var data3 = Context.DbSurveyBuilder.Join(Context.DbSurveyResult.Where(a => a.SurveyId == SurveyId && a.RowStatusId == (int)RowActiveStatus.Active), x => x.BuilderId, y => y.BuilderId, (x, y) => new { x, y }) 
                        .Where(a=> a.x.IsSurveyCompleted == false)
                        .GroupBy(z => z.x.BuilderId)
                        .Select(m => new { BuilderId = m.Key, Result = m });
                    List<dynamic> ObjResult3 = new List<dynamic>();
                    foreach (var Item in data3)
                    {
                        ObjResult3.Add(new
                        {
                            SurveyBuilderId = Item.Result.FirstOrDefault().x.SurveyBuilderId,
                            SurveyStartDate = Item.Result.FirstOrDefault().x.SurveyStartDate,
                            SurveyId = Item.Result.FirstOrDefault().x.SurveyId,
                            BuilderId = Item.Result.FirstOrDefault().x.BuilderId,
                            IsSurveyCompleted = Item.Result.FirstOrDefault().x.IsSurveyCompleted,
                            SurveyCompleteDate = Item.Result.FirstOrDefault().x.SurveyCompleteDate,
                            RowStatusId = Item.Result.FirstOrDefault().x.RowStatusId,
                            CreatedOn = Item.Result.FirstOrDefault().x.CreatedOn,
                            CreatedBy = Item.Result.FirstOrDefault().x.CreatedBy,
                            ModifiedOn = Item.Result.FirstOrDefault().x.ModifiedOn,
                            ModifiedBy = Item.Result.FirstOrDefault().x.ModifiedBy,
                            RowGUID = Item.Result.FirstOrDefault().x.RowGUID

                        });
                    }
                    return ObjResult3;
            }
            return null;
        }
        public IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCP(Int64 SurveyId, Int64 BuilderId)
        {
            var Data = Context.DbQuestion.Join(Context.DbSurvey, t=> t.SurveyId, h=> h.SurveyId,(t, h)=> new{t, h}) 
                .Where(s => s.t.SurveyId == SurveyId)  //  && s.h.Year == ? && s.h.Quater == ? - angshuman 
                .GroupJoin(Context.DbBuilderQuaterContractProjectReport.Join(Context.DbBuilderQuaterContractProjectDetails.Where(w=> w.RowStatusId == (int)RowActiveStatus.Active), a => a.BuilderQuaterContractProjectReportId, b => b.BuilderQuaterContractProjectReportId, (a, b) => new { a, b }), x => x.t.QuestionId, b => b.b.QuestionId,
                (x, y) => new { QuestionSet = x.t, ResultSet = y }
                ).Where(x => x.QuestionSet.RowStatusId == (Int32)RowActiveStatus.Active && x.QuestionSet.SurveyId == SurveyId)
                .SelectMany(m => m.ResultSet.DefaultIfEmpty(), (m, n) => new { Question = m.QuestionSet, Resultset = n })
                .Where(x => x.Resultset.a.BuilderId == BuilderId)
                .GroupBy(x => new {x.Resultset.b.BuilderQuaterContractProjectDetailsId, x.Question.QuestionId, x.Question.QuestionTypeId, x.Question.SurveyOrder })
                .Select(x => new SurveyResponseNCP
                {
                    BuilderQuaterContractProjectDetailsId = x.Select(g => g.Resultset.b.BuilderQuaterContractProjectDetailsId).FirstOrDefault(),
                    QuestionId = x.Key.QuestionId,
                    QuestionTypeId = x.Key.QuestionTypeId,
                    SurveyOrder = x.Key.SurveyOrder,
                    ContractId = x.Select(g => g.Resultset.a.ContractId).FirstOrDefault(),
                    QuaterId = x.Select(g => g.Resultset.a.QuaterId).FirstOrDefault(),
                    ProjectId = x.Select(g => g.Resultset.a.ProjectId).FirstOrDefault(),
                    //BuilderId = x.Select(g => g.Resultset.a.BuilderId).FirstOrDefault(),
                    //Year = x.Select(g => g.Resultset.a.Year).FirstOrDefault(), 
                    RowIndex = x.Select(g => g.Resultset.b.RowNumber).FirstOrDefault(),
                    ColIndex = x.Select(g => g.Resultset.b.ColumnNumber).FirstOrDefault(),
                    Result = x.Select(y => y.Resultset.b).OrderBy(z => new { z.BuilderQuaterContractProjectDetailsId, z.RowNumber, z.ColumnNumber })
                }).OrderBy(d => d.BuilderQuaterContractProjectDetailsId)
               // .OrderBy(z=>z.ProjectId).OrderBy(x=>x.QuestionId)
                .ToList();
            return Data;

        }
        public IEnumerable<dynamic> GetBuilderSurveyResponseNCPNew(Int64 SurveyId, List<Int64> QuestionFilter)
        {
            //List<dynamic> data = new List<dynamic>();
            //dynamic data;
            //List<dynamic> Result = new List<dynamic>();

            //// - get question list first 
            //List<SurveyResponseDynamicRepository> QuestionList = new List<SurveyResponseDynamicRepository>();
            //SqlParameter parameterSurveyId1 = new SqlParameter("@SurveyId", SurveyId);
            ////SqlParameter parameterQuestionList1 = new SqlParameter("@QuestionIdList", QuestionFilter.ToString());
            //SqlParameter parameterQuestionList1 = new SqlParameter("@QuestionIdList", "");
            //QuestionList = Context.Database.SqlQuery<SurveyResponseDynamicRepository>("exec proc_GetNCPSurveyResponseQuestionHeader_New @SurveyId, @QuestionIdList ", parameterSurveyId1, parameterQuestionList1).ToList();
            //// - get question list first 
            //TypeBuilder obj = CreateTypeBuilder("MyDynamicAssembly", "NCPResponseAnswer", "MyType");
            ////CreateAutoImplementedProperty(obj, "RowNumber", typeof(int));
            ////CreateAutoImplementedProperty(obj, "MarketName", typeof(string));
            ////CreateAutoImplementedProperty(obj, "BuilderCompany", typeof(string));

            //foreach(var item in QuestionList)
            //{
            //    CreateAutoImplementedProperty(obj, item.QuestionValue, typeof(string));
            //    Result.Add(item.QuestionValue);
            //}

            //Type resultType = obj.CreateType();

            //if (QuestionFilter.Count >0)
            //{
            //    SqlParameter parameterSurveyId = new SqlParameter("@SurveyId", SurveyId);
            //    //SqlParameter parameterQuestionList = new SqlParameter("@QuestionIdList", QuestionIdListFilter != "" ? QuestionIdListFilter : null);
            //    SqlParameter parameterQuestionList = new SqlParameter("@QuestionIdList", QuestionFilter);
            //    data = Context.Database.SqlQuery(resultType, "exec Proc_GetNcpResult @SurveyId, @QuestionIdList ", parameterSurveyId, parameterQuestionList);
            //}
            //else
            //{
            //    SqlParameter parameterSurveyId = new SqlParameter("@SurveyId", SurveyId);
            //    //SqlParameter parameterQuestionList = new SqlParameter("@QuestionIdList", QuestionIdListFilter != "" ? QuestionIdListFilter : null);
            //    SqlParameter parameterQuestionList = new SqlParameter("@QuestionIdList", "");
            //    //data = Context.Database.SqlQuery<SurveyResponseDynamicRepository>("exec Proc_GetNcpResult @SurveyId, @QuestionIdList ", parameterSurveyId, parameterQuestionList).ToList();
            //    var xx = Context.Database.SqlQuery("exec Proc_GetNcpResult @SurveyId, @QuestionIdList ", parameterSurveyId, parameterQuestionList);
            //}
            ////Result = data;
            //return data;
            string QuestionList = QuestionFilter.ToString();
            SqlConnection con;
            string constr = ConfigurationManager.ConnectionStrings["CBUSA"].ToString();
            con = new SqlConnection(constr);
            SqlCommand com = new SqlCommand("Proc_GetNcpResult", con);
            com.Parameters.AddWithValue("@SurveyId", SurveyId);
            com.Parameters.AddWithValue("@QuestionIdList", QuestionFilter.Count > 0 ? QuestionList : "");
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            List<dynamic> data = new List<dynamic>();
            data = dynamicDt.ToList();

            
            return data;

        }
        public string GetContractQuater(Int64 SurveyId)
        {
            var data = Context.DbSurvey.Join(Context.DbQuater, a => a.Quater, b => b.QuaterName, (a, b) => new { a, b })
                        .Join(Context.DbContract, c => c.a.ContractId, d => d.ContractId, (c, d) => new { c, d })
                        .Where(x => x.c.a.SurveyId == SurveyId)
                        .Select(y => new { QuaterName = y.c.b.QuaterName, ContractName = y.d.ContractName }).FirstOrDefault();

            return data.ContractName + " - " + data.QuaterName;
        }
        public IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCPFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter)
        {
            //var Data = Context.DbQuestion.Join(Context.DbSurvey, t => t.SurveyId, h => h.SurveyId, (t, h) => new { t, h })
            //    .Where(s => s.t.SurveyId == SurveyId)  //  && s.h.Year == ? && s.h.Quater == ? - angshuman 
            //    .GroupJoin(Context.DbBuilderQuaterContractProjectReport.Join(Context.DbBuilderQuaterContractProjectDetails.Where(w => w.RowStatusId == (int)RowActiveStatus.Active), a => a.BuilderQuaterContractProjectReportId, b => b.BuilderQuaterContractProjectReportId, (a, b) => new { a, b }), x => x.t.QuestionId, f => f.b.QuestionId,
            //    (x, y) => new { QuestionSet = x.t, ResultSet = y }
            //    ).Where(x => QuestionFilter.Contains(x.QuestionSet.QuestionId) && x.QuestionSet.RowStatusId == (Int32)RowActiveStatus.Active && x.QuestionSet.SurveyId == SurveyId)
            //    .SelectMany(m => m.ResultSet.DefaultIfEmpty(), (m, n) => new { Question = m.QuestionSet, Resultset = n })
            //    .Where(x => x.Resultset.a.BuilderId == BuilderId)
            //    .GroupBy(x => new { x.Resultset.b.BuilderQuaterContractProjectDetailsId, x.Question.QuestionId, x.Question.QuestionTypeId, x.Question.SurveyOrder })
            //    .Select(x => new SurveyResponseNCP
            //    {
            //        BuilderQuaterContractProjectDetailsId = x.Select(g=> g.Resultset.b.BuilderQuaterContractProjectDetailsId).FirstOrDefault(),
            //        QuestionId = x.Key.QuestionId,
            //        QuestionTypeId = x.Key.QuestionTypeId,
            //        SurveyOrder = x.Key.SurveyOrder,
            //        ContractId = x.Select(g => g.Resultset.a.ContractId).FirstOrDefault(),
            //        QuaterId = x.Select(g => g.Resultset.a.QuaterId).FirstOrDefault(),
            //        ProjectId = x.Select(g => g.Resultset.a.ProjectId).FirstOrDefault(),
            //        //BuilderId = x.Select(g => g.Resultset.a.BuilderId).FirstOrDefault(),
            //        //Year = x.Select(g => g.Resultset.a.Year).FirstOrDefault(),
            //        Result = x.Select(y => y.Resultset.b).OrderBy(z => new { z.BuilderQuaterContractProjectDetailsId, z.RowNumber, z.ColumnNumber })
            //    }).OrderBy(x=> x.BuilderQuaterContractProjectDetailsId).ToList();

            var Data = Context.DbQuestion.Join(Context.DbSurvey, t => t.SurveyId, h => h.SurveyId, (t, h) => new { t, h })
                .Where(s => s.t.SurveyId == SurveyId)  //  && s.h.Year == ? && s.h.Quater == ? - angshuman 
                .GroupJoin(Context.DbBuilderQuaterContractProjectReport.Join(Context.DbBuilderQuaterContractProjectDetails.Where(w => w.RowStatusId == (int)RowActiveStatus.Active), a => a.BuilderQuaterContractProjectReportId, b => b.BuilderQuaterContractProjectReportId, (a, b) => new { a, b }), x => x.t.QuestionId, f => f.b.QuestionId,
                (x, y) => new { QuestionSet = x.t, ResultSet = y }
                ).Where(x => QuestionFilter.Contains(x.QuestionSet.QuestionId) && x.QuestionSet.RowStatusId == (Int32)RowActiveStatus.Active && x.QuestionSet.SurveyId == SurveyId)
                .SelectMany(m => m.ResultSet.DefaultIfEmpty(), (m, n) => new { Question = m.QuestionSet, Resultset = n })
                .Where(x => x.Resultset.a.BuilderId == BuilderId)
                .GroupBy(x => new { x.Resultset.b.BuilderQuaterContractProjectDetailsId, x.Question.QuestionId, x.Question.QuestionTypeId, x.Question.SurveyOrder })
                .Select(x => new SurveyResponseNCP
                {
                    BuilderQuaterContractProjectDetailsId = x.Select(g => g.Resultset.b.BuilderQuaterContractProjectDetailsId).FirstOrDefault(),
                    QuestionId = x.Key.QuestionId,
                    QuestionTypeId = x.Key.QuestionTypeId,
                    SurveyOrder = x.Key.SurveyOrder,
                    ContractId = x.Select(g => g.Resultset.a.ContractId).FirstOrDefault(),
                    QuaterId = x.Select(g => g.Resultset.a.QuaterId).FirstOrDefault(),
                    ProjectId = x.Select(g => g.Resultset.a.ProjectId).FirstOrDefault(),
                    RowIndex = x.Select(g => g.Resultset.b.RowNumber).FirstOrDefault(),
                    ColIndex = x.Select(g => g.Resultset.b.ColumnNumber).FirstOrDefault(),
                    //BuilderId = x.Select(g => g.Resultset.a.BuilderId).FirstOrDefault(),
                    //Year = x.Select(g => g.Resultset.a.Year).FirstOrDefault(),
                    Result = x.Select(y => y.Resultset.b).OrderBy(z => new { z.BuilderQuaterContractProjectDetailsId, z.RowNumber, z.ColumnNumber })
                }).OrderBy(x => x.BuilderQuaterContractProjectDetailsId).ToList();

            return Data;
        }
        // added end newly - angshuman on 17/09/2016 
    }
}