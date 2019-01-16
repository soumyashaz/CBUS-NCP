using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Model
{
    class SurveyBuilderEmailSentRepository : Repository<BuilderSurveyEmailSent>, ISurveyBuilderEmailSentRepository
    {
        public SurveyBuilderEmailSentRepository(CBUSADbContext Context)
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



    class SurveyBuilderUserEmailSentRepository : Repository<BuilderUserSurveyEmailSent>, ISurveyBuilderUserEmailSentRepository
    {
        public SurveyBuilderUserEmailSentRepository(CBUSADbContext Context)
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
