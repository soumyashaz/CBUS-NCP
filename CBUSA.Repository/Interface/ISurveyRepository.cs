using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Repository.Interface
{
    public interface ISurveyRepository : IRepository<Survey>
    {
        //List<Int64> GetBuilderAssocaitedSurvey(Int64 SurvayId);
        int GetCompleteedBuilderNCP(Int64 SurveyId);
        int GetInCompleteedBuilderNCP(Int64 SurveyId);

        IEnumerable<Builder> BuilderAllreadyGetInvitationForSurvey(Int64 SurveyId,bool IsEnrollmentSurvey);

    }
}
