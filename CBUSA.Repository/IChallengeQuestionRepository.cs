using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public interface IChallengeQuestionRepository : IRepository<ChallengeQuestion>
    {
        IEnumerable<dynamic> GetChallengeQuestionDetails();

    }
}
