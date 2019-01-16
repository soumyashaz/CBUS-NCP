using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public class ChallengeQuestionServices : CBUSA.Services.IChallengeQuestionServices
    {

        private readonly IUnitOfWork _ObjUnitWork;
        public ChallengeQuestionServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<ChallengeQuestion> GetChallengeQuestion()
        {
            return _ObjUnitWork.ChallengeQuestions.GetAll();
        }

        public ChallengeQuestion FindChallengeQuestion(Int64 Id)
        {
            return _ObjUnitWork.ChallengeQuestions.Get(Id);
        }

        public void EditChallengeQuestion(ChallengeQuestion ObjChallengeQuestion)
        {

            _ObjUnitWork.ChallengeQuestions.Update(ObjChallengeQuestion);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void SaveChallengeQuestion(ChallengeQuestion ObjChallengeQuestion)
        {
            _ObjUnitWork.ChallengeQuestions.Add(ObjChallengeQuestion);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public bool IsAnswareCorrect(int UserId, Dictionary<int, string> QuestionAnswerList)
        {
            return _ObjUnitWork.UserChallangeQuestion.IsAnswareCorrect(UserId, QuestionAnswerList);
        }
        public bool SaveUserChallangeQuestion(List<UserChallangeQuestion> ChallangeQuestionLis)
        {
            //  return _ObjUnitWork.UserChallangeQuestion.IsAnswareCorrect(UserId, QuestionAnswerList);
            foreach (UserChallangeQuestion ObjUserChallangeQuestion in ChallangeQuestionLis)
            {
                _ObjUnitWork.UserChallangeQuestion.Add(ObjUserChallangeQuestion);

            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();

            return true;
        }
        public bool EditUserChallangeQuestion(List<UserChallangeQuestion> ChallangeQuestionLis)
        {
            //  return _ObjUnitWork.UserChallangeQuestion.IsAnswareCorrect(UserId, QuestionAnswerList);
            foreach (UserChallangeQuestion ObjUserChallangeQuestion in ChallangeQuestionLis)
            {
                _ObjUnitWork.UserChallangeQuestion.Update(ObjUserChallangeQuestion);

            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();

            return true;
        }
        public IEnumerable<UserChallangeQuestion> GetUserChallengeQuestion(int UserId)
        {
            return _ObjUnitWork.UserChallangeQuestion.Find(x => x.Id == UserId);
        }
        public IEnumerable<dynamic> GetChallengeQuestionDetail()
        {
            return _ObjUnitWork.ChallengeQuestions.GetChallengeQuestionDetails();
        }
    }
}
