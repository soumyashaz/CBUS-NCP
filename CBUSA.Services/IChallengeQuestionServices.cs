using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public interface IChallengeQuestionServices
    {
        ChallengeQuestion FindChallengeQuestion(Int64 Id);
        IEnumerable<ChallengeQuestion> GetChallengeQuestion();
        void EditChallengeQuestion(ChallengeQuestion ObjChallengeQuestion);
        void SaveChallengeQuestion(ChallengeQuestion ObjChallengeQuestion);
        bool IsAnswareCorrect(int UserId, Dictionary<int, string> QuestionAnswerList);
        bool SaveUserChallangeQuestion(List<UserChallangeQuestion> ChallangeQuestionLis);
        bool EditUserChallangeQuestion(List<UserChallangeQuestion> ChallangeQuestionLis);        
        IEnumerable<UserChallangeQuestion> GetUserChallengeQuestion(int UserId);
        IEnumerable<dynamic> GetChallengeQuestionDetail();
    }
}
