using CBUSA.Areas.TakeSurvey.Models;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Areas.TakeSurvey.Controllers
{

    public class SurveyParticipationController : Controller
    {
        readonly ISurveyService _ObjSurveyService;
        public SurveyParticipationController(ISurveyService ObjSurveyService)
        {
            _ObjSurveyService = ObjSurveyService;
        }

        // GET: TakeSurvey/SurveyParticipation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EnrolmentSurvey(Int64? SurveyId, Int64? BuilderID)
        {

            //if (SurveyId.HasValue && BuilderID.HasValue)
            //{
            //    var ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());
            //    var SurveyResult = _ObjSurveyService.SurveyBuilderResult(SurveyId.GetValueOrDefault(), BuilderID.GetValueOrDefault());
            //    if (ObjSurvey != null)
            //    {
            //        TakeSurveyViewModel ObjVm = new TakeSurveyViewModel
            //        {
            //            Survey = ObjSurvey,
            //            BuilderId = BuilderID.GetValueOrDefault(),
            //            SurveyResult = SurveyResult != null ? SurveyResult : new List<SurveyResult> { }

            //        };
            //        return View(ObjVm);
            //    }
            //}
            return View();
        }
    }
}