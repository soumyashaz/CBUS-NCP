using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    public class UserChallangeQuestionRepository : Repository<UserChallangeQuestion>, IUserChallangeQuestionRepository
    {
        public UserChallangeQuestionRepository(CBUSADbContext Context)
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

        public bool IsAnswareCorrect(int UserId, Dictionary<int, string> QuestionAnswerList)
        {
            bool ReturnValue = false;
            var ObjUserQuestionAnswerList = Context.DbsUserChallangeQuestion.Where(x => x.Id == UserId);
            string Answer = string.Empty;
            foreach (UserChallangeQuestion Obj in ObjUserQuestionAnswerList)
            {
                if (QuestionAnswerList.TryGetValue(Obj.ChallengeQuestionId, out Answer))
                {
                    if (Obj.Answer.ToLower() == Answer.ToLower())
                    {
                        ReturnValue = true;
                        break;
                    }
                }
            }

            return ReturnValue;
        }
        
    }
}
