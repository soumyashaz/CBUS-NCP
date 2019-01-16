using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Model
{
   public class SurveyBuilderRepository : Repository<SurveyBuilder>, ISurveyBuilderRepository
    {
       public SurveyBuilderRepository(CBUSADbContext Context)
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
