using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
  public  class SurveyEmailSettingRepository : Repository<SurveyEmailSetting>, ISurveyEmailSettingRepository
    {

        public SurveyEmailSettingRepository(CBUSADbContext Context)
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

  public class SurveyInviteEmailSettingRepository : Repository<SurveyInviteEmailSetting>, ISurveyInviteEmailSettingRepository
  {

      public SurveyInviteEmailSettingRepository(CBUSADbContext Context)
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

  public class SurveyRemainderEmailSettingRepository : Repository<SurveyRemainderEmailSetting>, ISurveyRemainderEmailSettingRepository
  {

      public SurveyRemainderEmailSettingRepository(CBUSADbContext Context)
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

  public class SurveySaveContinueEmailSettingRepository : Repository<SurveySaveContinueEmailSetting>, ISurveySaveContinueEmailSettingRepository
  {

      public SurveySaveContinueEmailSettingRepository(CBUSADbContext Context)
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
