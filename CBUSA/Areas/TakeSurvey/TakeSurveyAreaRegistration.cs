using System.Web.Mvc;

namespace CBUSA.Areas.TakeSurvey
{
    public class TakeSurveyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TakeSurvey";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TakeSurvey_EnrollmentSurvey",
                "TakeSurvey/{SurveyParticipation}/{EnrolmentSurvey}/{SurveyId}/{BuilderId}",
                new { controller = "SurveyParticipation", action = "EnrolmentSurvey", SurveyId = UrlParameter.Optional, BuilderId = UrlParameter.Optional }
            );

            
            context.MapRoute(
                "TakeSurvey_default",
                "TakeSurvey/{controller}/{action}/{SurveyId}/{BuilderId}",
                new { action = "Index", SurveyId = UrlParameter.Optional, BuilderId = UrlParameter.Optional }
            );
        }
    }
}