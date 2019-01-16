using System.Web.Mvc;

namespace CBUSA.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "NonResponderReport_DownloadNonResponderReport",
               "Admin/NonResponderReport/DownloadNonResponderReport/{QuarterId}",
               new { controller = "NonResponderReport", action = "DownloadNonResponderReport", QuarterId = UrlParameter.Optional }
           );
            context.MapRoute(
               "NonResponderReport_GetNonResponderList",
               "Admin/NonResponderReport/GetNonResponderList/{QuarterId}",
               new { controller = "NonResponderReport", action = "GetNonResponderList", QuarterId = UrlParameter.Optional }
           );
            context.MapRoute(
              "EditSurvey_BuilderProject",
              "Admin/SurveyResponse/EditBuilderReport/{ContractId}/{BuilderId}/{QuaterId}",
              new
              {
                  controller = "SurveyResponse",
                  action = "EditBuilderReport",
                  ContractId = UrlParameter.Optional,
                  BuilderId = UrlParameter.Optional,
                  QuaterId = UrlParameter.Optional
                  
              }
          );
            context.MapRoute(
               "EditSurvey_Response",
               "Admin/SurveyResponse/EditBuilderSurveyResponse/{SurveyId}/{BuilderId}",
               new { controller = "SurveyResponse", action = "EditBuilderSurveyResponse", SurveyId = UrlParameter.Optional, BuilderId = UrlParameter.Optional }
           );

            context.MapRoute(
                "SurveyResponse",
                "Admin/SurveyResponse/ShowResponse/{SurveyId}/{IsCompleted}/{Filter}",
                new { controller = "SurveyResponse", action = "ShowResponse", SurveyId = UrlParameter.Optional, IsCompleted = UrlParameter.Optional, Filter = UrlParameter.Optional }
            );

            context.MapRoute(
                "PublishSurvey",
                "Admin/Survey/PublishSurvey/{SurveyId}",
                new { controller = "Survey", action = "PublishSurvey", SurveyId = UrlParameter.Optional }
            );
            context.MapRoute(
                "PreviewSurvey",
                "Admin/Survey/PreviewQuestion/{SurveyId}",
                new { controller = "Survey", action = "PreviewQuestion", SurveyId = UrlParameter.Optional }
            );
            context.MapRoute(
                "SurveySettings",
                "Admin/Survey/SurveySettings/{SurveyId}",
                new { controller = "Survey", action = "SurveySettings", SurveyId = UrlParameter.Optional }
            );
            context.MapRoute(
                "SurveyConfigureInvite",
                "Admin/Survey/ConfigureInvites/{SurveyId}",
                new { controller = "Survey", action = "ConfigureInvites", SurveyId = UrlParameter.Optional }
            );
            context.MapRoute(
                 "SurveyAddQuestion",
                 "Admin/Survey/AddQuestion/{SurveyId}/{QuestionId}/{IsCopy}",
                 new { controller = "Survey", action = "AddQuestion", SurveyId = UrlParameter.Optional, QuestionId = UrlParameter.Optional, IsCopy = UrlParameter.Optional }
             );
            context.MapRoute(
                 "SurveyDetails",
                 "Admin/Survey/SurveyDetails/{SurveyId}/{IsNcpId}",
                 new { controller = "Survey", action = "SurveyDetails", SurveyId = UrlParameter.Optional, IsNcpId = UrlParameter.Optional }
             );
            context.MapRoute(
                 "ViewContract",
                 "Admin/Contract/ViewContract/{ContrcatId}",
                 new { controller = "Contract", action = "ViewContract", ContrcatId = UrlParameter.Optional }
             );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                 "CC_GetVariationList",
                 "Admin/{controller}/{action}/{ContractId}/{SectionId}",
                 new { controller = "AdminContractCentral", action = "GetVariationList", ContractId = UrlParameter.Optional, SectionId = UrlParameter.Optional }
             );
        }
    }
}