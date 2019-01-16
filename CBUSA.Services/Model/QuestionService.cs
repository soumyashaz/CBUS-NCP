using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public QuestionService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }


        public IEnumerable<QuestionType> GetQuestionTypeAll()
        {
            return _ObjUnitWork.QuestionType.GetAll();
        }


        public IEnumerable<TextBoxType> GetTextBoxTypeAll()
        {
            return _ObjUnitWork.TextBoxType.GetAll();
        }


        public IEnumerable<Question> GetQuestionAll()
        {
            throw new NotImplementedException();
        }

        public Question GetQuestion(long QuestionId)
        {
            return _ObjUnitWork.Question.Search(x => x.QuestionId == QuestionId && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
        }

        public Question GetQuestion(Int64 QuestionId, Int64 SurveyID)
        {
            return _ObjUnitWork.Question.Search(x => x.QuestionId == QuestionId && x.RowStatusId ==
                (int)RowActiveStatus.Active && x.SurveyId == SurveyID).FirstOrDefault();
        }

        public void SaveQuestion(Question ObjQuestion)
        {
            throw new NotImplementedException();
        }




        public bool IsQuestionLableUniueInContract(string LableName, string DumpId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Question> GetQuestionofDump(string DumpId)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestion(Question ObjQuestion)
        {
            throw new NotImplementedException();
        }






        public void SaveTextBoxQuestion(Question ObjQuestion, QuestionTextBoxSetting ObjQuestionTextBoxSetting)
        {
            ObjQuestion.SurveyOrder = _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
                RowActiveStatus.Active).Count() == 0 ? 1 : _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
                RowActiveStatus.Active).Max(x => x.SurveyOrder) + 1;
            _ObjUnitWork.Question.Add(ObjQuestion);
            _ObjUnitWork.QuestionTextBoxSetting.Add(ObjQuestionTextBoxSetting);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();

        }

        public void SaveDropListQuestion(Question ObjQuestion, List<QuestionDropdownSetting> ObjQuestionDropdownSetting)
        {
            ObjQuestion.SurveyOrder = _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
               RowActiveStatus.Active).Count() == 0 ? 1 : _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
               RowActiveStatus.Active).Max(x => x.SurveyOrder) + 1;

            _ObjUnitWork.Question.Add(ObjQuestion);
            _ObjUnitWork.QuestionDropdownSetting.AddRange(ObjQuestionDropdownSetting);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void SaveGridQuestion(Question ObjQuestion, QuestionGridSetting ObjQuestionGridSetting, List<QuestionGridSettingHeader> ObjQuestionGridSettingHeader)
        {
            ObjQuestion.SurveyOrder = _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
               RowActiveStatus.Active).Count() == 0 ? 1 : _ObjUnitWork.Question.Find(x => x.SurveyId == ObjQuestion.SurveyId && x.RowStatusId == (int)
               RowActiveStatus.Active).Max(x => x.SurveyOrder) + 1;
            _ObjUnitWork.Question.Add(ObjQuestion);
            _ObjUnitWork.QuestionGridSetting.Add(ObjQuestionGridSetting);
            _ObjUnitWork.QuestionGridSettingHeader.AddRange(ObjQuestionGridSettingHeader);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public void EditTextBoxQuestion(Question ObjQuestion, QuestionTextBoxSetting ObjQuestionTextBoxSetting)
        {




            _ObjUnitWork.Question.Update(ObjQuestion);
            _ObjUnitWork.QuestionTextBoxSetting.Add(ObjQuestionTextBoxSetting);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();

        }

        public void EditDropListQuestion(Question ObjQuestion, List<QuestionDropdownSetting> ObjQuestionDropdownSetting)
        {
            _ObjUnitWork.Question.Update(ObjQuestion);
            _ObjUnitWork.QuestionDropdownSetting.AddRange(ObjQuestionDropdownSetting);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void EditGridQuestion(Question ObjQuestion, QuestionGridSetting ObjQuestionGridSetting, List<QuestionGridSettingHeader> ObjQuestionGridSettingHeader)
        {
            _ObjUnitWork.Question.Update(ObjQuestion);
            _ObjUnitWork.QuestionGridSetting.Add(ObjQuestionGridSetting);
            _ObjUnitWork.QuestionGridSettingHeader.AddRange(ObjQuestionGridSettingHeader);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteTextDropDownGridSettingData(Question _ObjQuestion
            )
        {
            if (_ObjQuestion.QuestionTextBoxSetting.FirstOrDefault() != null)
            {
                _ObjUnitWork.QuestionTextBoxSetting.Remove(_ObjQuestion.QuestionTextBoxSetting.FirstOrDefault());
            }



            if (_ObjQuestion.QuestionDropdownSetting.Count > 0)
            {
                foreach (var Item in _ObjQuestion.QuestionDropdownSetting.ToList())
                {
                    //QuestionDropdownSetting _Obj = Item;
                    _ObjUnitWork.QuestionDropdownSetting.Remove(Item);
                }
            }

            if (_ObjQuestion.QuestionGridSetting.FirstOrDefault() != null)
            {
                if (_ObjQuestion.QuestionGridSetting.FirstOrDefault().QuestionGridSettingHeader.Count > 0)
                {
                    foreach (var Item in _ObjQuestion.QuestionGridSetting.FirstOrDefault().QuestionGridSettingHeader.ToList())
                    {
                        _ObjUnitWork.QuestionGridSettingHeader.Remove(Item);
                    }
                }

            }
            if (_ObjQuestion.QuestionGridSetting.FirstOrDefault() != null)
            {
                _ObjUnitWork.QuestionGridSetting.Remove(_ObjQuestion.QuestionGridSetting.FirstOrDefault());
            }
            // _ObjUnitWork.Complete();
            // _ObjUnitWork.Dispose();
        }



        public void DeleteQuestion(Question ObjQuestion)
        {




            _ObjUnitWork.Question.Update(ObjQuestion);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();


        }

        public void EditQuestion(Question ObjQuestion)
        {
            _ObjUnitWork.Question.Update(ObjQuestion);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void OrderingUpQuestion(Int64 SurveyId, Int64 QuestionId)
        {
            Question ObjQuestion = _ObjUnitWork.Question.Search(x => x.QuestionId == QuestionId && x.SurveyId == SurveyId).FirstOrDefault();

            if (ObjQuestion != null)
            {
                int UpOrder = ObjQuestion.SurveyOrder - 1;
                Question Up = _ObjUnitWork.Question.Search(x => x.SurveyId == SurveyId && x.SurveyOrder == UpOrder).FirstOrDefault();

                Up.SurveyOrder = ObjQuestion.SurveyOrder;
                ObjQuestion.SurveyOrder = UpOrder;


                _ObjUnitWork.Question.Update(ObjQuestion);
                _ObjUnitWork.Question.Update(Up);
                _ObjUnitWork.Complete();
                _ObjUnitWork.Dispose();
            }


        }
        public void OrderingDownQuestion(Int64 SurveyId, Int64 QuestionId)
        {
            Question ObjQuestion = _ObjUnitWork.Question.Search(x => x.QuestionId == QuestionId && x.SurveyId == SurveyId).FirstOrDefault();

            if (ObjQuestion != null)
            {
                int DownOrder = ObjQuestion.SurveyOrder + 1;
                Question Down = _ObjUnitWork.Question.Search(x => x.SurveyId == SurveyId && x.SurveyOrder == DownOrder).FirstOrDefault();

                Down.SurveyOrder = ObjQuestion.SurveyOrder;
                ObjQuestion.SurveyOrder = DownOrder;


                _ObjUnitWork.Question.Update(ObjQuestion);
                _ObjUnitWork.Question.Update(Down);
                _ObjUnitWork.Complete();
                _ObjUnitWork.Dispose();
            }
        }

        public IEnumerable<Question> GetSurveyQuestionAll(Int64 SurveyId)
        {
            return _ObjUnitWork.Question.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active, q => q.OrderBy(d => d.SurveyOrder));
        }
        public IEnumerable<Question> GetSurveyNumericQuestionAll(Int64 SurveyId)
        {
            return _ObjUnitWork.Question.GetNumericQuestion(SurveyId);
        }

        public Question GetQuestionWithIndexNo(Int64 SurveyId, int IndexNo)
        {
            return _ObjUnitWork.Question.GetQuestionWithIndexNo(SurveyId, IndexNo);
        }

        public IEnumerable<Question> GetBuilderReportQuestion(Int64 ContractId, string QuaterName, string Year)
        {
            var Survey = _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.Quater == QuaterName && x.Year == Year && x.IsNcpSurvey == true && x.IsPublished == true && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
            if (Survey != null)
            {
                return _ObjUnitWork.Question.Search(x => x.SurveyId == Survey.SurveyId && x.RowStatusId == (int)RowActiveStatus.Active, q => q.OrderBy(d => d.SurveyOrder));
            }
            return null;
        }
        // added by angshuman on 29-apr-2017
        public IEnumerable<Question> GetBuilderReportQuestionHistory(Int64 ContractId, string QuaterName, string Year)
        {
            var Survey = _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.Quater == QuaterName && x.Year == Year && x.IsNcpSurvey == true && x.IsPublished == true ).FirstOrDefault();
            if (Survey != null)
            {
                return _ObjUnitWork.Question.Search(x => x.SurveyId == Survey.SurveyId && x.RowStatusId == (int) RowActiveStatus.Active, q => q.OrderBy(d => d.SurveyOrder));
            }
            return null;
        }
        public IEnumerable<dynamic> GetQuestionValueWithGrid(Int64 SurveyId, string QuestionIdListFilter)
        {
            return _ObjUnitWork.Question.GetQuestionValueWithGrid(SurveyId, QuestionIdListFilter);
        }
        public IEnumerable<dynamic> GetQuestionByQuestionType(Int64 QuestionId, int QuestionTypeId, Int64? QuestionSettingId, bool? IsTypeOfGridQuestionHeaderSettingId)
        {
            if (QuestionTypeId == 2)
            {
                return _ObjUnitWork.QuestionDropdownSetting.Search(a => a.QuestionId == QuestionId);
            }
            else if (QuestionTypeId == 3)
            {
                return _ObjUnitWork.Question.GetGridQuestionHeader(QuestionId, QuestionSettingId.GetValueOrDefault(), IsTypeOfGridQuestionHeaderSettingId.GetValueOrDefault());
            }
            else
            {
                return null;
            }
            //return _ObjUnitWork.Question.Search(a=> a.QuestionId == QuestionId && a.QuestionTypeId == QuestionTypeId);
        }

        //public IEnumerable<dynamic> GetQuestionValueBySettingId(Int64 QuestionId, int QuestionTypeId, Int64 QuestionSettingId)
        //{
        //    if (QuestionTypeId == 2)
        //    {
        //        return _ObjUnitWork.QuestionDropdownSetting.Search(a => a.QuestionId == QuestionId && a.QuestionDropdownSettingId == QuestionSettingId);
        //    }
        //    else if (QuestionTypeId == 3)
        //    {
        //        return _ObjUnitWork.Question.GetGridQuestionHeader(QuestionId, QuestionSettingId);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //    //return _ObjUnitWork.Question.Search(a=> a.QuestionId == QuestionId && a.QuestionTypeId == QuestionTypeId);
        //}
    }
}
