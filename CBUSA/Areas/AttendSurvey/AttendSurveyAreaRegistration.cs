using System.Web.Mvc;

namespace CBUSA.Areas.AttendSurvey
{
    public class AttendSurveyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AttendSurvey";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "AttendSurvey_ThankYou",

                "AttendSurvey/Survey/ThankYou/{SurveyId}/{BuilderId}/{IsSurveyCompleted}",
                new { controller = "Survey", action = "ThankYou", SurveyId = UrlParameter.Optional, BuilderId = UrlParameter.Optional, IsSurveyCompleted = UrlParameter.Optional }
            );

            context.MapRoute(
                "AttendSurvey_TakeSurvey",
                "AttendSurvey/Survey/TakeSurvey/{SurveyId}/{BuilderId}",
                new { controller = "Survey", action = "TakeSurvey", SurveyId = UrlParameter.Optional, BuilderId = UrlParameter.Optional }
            );
            context.MapRoute(
                "AttendSurvey_default",
                "AttendSurvey/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}