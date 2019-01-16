using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;
using System.IO;

namespace CBUSA.Services.Model
{
    public class SurveyService : ISurveyService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        private readonly CBUSADbContext DbContext = new CBUSADbContext();
        public SurveyService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public void Flag(bool flag)
        {
            _ObjUnitWork.Flag = flag;
        }
        public IEnumerable<Survey> GetSurveyAll()
        {
            return _ObjUnitWork.Survey.Search(x => x.IsNcpSurvey == false && x.RowStatusId == (int)RowActiveStatus.Active);

            // return list;
        }
        public IEnumerable<Survey> GetSurveyAllActiveAndClose()
        {
            return _ObjUnitWork.Survey.Search(x => x.IsNcpSurvey == false && x.RowStatusId != (int)RowActiveStatus.Archived);

            // return list;
        }

        public Survey GetSurvey(Int64 SurveyId)
        {
            return _ObjUnitWork.Survey.Get(SurveyId);
        }


        public bool IsSurveyExist(Int64 SurveyId)
        {
            return _ObjUnitWork.Survey.Find(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active).Count() > 0;
        }
        public void SaveSurvey(Survey ObjSurvey)
        {
            _ObjUnitWork.Survey.Add(ObjSurvey);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void UpdateSurvey(Survey ObjSurvey)
        {
            _ObjUnitWork.Survey.Update(ObjSurvey);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public void DeleteSurvey(Survey ObjSurvey)
        {
            _ObjUnitWork.Survey.Update(ObjSurvey);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public IEnumerable<Survey> FindContractSurveys(Int64 ContractId)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId
                && x.RowStatusId == (int)RowActiveStatus.Active && x.IsEnrolment == true);
        }

        public void SaveSurveyMarket(List<Int64> MarketList, Int64 SurveyId)
        {

            foreach (var Item in MarketList)
            {
                _ObjUnitWork.SurveyMarket.Add(new SurveyMarket { MarketId = Item, SurveyId = SurveyId });

            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public void RemoveSurveyMarket(Int64 MarketId, Int64 SurveyId)
        {
            SurveyMarket ObjSurveyMarket = _ObjUnitWork.SurveyMarket.Find(x => x.SurveyId == SurveyId && x.MarketId == MarketId).FirstOrDefault();

            if (ObjSurveyMarket != null)
            {
                _ObjUnitWork.SurveyMarket.Remove(ObjSurveyMarket);
            }


            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void SaveSurveyEmailSetting(SurveyEmailSetting ObjSurveyEmailSetting, SurveyInviteEmailSetting ObjSurveyInviteEmailSetting,
            SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting, SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting
            )
        {
            _ObjUnitWork.SurveyEmailSetting.Add(ObjSurveyEmailSetting);
            if (ObjSurveyInviteEmailSetting != null)
                _ObjUnitWork.SurveyInviteEmailSetting.Update(ObjSurveyInviteEmailSetting);
            if (ObjSurveyRemainderEmailSetting != null)
                _ObjUnitWork.SurveyRemainderEmailSetting.Update(ObjSurveyRemainderEmailSetting);
            if (ObjSurveySaveContinueEmailSetting != null)
                _ObjUnitWork.SurveySaveContinueEmailSetting.Update(ObjSurveySaveContinueEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void EditSurveyEmailSetting(SurveyEmailSetting ObjSurveyEmailSetting, SurveyInviteEmailSetting ObjSurveyInviteEmailSetting,
            SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting, SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting
            )
        {
            _ObjUnitWork.SurveyEmailSetting.Update(ObjSurveyEmailSetting);
            if (ObjSurveyInviteEmailSetting != null)
                _ObjUnitWork.SurveyInviteEmailSetting.Update(ObjSurveyInviteEmailSetting);
            if (ObjSurveyRemainderEmailSetting != null)
                _ObjUnitWork.SurveyRemainderEmailSetting.Update(ObjSurveyRemainderEmailSetting);
            if (ObjSurveySaveContinueEmailSetting != null)
                _ObjUnitWork.SurveySaveContinueEmailSetting.Update(ObjSurveySaveContinueEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public SurveyEmailSetting GetSurveyEmailSetting(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyEmailSetting.Search(x => x.SurveyId == SurveyId).FirstOrDefault();
        }


        public void SaveSurveyInviteEmailSetting(SurveyInviteEmailSetting ObjSurveyInviteEmailSetting)
        {
            _ObjUnitWork.SurveyInviteEmailSetting.Add(ObjSurveyInviteEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void UpdateSurveyInviteEmailSetting(SurveyInviteEmailSetting ObjSurveyInviteEmailSetting)
        {
            _ObjUnitWork.SurveyInviteEmailSetting.Update(ObjSurveyInviteEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public SurveyInviteEmailSetting GetSurveyInviteEmailSetting(string DumpId)
        {
            return _ObjUnitWork.SurveyInviteEmailSetting.Search(x => x.DumpId == DumpId).FirstOrDefault();

        }
        public SurveyInviteEmailSetting GetSurveyInviteEmailSettingBySurveySetting(Int64 SurveySettingId)
        {
            return _ObjUnitWork.SurveyInviteEmailSetting.Search(x => x.SurveyEmailSettingId == SurveySettingId).FirstOrDefault();

        }

        public void SaveSurveyRemainderEmailSetting(SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting)
        {
            _ObjUnitWork.SurveyRemainderEmailSetting.Add(ObjSurveyRemainderEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void UpdateSurveyRemainderEmailSetting(SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting)
        {
            _ObjUnitWork.SurveyRemainderEmailSetting.Update(ObjSurveyRemainderEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public SurveyRemainderEmailSetting GetSurveyRemainderEmailSetting(string DumpId)
        {
            return _ObjUnitWork.SurveyRemainderEmailSetting.Search(x => x.DumpId == DumpId).FirstOrDefault();

        }


        public void SaveSurveySaveContinueEmailSetting(SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting)
        {
            _ObjUnitWork.SurveySaveContinueEmailSetting.Add(ObjSurveySaveContinueEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void UpdateSurveySaveContinueEmailSetting(SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting)
        {
            _ObjUnitWork.SurveySaveContinueEmailSetting.Update(ObjSurveySaveContinueEmailSetting);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public SurveySaveContinueEmailSetting GetSurveySaveContinueEmailSetting(string DumpId)
        {
            return _ObjUnitWork.SurveySaveContinueEmailSetting.Search(x => x.DumpId == DumpId).FirstOrDefault();

        }



        public IEnumerable<SurveyBuilder> GetSurveyBuilder(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.IsSurveyCompleted == true);
            //return _ObjUnitWork.Survey.Find(x => x.ContractId == ContractId
            //    && x.RowStatusId == (int)RowActiveStatus.Active && x.IsEnrolment == true);
        }


        public IEnumerable<Survey> GetContractSurvey(Int64 ContractId)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active).OrderByDescending(x => x.SurveyId);
        }
        public IEnumerable<Survey> GetContractEnrolledSurvey(Int64 ContractId)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsEnrolment == true).OrderByDescending(x => x.SurveyId);
        }

        public bool CopySurvey(Int64 ContractId, Int64 SurveyId, bool? IsNcp)
        {

            Survey _ObjSurvey = _ObjUnitWork.Survey.Search(x => x.SurveyId == SurveyId && x.ContractId == ContractId).FirstOrDefault();
            if (_ObjSurvey != null)
            {

                Survey ObjDbObject = new Survey
                {
                    SurveyName = _ObjSurvey.SurveyName + "_Copy",
                    Label = _ObjSurvey.Label,
                    ContractId = _ObjSurvey.ContractId,
                    // StartDate = DateTime.Now.AddDays(1),
                    // EndDate = DateTime.Now.AddDays(30),
                    IsNcpSurvey = IsNcp.HasValue ? true : false,
                    IsPublished = false,
                    IsEnrolment = false,
                    RowStatusId = (int)RowActiveStatus.Active,
                    RowGUID = Guid.NewGuid(),
                    CreatedBy = 1,
                    ModifiedBy = 1,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                _ObjUnitWork.Survey.Add(ObjDbObject);
                _ObjUnitWork.Complete();
                List<Question> _ObjQuestion = _ObjSurvey.Question.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).ToList();
                //foreach (var Item in _ObjQuestion)
                //{

                //    _ObjUnitWork.Complete();
                //}





                foreach (var Item in _ObjQuestion)
                {

                    Question Obj = new Question
                    {
                        QuestionValue = Item.QuestionValue,
                        IsMandatory = Item.IsMandatory,
                        IsFileNeedtoUpload = Item.IsFileNeedtoUpload,
                        SurveyOrder = Item.SurveyOrder,
                        SurveyId = ObjDbObject.SurveyId,
                        QuestionTypeId = Item.QuestionTypeId
                    };
                    _ObjUnitWork.Question.Add(Obj);

                    if (Item.QuestionTextBoxSetting.Count > 0)
                    {

                        _ObjUnitWork.QuestionTextBoxSetting.Add(new QuestionTextBoxSetting
                        {
                            IsAlphabets = Item.QuestionTextBoxSetting.FirstOrDefault().IsAlphabets,
                            IsNumber = Item.QuestionTextBoxSetting.FirstOrDefault().IsNumber,
                            IsSpecialCharecter = Item.QuestionTextBoxSetting.FirstOrDefault().IsSpecialCharecter,
                            LowerLimit = Item.QuestionTextBoxSetting.FirstOrDefault().LowerLimit,
                            UpperLimit = Item.QuestionTextBoxSetting.FirstOrDefault().UpperLimit,
                            TextBoxTypeId = Item.QuestionTextBoxSetting.FirstOrDefault().TextBoxTypeId,
                            QuestionId = Obj.QuestionId,


                        });
                    }
                    if (Item.QuestionDropdownSetting.Count > 0)
                    {
                        foreach (var Item1 in Item.QuestionDropdownSetting)
                        {
                            _ObjUnitWork.QuestionDropdownSetting.Add(new QuestionDropdownSetting
                            {
                                Value = Item1.Value,
                                // QuestionId = Item.QuestionDropdownSetting.FirstOrDefault().QuestionId
                            });
                        }
                    }

                    if (Item.QuestionGridSetting.Count > 0)
                    {
                        _ObjUnitWork.QuestionGridSetting.Add(new QuestionGridSetting
                        {
                            Row = Item.QuestionGridSetting.FirstOrDefault().Row,
                            Column = Item.QuestionGridSetting.FirstOrDefault().Column,
                            // QuestionId = Item.QuestionGridSetting.FirstOrDefault().QuestionId

                        });
                        List<QuestionGridSettingHeader> _ObjQuestionGridSettingHeader = Item.QuestionGridSetting.FirstOrDefault().QuestionGridSettingHeader.ToList();
                        foreach (var ItemGridHeader in _ObjQuestionGridSettingHeader)
                        {

                            _ObjUnitWork.QuestionGridSettingHeader.Add(
                                new QuestionGridSettingHeader
                                {
                                    RowHeaderValue = ItemGridHeader.RowHeaderValue,
                                    ColoumnHeaderValue = ItemGridHeader.ColoumnHeaderValue,
                                    IndexNumber = ItemGridHeader.IndexNumber,
                                    //  QuestionGridSettingId = ItemGridHeader.QuestionGridSettingId,
                                    ControlType = ItemGridHeader.ControlType,
                                    DropdownTypeOptionValue = ItemGridHeader.DropdownTypeOptionValue
                                }

                                );
                        }



                        // _ObjQuestionGridSettingHeader = ;

                    }
                    _ObjUnitWork.Complete();
                }

                if (_ObjSurvey.SurveyMarket.Count > 0)
                {
                    foreach (var Item in _ObjSurvey.SurveyMarket.ToList())
                    {
                        _ObjUnitWork.SurveyMarket.Add(new SurveyMarket { MarketId = Item.MarketId, SurveyId = ObjDbObject.SurveyId });
                    }


                }

                if (_ObjSurvey.SurveyEmailSetting.Count > 0)
                {
                    // _ObjUnitWork.SurveyEmailSetting.AddRange(_ObjSurvey.SurveyEmailSetting);

                    SurveyEmailSetting ObjCopySettings = new SurveyEmailSetting();

                    ObjCopySettings.SenderEmail = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SenderEmail;
                    ObjCopySettings.RemainderForTakeSurveySecond = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().RemainderForTakeSurveySecond;
                    ObjCopySettings.RemainderForTakeSurveyThird = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().RemainderForTakeSurveyThird;
                    ObjCopySettings.DayBeforeSurveyEndSecond = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().DayBeforeSurveyEndSecond;
                    ObjCopySettings.DayBeforeSurveyEndThird = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().DayBeforeSurveyEndThird;
                    ObjCopySettings.RemainderForTakeSurvey = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().RemainderForTakeSurvey;
                    ObjCopySettings.DayBeforeSurveyEnd = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().DayBeforeSurveyEnd;
                    ObjCopySettings.RemainderForContinueSurvey = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().RemainderForContinueSurvey;
                    ObjCopySettings.DayAfterSurveyEnd = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().DayAfterSurveyEnd;
                    ObjCopySettings.SurveyId = ObjDbObject.SurveyId;

                    _ObjUnitWork.SurveyEmailSetting.Add(ObjCopySettings);

                    if (_ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyInviteEmailSetting.Count > 0)
                    {
                        //  _ObjUnitWork.SurveyInviteEmailSetting.AddRange(_ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyInviteEmailSetting);

                        _ObjUnitWork.SurveyInviteEmailSetting.Add(new SurveyInviteEmailSetting
                        {
                            Subject = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyInviteEmailSetting.FirstOrDefault().Subject,
                            EmailContent = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyInviteEmailSetting.FirstOrDefault().EmailContent,
                            SurveyEmailSettingId = ObjCopySettings.SurveyEmailSettingId,
                            DumpId = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyInviteEmailSetting.FirstOrDefault().DumpId,


                        });
                    }

                    if (_ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyRemainderEmailSetting.Count > 0)
                    {
                        //  _ObjUnitWork.SurveyRemainderEmailSetting.AddRange(_ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyRemainderEmailSetting);
                        _ObjUnitWork.SurveyRemainderEmailSetting.Add(new SurveyRemainderEmailSetting
                        {
                            Subject = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyRemainderEmailSetting.FirstOrDefault().Subject,
                            EmailContent = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyRemainderEmailSetting.FirstOrDefault().EmailContent,
                            SurveyEmailSettingId = ObjCopySettings.SurveyEmailSettingId,
                            DumpId = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyRemainderEmailSetting.FirstOrDefault().DumpId,


                        });

                    }

                    if (_ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveySaveContinueEmailSetting.Count > 0)
                    {
                        _ObjUnitWork.SurveySaveContinueEmailSetting.Add(new SurveySaveContinueEmailSetting
                        {
                            Subject = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveySaveContinueEmailSetting.FirstOrDefault().Subject,
                            EmailContent = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveySaveContinueEmailSetting.FirstOrDefault().EmailContent,
                            SurveyEmailSettingId = ObjCopySettings.SurveyEmailSettingId,
                            DumpId = _ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveySaveContinueEmailSetting.FirstOrDefault().DumpId,


                        });
                    }

                }
                _ObjUnitWork.Complete();
                _ObjUnitWork.Dispose();
                return true;
            }
            return false;
            // return _ObjUnitWork.Survey.Find(x => x.ContractId == ContractId);
        }


        public SurveyEmailSetting GetSurveyEmail(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyEmailSetting.Get(SurveyId);
        }

        public bool IsEnrollmentAvailable(Int64 ContractId)
        {
            Int64 ContractCurrentStatus = _ObjUnitWork.Contracts.Get(ContractId).ContractStatusId;

            if (ContractCurrentStatus == (Int64)ContractActiveStatus.Active)
            {
                return !(_ObjUnitWork.Survey.Find(x => x.ContractCurrentStatus ==
                    (Int64)ContractActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active && x.IsEnrolment == true
                    && x.ContractId == ContractId && x.IsNcpSurvey == false).Count() > 0);
            }
            else
            {
                return !(_ObjUnitWork.Survey.Find(x => x.ContractCurrentStatus !=
                   (Int64)ContractActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active && x.IsEnrolment == true
                   && x.ContractId == ContractId && x.IsNcpSurvey == false).Count() > 0);
            }
        }

        public IEnumerable<Survey> FindContractSurveysAll(Int64 ContractId)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.IsNcpSurvey == false
                && x.RowStatusId != (int)RowActiveStatus.Archived);
        }
        public IEnumerable<Survey> GetSurveyAllArchive()
        {
            return _ObjUnitWork.Survey.Search(x => x.IsNcpSurvey == false && x.RowStatusId == (int)RowActiveStatus.Archived);

            // return list;
        }
        public IEnumerable<Survey> FindContractSurveysAllArchive(Int64 ContractId)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.IsNcpSurvey == false
                && x.RowStatusId == (int)RowActiveStatus.Archived);
        }


        #region  Ncp Survey
        #region Prasenjit
        public IEnumerable<Survey> GetNcpSurveyAllArchive()
        {
            return _ObjUnitWork.Survey.Search(x => x.IsNcpSurvey == true && x.RowStatusId == (int)RowActiveStatus.Archived);

            // return list;
        }
        public IEnumerable<Survey> FindContractNcpSurveysAllArchive(Int64 ContractId)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.IsNcpSurvey == true
                && x.RowStatusId == (int)RowActiveStatus.Archived);
        }
        #endregion

        #region Rabi
        public IEnumerable<Survey> GetNcpSurveyAll()
        {
            return _ObjUnitWork.Survey.Search(x => x.IsNcpSurvey == true && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<Survey> GetNcpSurveyAllActiveAndClose()
        {
            return _ObjUnitWork.Survey.Search(x => x.IsNcpSurvey == true && x.RowStatusId != (int)RowActiveStatus.Archived);

        }
        public IEnumerable<Survey> FindContractNcpSurveysAll(Int64 ContractId)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.IsNcpSurvey == true
               && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        public string[] GetQuaterAll()
        {
            return _ObjUnitWork.Quater.GetAll().Select(x => x.QuaterName).Distinct().ToArray();
        }
        public long[] GetYearAll()
        {
            return _ObjUnitWork.Quater.GetAll().Select(x => x.Year).Distinct().ToArray();
        }

        public bool[] IsNcpSurvey(Int64 SurveyId)
        {
            // return _ObjUnitWork.Survey.Find(x => x.SurveyId == SurveyId && x.IsNcpSurvey == true).Count() > 0;

            var Survey = _ObjUnitWork.Survey.Search(x => x.SurveyId == SurveyId).FirstOrDefault();
            if (Survey != null)
            {
                return new bool[] { true, Survey.IsNcpSurvey };
            }
            else
            {
                return new bool[] { false };
            }

        }

        public bool IsNcpSurveyAvailable(string Quater, string Year, Int64 ContractId)
        {
            return !_ObjUnitWork.Survey.Search(x => x.IsNcpSurvey == true && x.Year == Year && x.Quater == Quater && x.RowStatusId
                == (Int32)RowActiveStatus.Active && x.ContractId == ContractId).Any();
        }



        #endregion


        #endregion


        public IEnumerable<SurveyResult> GetBuilderSurveyResult(Int64 SurveyId, Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        public void SaveSurveyResult(Int64 SurveyId, Int64 BuilderId, bool IsSurveyComplete, List<SurveyResult> ObjSurveyResult, string ServerFilePath)
        {


            var ObjSurveyBuilder = _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId).FirstOrDefault();
            if (ObjSurveyBuilder != null)
            {
                ObjSurveyBuilder.IsSurveyCompleted = IsSurveyComplete;
                ObjSurveyBuilder.ModifiedOn = DateTime.Now;
                _ObjUnitWork.SurveyBuilder.Update(ObjSurveyBuilder);
            }
            else
            {
                SurveyBuilder Obj = new SurveyBuilder
                {
                    BuilderId = BuilderId,
                    SurveyId = SurveyId,
                    SurveyStartDate = DateTime.Now,
                    SurveyCompleteDate = DateTime.Now,
                    IsSurveyCompleted = IsSurveyComplete,
                    RowStatusId = (int)RowActiveStatus.Active,
                    CreatedBy = 1,
                    ModifiedBy = 1,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    RowGUID = Guid.NewGuid()
                };

                _ObjUnitWork.SurveyBuilder.Add(Obj);
            }



            foreach (var Item in ObjSurveyResult)
            {
                string Answare = Item.Answer != null ? Item.Answer : "";
                switch (Item.SurveyId) //1 for TextBox Type /// We use Survey Idto send data of question type from front end
                {

                    case 1:
                        var Result = _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.QuestionId == Item.QuestionId).FirstOrDefault();
                        if (Result != null) //for update
                        {
                            Result.Answer = Answare;
                            Result.ModifiedBy = 1;
                            Result.ModifiedOn = DateTime.Now;

                            if (Item.FileName != null)
                            {
                                if (Result.FileName != null)
                                {
                                    var physicalPath = Path.Combine(ServerFilePath, Result.FileName);
                                    if (System.IO.File.Exists(physicalPath))
                                    {
                                        System.IO.File.Delete(physicalPath);
                                    }
                                }
                                Result.FileName = Item.FileName;
                            }
                            _ObjUnitWork.SurveyResult.Update(Result);
                        }
                        else
                        {
                            SurveyResult ObjResult = new SurveyResult
                            {
                                Answer = Answare,
                                RowNumber = Item.RowNumber,
                                ColumnNumber = Item.ColumnNumber,
                                QuestionId = Item.QuestionId,
                                SurveyId = SurveyId,
                                BuilderId = BuilderId,
                                RowStatusId = (int)RowActiveStatus.Active,
                                CreatedBy = 1,
                                ModifiedBy = 1,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                RowGUID = Guid.NewGuid(),
                                FileName = Item.FileName != null ? Item.FileName : null
                            };
                            _ObjUnitWork.SurveyResult.Add(ObjResult);
                        }

                        break;
                    case 2:

                        var Result2 = _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.QuestionId == Item.QuestionId).FirstOrDefault();
                        if (Result2 != null) //for update
                        {
                            Result2.Answer = Answare;
                            Result2.ModifiedBy = 1;
                            Result2.ModifiedOn = DateTime.Now;

                            if (Item.FileName != null)
                            {
                                if (Result2.FileName != null)
                                {
                                    var physicalPath = Path.Combine(ServerFilePath, Result2.FileName);
                                    if (System.IO.File.Exists(physicalPath))
                                    {
                                        System.IO.File.Delete(physicalPath);
                                    }
                                }
                                Result2.FileName = Item.FileName;
                            }


                            _ObjUnitWork.SurveyResult.Update(Result2);
                        }
                        else
                        {
                            SurveyResult ObjResult = new SurveyResult
                            {
                                Answer = Answare,
                                RowNumber = Item.RowNumber,
                                ColumnNumber = Item.ColumnNumber,
                                QuestionId = Item.QuestionId,
                                SurveyId = SurveyId,
                                BuilderId = BuilderId,
                                RowStatusId = (int)RowActiveStatus.Active,
                                CreatedBy = 1,
                                ModifiedBy = 1,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                RowGUID = Guid.NewGuid(),
                                FileName = Item.FileName != null ? Item.FileName : null
                            };
                            _ObjUnitWork.SurveyResult.Add(ObjResult);

                        }

                        break;
                    case 3:

                        var Result3 = _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId
                            && x.QuestionId == Item.QuestionId && x.RowNumber == Item.RowNumber && x.ColumnNumber == Item.ColumnNumber).FirstOrDefault();
                        if (Result3 != null) //for update
                        {
                            Result3.Answer = Answare;
                            Result3.ModifiedBy = 1;
                            Result3.ModifiedOn = DateTime.Now;
                            _ObjUnitWork.SurveyResult.Update(Result3);
                        }
                        else
                        {
                            SurveyResult ObjResult = new SurveyResult
                            {
                                Answer = Answare,
                                RowNumber = Item.RowNumber,
                                ColumnNumber = Item.ColumnNumber,
                                QuestionId = Item.QuestionId,
                                SurveyId = SurveyId,
                                BuilderId = BuilderId,
                                RowStatusId = (int)RowActiveStatus.Active,
                                CreatedBy = 1,
                                ModifiedBy = 1,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                RowGUID = Guid.NewGuid()

                            };
                            _ObjUnitWork.SurveyResult.Add(ObjResult);

                        }

                        break;

                }

            }


            if (IsSurveyComplete)
            {

                var Survey = _ObjUnitWork.Survey.Search(x => x.SurveyId == SurveyId && x.IsEnrolment == true && x.IsNcpSurvey == false && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                if (Survey != null)
                {
                    var QuestionCount = Survey.Question.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Count();
                    var LastQuestion = _ObjUnitWork.Question.GetQuestionWithIndexNo(Survey.SurveyId, QuestionCount);
                    if (LastQuestion.QuestionTypeId == (int)EnumQuestionType.DropList)
                    {
                        string Answare = ObjSurveyResult.Where(x => x.QuestionId == LastQuestion.QuestionId).FirstOrDefault().Answer;

                        if (Answare.ToLower().Trim() == "yes")
                        {

                            var Contract = _ObjUnitWork.Contracts.Search(x => x.ContractId == Survey.ContractId
                                && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                            var IsBuilderAllreadyRegister = _ObjUnitWork.ContractBuilder.Search(x => x.ContractId == Contract.ContractId && x.BuilderId == BuilderId
                             //   && x.RowStatusId == (int)RowActiveStatus.Active
                                ).Any();

                            if (!IsBuilderAllreadyRegister)
                            {
                                if (Contract != null)  /// Builder Is Associate with contract
                                {
                                    ContractBuilder Obj = new ContractBuilder
                                    {
                                        BuilderId = BuilderId,
                                        ContractId = Contract.ContractId,
                                        ContractStatusId = Contract.ContractStatusId,
                                        JoiningDate = DateTime.Now,
                                        CreatedBy = 1,
                                        CreatedOn = DateTime.Now,
                                        ModifiedBy = 1,
                                        ModifiedOn = DateTime.Now,
                                        RowStatusId = (int)RowActiveStatus.Active,
                                        RowGUID = Guid.NewGuid()


                                    };
                                    _ObjUnitWork.ContractBuilder.Add(Obj);

                                }
                            }
                        }
                        else
                        {

                            var Contract = _ObjUnitWork.Contracts.Search(x => x.ContractId == Survey.ContractId
                                && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                            var IsBuilderAllreadyArchived = _ObjUnitWork.ContractBuilder.Search(x => x.ContractId == Contract.ContractId && x.BuilderId == BuilderId
                              //  && x.RowStatusId == (int)RowActiveStatus.Archived
                                ).Any();

                            if (!IsBuilderAllreadyArchived)
                            {
                                if (Contract != null)  /// Builder Is Associate with contract
                                {
                                    ContractBuilder Obj = new ContractBuilder
                                    {
                                        BuilderId = BuilderId,
                                        ContractId = Contract.ContractId,
                                        ContractStatusId = Contract.ContractStatusId,
                                        JoiningDate = DateTime.Now,
                                        CreatedBy = 1,
                                        CreatedOn = DateTime.Now,
                                        ModifiedBy = 1,
                                        ModifiedOn = DateTime.Now,
                                        RowStatusId = (int)RowActiveStatus.Archived,
                                        RowGUID = Guid.NewGuid()


                                    };
                                    _ObjUnitWork.ContractBuilder.Add(Obj);

                                }
                            }

                        }
                    }

                }
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public IEnumerable<SurveyResult> GetQuestionWiseBuilderSurveyResult(Int64 SurveyId, Int64 BuilderId, Int64 QuestionId)
        {
            return _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.QuestionId == QuestionId
                && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        //public IEnumerable<dynamic> GetSurveyResponse(Int64 SurveyId)
        //{
        //    return _ObjUnitWork.SurveyResult.GetSurveyResponse(SurveyId);
        //}

        //public IEnumerable<dynamic> GetSurveyResponse(Int64 SurveyId, Int64 BuilderId)
        //{
        //    return _ObjUnitWork.SurveyResult.GetSurveyResponse(SurveyId, BuilderId);
        //}

        public bool IsBuilderAuthorizedToAcessSurvey(Int64 SurveyId, Int64 BuilderId)
        {
            return _ObjUnitWork.BuilderEmailSent.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.IsMailSent == true).Any();
        }

        public bool IsBuilderAllraedyCompleteSurvey(Int64 SurveyId, Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSurveyCompleted == true).Any();
        }

        public Survey GetEditResponseSurvey(Int64 SurveyId)
        {
            return _ObjUnitWork.Survey.SearchWithInclude(x => x.SurveyId == SurveyId, y => y.Question).FirstOrDefault();
        }


        public void EditSurveyResultByAdmin(Int64 SurveyId, Int64 BuilderId, List<SurveyResult> ObjSurveyResult, string ServerFilePath)
        {


            var ObjSurveyBuilder = _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId).FirstOrDefault();
            //if (ObjSurveyBuilder != null)
            //{
            //    ObjSurveyBuilder.IsSurveyCompleted = IsSurveyComplete;
            //    ObjSurveyBuilder.ModifiedOn = DateTime.Now;
            //    _ObjUnitWork.SurveyBuilder.Update(ObjSurveyBuilder);
            //}
            //else
            //{
            //    SurveyBuilder Obj = new SurveyBuilder
            //    {
            //        BuilderId = BuilderId,
            //        SurveyId = SurveyId,
            //        SurveyStartDate = DateTime.Now,
            //        SurveyCompleteDate = DateTime.Now,
            //        IsSurveyCompleted = IsSurveyComplete,
            //        RowStatusId = (int)RowActiveStatus.Active,
            //        CreatedBy = 1,
            //        ModifiedBy = 1,
            //        CreatedOn = DateTime.Now,
            //        ModifiedOn = DateTime.Now,
            //        RowGUID = Guid.NewGuid()
            //    };

            //    _ObjUnitWork.SurveyBuilder.Add(Obj);
            //}



            foreach (var Item in ObjSurveyResult)
            {
                string Answare = Item.Answer != null ? Item.Answer : "";
                switch (Item.SurveyId) //1 for TextBox Type /// We use Survey Idto send data of question type from front end
                {

                    case 1:
                        var Result = _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.QuestionId == Item.QuestionId).FirstOrDefault();
                        if (Result != null) //for update
                        {
                            Result.Answer = Answare;
                            Result.ModifiedBy = 1;
                            Result.ModifiedOn = DateTime.Now;

                            if (Item.FileName != null)
                            {
                                if (Result.FileName != null)
                                {
                                    var physicalPath = Path.Combine(ServerFilePath, Result.FileName);
                                    if (System.IO.File.Exists(physicalPath))
                                    {
                                        System.IO.File.Delete(physicalPath);
                                    }
                                }
                                Result.FileName = Item.FileName;
                            }
                            _ObjUnitWork.SurveyResult.Update(Result);
                        }
                        else
                        {
                            SurveyResult ObjResult = new SurveyResult
                            {
                                Answer = Answare,
                                RowNumber = Item.RowNumber,
                                ColumnNumber = Item.ColumnNumber,
                                QuestionId = Item.QuestionId,
                                SurveyId = SurveyId,
                                BuilderId = BuilderId,
                                RowStatusId = (int)RowActiveStatus.Active,
                                CreatedBy = 1,
                                ModifiedBy = 1,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                RowGUID = Guid.NewGuid(),
                                FileName = Item.FileName != null ? Item.FileName : null
                            };
                            _ObjUnitWork.SurveyResult.Add(ObjResult);
                        }

                        break;
                    case 2:

                        var Result2 = _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.QuestionId == Item.QuestionId).FirstOrDefault();
                        if (Result2 != null) //for update
                        {
                            Result2.Answer = Answare;
                            Result2.ModifiedBy = 1;
                            Result2.ModifiedOn = DateTime.Now;

                            if (Item.FileName != null)
                            {
                                if (Result2.FileName != null)
                                {
                                    var physicalPath = Path.Combine(ServerFilePath, Result2.FileName);
                                    if (System.IO.File.Exists(physicalPath))
                                    {
                                        System.IO.File.Delete(physicalPath);
                                    }
                                }
                                Result2.FileName = Item.FileName;
                            }


                            _ObjUnitWork.SurveyResult.Update(Result2);
                        }
                        else
                        {
                            SurveyResult ObjResult = new SurveyResult
                            {
                                Answer = Answare,
                                RowNumber = Item.RowNumber,
                                ColumnNumber = Item.ColumnNumber,
                                QuestionId = Item.QuestionId,
                                SurveyId = SurveyId,
                                BuilderId = BuilderId,
                                RowStatusId = (int)RowActiveStatus.Active,
                                CreatedBy = 1,
                                ModifiedBy = 1,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                RowGUID = Guid.NewGuid(),
                                FileName = Item.FileName != null ? Item.FileName : null
                            };
                            _ObjUnitWork.SurveyResult.Add(ObjResult);

                        }

                        break;
                    case 3:

                        var Result3 = _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId
                            && x.QuestionId == Item.QuestionId && x.RowNumber == Item.RowNumber && x.ColumnNumber == Item.ColumnNumber).FirstOrDefault();
                        if (Result3 != null) //for update
                        {
                            Result3.Answer = Answare;
                            Result3.ModifiedBy = 1;
                            Result3.ModifiedOn = DateTime.Now;
                            _ObjUnitWork.SurveyResult.Update(Result3);
                        }
                        else
                        {
                            SurveyResult ObjResult = new SurveyResult
                            {
                                Answer = Answare,
                                RowNumber = Item.RowNumber,
                                ColumnNumber = Item.ColumnNumber,
                                QuestionId = Item.QuestionId,
                                SurveyId = SurveyId,
                                BuilderId = BuilderId,
                                RowStatusId = (int)RowActiveStatus.Active,
                                CreatedBy = 1,
                                ModifiedBy = 1,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                RowGUID = Guid.NewGuid()

                            };
                            _ObjUnitWork.SurveyResult.Add(ObjResult);

                        }

                        break;

                }

            }


            if (ObjSurveyBuilder.IsSurveyCompleted)
            {

                var Survey = _ObjUnitWork.Survey.Search(x => x.SurveyId == SurveyId && x.IsEnrolment == true && x.IsNcpSurvey == false && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                if (Survey != null)
                {
                    var QuestionCount = Survey.Question.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Count();
                    var LastQuestion = _ObjUnitWork.Question.GetQuestionWithIndexNo(Survey.SurveyId, QuestionCount);
                    if (LastQuestion.QuestionTypeId == (int)EnumQuestionType.DropList)
                    {
                        string Answare = ObjSurveyResult.Where(x => x.QuestionId == LastQuestion.QuestionId).FirstOrDefault().Answer;

                        if (Answare.ToLower().Trim() == "yes")
                        {

                            var Contract = _ObjUnitWork.Contracts.Search(x => x.ContractId == Survey.ContractId
                                && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                            var IsBuilderAllreadyRegister = _ObjUnitWork.ContractBuilder.Search(x => x.ContractId == Contract.ContractId && x.BuilderId == BuilderId
                               // && x.RowStatusId == (int)RowActiveStatus.Active
                                ).Any();

                            if (!IsBuilderAllreadyRegister)
                            {
                                if (Contract != null)  /// Builder Is Associate with contract
                                {
                                    ContractBuilder Obj = new ContractBuilder
                                    {
                                        BuilderId = BuilderId,
                                        ContractId = Contract.ContractId,
                                        ContractStatusId = Contract.ContractStatusId,
                                        JoiningDate = DateTime.Now,
                                        CreatedBy = 1,
                                        CreatedOn = DateTime.Now,
                                        ModifiedBy = 1,
                                        ModifiedOn = DateTime.Now,
                                        RowStatusId = (int)RowActiveStatus.Active,
                                        RowGUID = Guid.NewGuid()


                                    };
                                    _ObjUnitWork.ContractBuilder.Add(Obj);

                                }
                            }
                            else
                            {
                                var ObjContractBuilder = _ObjUnitWork.ContractBuilder.Search(x => x.ContractId == Contract.ContractId && x.BuilderId == BuilderId
                                // && x.RowStatusId == (int)RowActiveStatus.Active
                                ).FirstOrDefault();

                                ObjContractBuilder.RowStatusId= (int)RowActiveStatus.Active;
                                ObjContractBuilder.ModifiedOn = DateTime.Now;

                                _ObjUnitWork.ContractBuilder.Update(ObjContractBuilder);
                            }
                        }
                        else
                        {

                            var Contract = _ObjUnitWork.Contracts.Search(x => x.ContractId == Survey.ContractId
                                && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                            var IsBuilderAllreadyArchived = _ObjUnitWork.ContractBuilder.Search(x => x.ContractId == Contract.ContractId && x.BuilderId == BuilderId
                              //  && x.RowStatusId == (int)RowActiveStatus.Archived
                                ).Any();

                            if (!IsBuilderAllreadyArchived)
                            {
                                if (Contract != null)  /// Builder Is Associate with contract
                                {
                                    ContractBuilder Obj = new ContractBuilder
                                    {
                                        BuilderId = BuilderId,
                                        ContractId = Contract.ContractId,
                                        ContractStatusId = Contract.ContractStatusId,
                                        JoiningDate = DateTime.Now,
                                        CreatedBy = 1,
                                        CreatedOn = DateTime.Now,
                                        ModifiedBy = 1,
                                        ModifiedOn = DateTime.Now,
                                        RowStatusId = (int)RowActiveStatus.Archived,
                                        RowGUID = Guid.NewGuid()


                                    };
                                    _ObjUnitWork.ContractBuilder.Add(Obj);
                                }
                            }
                            else
                            {
                                var ObjContractBuilder = _ObjUnitWork.ContractBuilder.Search(x => x.ContractId == Contract.ContractId && x.BuilderId == BuilderId
                             //    && x.RowStatusId == (int)RowActiveStatus.Active
                                 )
                                 .FirstOrDefault();

                                ObjContractBuilder.RowStatusId = (int)RowActiveStatus.Archived;
                                ObjContractBuilder.ModifiedOn = DateTime.Now;

                                _ObjUnitWork.ContractBuilder.Update(ObjContractBuilder);
                            }

                        }
                    }

                }
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public IEnumerable<SurveyMarket> GetSurveyMarket(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyMarket.Search(x => x.SurveyId == SurveyId);
        }
        public int GetCompleteedBuilderNCP(Int64 SurveyId)
        {
            return _ObjUnitWork.Survey.GetCompleteedBuilderNCP(SurveyId);
        }
        public int GetInCompleteedBuilderNCP(Int64 SurveyId)
        {
            return _ObjUnitWork.Survey.GetInCompleteedBuilderNCP(SurveyId);
        }

        public IEnumerable<Builder> BuilderAllreadyGetInvitationForSurvey(Int64 SurveyId,bool IsEnrollmentSurvey)
        {
            return _ObjUnitWork.Survey.BuilderAllreadyGetInvitationForSurvey(SurveyId, IsEnrollmentSurvey);
        }

        //public string[] GetQuaterYearAll()
        //{
        //    return _ObjUnitWork.Quater.GetAll().Select(x => x.QuaterName + " - " + x.Year).Distinct().ToArray();
        //}
        public IEnumerable<Survey> FindContractNcpSurveysAllByQuarter(Int64 ContractId, string Quarter, string Year)
        {
            return _ObjUnitWork.Survey.Search(x => x.ContractId == ContractId && x.IsNcpSurvey == true
                && x.Quater == Quarter && x.Year == Year);
        }
        //public IEnumerable<SurveyResponseEditStatus> GetNCPSurveyResponseEditStatus(Int64 BuilderId, Int64 QuaterId, Int64? ContractId)
        //{
        //    if (ContractId.HasValue)
        //    {
        //        return _ObjUnitWork.SurveyResponseEditStatus.Search(z => z.BuilderId == BuilderId && z.QuaterId == QuaterId && z.RowStatusId == (int) RowActiveStatus.Active
        //        && z.ContractId == ContractId.GetValueOrDefault());
        //    }
        //    else
        //    {
        //        return _ObjUnitWork.SurveyResponseEditStatus.Search(z => z.BuilderId == BuilderId && z.QuaterId == QuaterId && z.RowStatusId == (int) RowActiveStatus.Active);
        //    }
            
        //}
        //public bool GetNCPSurveyResponseEditPermission(Int64 BuilderId, Int64 ContractId, Int64 QuarterId)
        //{
        //    return _ObjUnitWork.SurveyResponseEditStatus.Search(x => x.BuilderId == BuilderId & x.ContractId == ContractId && x.QuaterId == QuarterId).Select(y => y.IsEditable).SingleOrDefault();
        //}
        //public string SaveNCPSurveyResponseEditPermission(SurveyResponseEditStatus ObjModel)
        //{
        //    string Message = "";
        //    if (ObjModel.BuilderId >0 && ObjModel.ContractId >0)
        //    {
        //        var CheckStatusExists = _ObjUnitWork.SurveyResponseEditStatus.Find(x => x.BuilderId == ObjModel.BuilderId && x.ContractId == ObjModel.ContractId && x.QuaterId == ObjModel.QuaterId).Select(y=> y.BuilderId).SingleOrDefault();
        //        if(CheckStatusExists == 0 )
        //        {
        //            ObjModel.CreatedBy = 1;
        //            ObjModel.ModifiedBy = 1;
        //            ObjModel.CreatedOn = DateTime.Now;
        //            ObjModel.ModifiedOn = DateTime.Now;
        //            ObjModel.RowGUID = Guid.NewGuid();
        //            _ObjUnitWork.SurveyResponseEditStatus.Add(ObjModel);
        //            _ObjUnitWork.Complete();
        //            _ObjUnitWork.Dispose();
        //            Message = "Successfully re-opened survey response for edit.";
        //        }
        //        else
        //        {
        //            // update as false 
        //            var CheckStatus = _ObjUnitWork.SurveyResponseEditStatus.Find(x => x.BuilderId == ObjModel.BuilderId && x.ContractId == ObjModel.ContractId && x.QuaterId == ObjModel.QuaterId);
        //            foreach (var Item in CheckStatus)
        //            {
        //                ObjModel.SurveyResponseEditStatusId = Item.SurveyResponseEditStatusId;
        //                ObjModel.QuaterId = Item.QuaterId;
        //                ObjModel.CreatedBy = Item.CreatedBy;
        //                ObjModel.ModifiedBy = Item.ModifiedBy;
        //                ObjModel.CreatedOn = Item.CreatedOn;
        //                ObjModel.RowGUID = Item.RowGUID;
        //                ObjModel.RowStatusId = Item.RowStatusId;
        //                ObjModel.RowStatus = Item.RowStatus;                        
        //            }
        //            try
        //            {                        
        //                DbContext.DbSurveyResponseEditStatus.Attach(ObjModel);
        //                DbContext.Entry(ObjModel).State = System.Data.Entity.EntityState.Modified;
        //                ObjModel.ModifiedOn = System.DateTime.Now;
        //                ObjModel.ModifiedBy = 1;
        //                DbContext.SaveChanges();
        //                Message = "Successfully changed edit access of ncp survey response.";
        //            }
        //            catch(Exception ee)
        //            {
        //                Message = ee.Message.ToString();
        //            }
        //        }                
        //    }
        //    return Message;
        //}
    }
}
