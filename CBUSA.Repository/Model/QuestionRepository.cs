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
    public class QuestionTypeRepository : Repository<QuestionType>, IQuestionTypeRepository
    {

        public QuestionTypeRepository(CBUSADbContext Context)
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

    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {

        public QuestionRepository(CBUSADbContext Context)
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

        public IEnumerable<Question> GetNumericQuestion(Int64 SurveyId)
        {
            return Context.DbQuestion.Where(Surv => Surv.SurveyId == SurveyId).
                Join(Context.DbQuestionTextBoxSetting, x => x.QuestionId, y => y.QuestionId, (x, y) =>
                  new { x, y }).Join(Context.DbTextBoxType, m => m.y.TextBoxTypeId, n => n.TextBoxTypeId, (m, n) => new { m, n })
                  .Where(z => z.n.TextBoxTypeName == "Number").Select(Ques => Ques.m.x);
        }

        public Question GetQuestionWithIndexNo(Int64 SurveyId, int IndexNo)
        {

            return Context.DbQuestion.Where(Surv => Surv.SurveyId == SurveyId && Surv.RowStatusId == (Int16)RowActiveStatus.Active).OrderBy(z => z.SurveyOrder)
               .Skip(IndexNo - 1).Take(1).Select(y => y).FirstOrDefault();
        }
        public IEnumerable<dynamic> GetQuestionValueWithGrid(Int64 SurveyId, string QuestionIdListFilter)
        {
            List<SurveyResponseDynamicRepository> data = new List<SurveyResponseDynamicRepository>();
            bool CheckIsNCPSurvey = Context.Database.SqlQuery<bool>("Select IsNcpSurvey  From Survey Where SurveyId = '" + SurveyId + "'").SingleOrDefault();// .DbSurvey.Where(a => a.Year != null && a.SurveyId == SurveyId).Select(b => b.Year != null ? b.Year : "").ToString();
            
            SqlParameter parameterSurveyId = new SqlParameter("@SurveyId", SurveyId);
            //SqlParameter parameterQuestionList = new SqlParameter("@QuestionIdList", QuestionIdListFilter != "" ? QuestionIdListFilter : null);
            SqlParameter parameterQuestionList = new SqlParameter("@QuestionIdList", QuestionIdListFilter);
            if (CheckIsNCPSurvey == true )
            {
                data = Context.Database.SqlQuery<SurveyResponseDynamicRepository>("exec proc_GetNCPSurveyResponseQuestionHeader_New @SurveyId, @QuestionIdList ", parameterSurveyId, parameterQuestionList).ToList();
            }
            else
            {
                data = Context.Database.SqlQuery<SurveyResponseDynamicRepository>("exec proc_GetSurveyResponseQuestionHeader @SurveyId, @QuestionIdList ", parameterSurveyId, parameterQuestionList).ToList();
            }
            return data;
        }
        public IEnumerable<dynamic> GetGridQuestionHeader(Int64 QuestionId, Int64? QuestionSettingId, bool? IsTypeOfGridQuestionHeaderSettingId)
        {
            if(QuestionSettingId.HasValue && QuestionSettingId.GetValueOrDefault() >0)
            {
                if(IsTypeOfGridQuestionHeaderSettingId.GetValueOrDefault() == true)
                {
                    return Context.DbQuestionGridSetting.Where(a => a.QuestionId == QuestionId).
                        Join(Context.DbQuestionGridSettingHeader, x => x.QuestionGridSettingId, y => y.QuestionGridSettingId, (x, y) =>
                           new { x, y })
                            .Where(z => z.y.QuestionGridSettingHeaderId == QuestionSettingId).Select(b => b.y);
                }
                else
                {
                    return Context.DbQuestionGridSetting.Where(a => a.QuestionId == QuestionId).
                        Join(Context.DbQuestionGridSettingHeader, x => x.QuestionGridSettingId, y => y.QuestionGridSettingId, (x, y) =>
                           new { x, y })
                            .Where(z => z.x.QuestionGridSettingId == QuestionSettingId).Select(b => b.y);
                }
                
            }
            else
            {
                return Context.DbQuestionGridSetting.Where(a => a.QuestionId == QuestionId).
                        Join(Context.DbQuestionGridSettingHeader, x => x.QuestionGridSettingId, y => y.QuestionGridSettingId, (x, y) =>
                           new { x, y })
                            .Select(b => b.y);
            }
             
        }
    }


    public class TextBoxTypeRepository : Repository<TextBoxType>, ITextBoxTypeRepository
    {

        public TextBoxTypeRepository(CBUSADbContext Context)
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


    public class QuestionTextBoxSettingRepository : Repository<QuestionTextBoxSetting>, IQuestionTextBoxSettingRepository
    {

        public QuestionTextBoxSettingRepository(CBUSADbContext Context)
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

    public class QuestionDropdownSettingRepository : Repository<QuestionDropdownSetting>, IQuestionDropdownSettingRepository
    {

        public QuestionDropdownSettingRepository(CBUSADbContext Context)
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


    public class QuestionGridSettingRepository : Repository<QuestionGridSetting>, IQuestionGridSettingRepository
    {

        public QuestionGridSettingRepository(CBUSADbContext Context)
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

    public class QuestionGridSettingHeaderRepository : Repository<QuestionGridSettingHeader>, IQuestionGridSettingHeaderRepository
    {

        public QuestionGridSettingHeaderRepository(CBUSADbContext Context)
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
