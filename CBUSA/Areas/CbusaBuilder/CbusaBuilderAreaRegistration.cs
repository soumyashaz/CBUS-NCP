using System.Web.Mvc;

namespace CBUSA.Areas.CbusaBuilder
{
    public class CbusaBuilderAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CbusaBuilder";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
               "CbusaBuilder_ BuilderReportAddProject",
               "CbusaBuilder/Project/AddProject/{ContractId}/{status}",
               new { action = "AddProject", controller = "Project", ContractId = UrlParameter.Optional, status = UrlParameter.Optional }
           );
            context.MapRoute(
              "CbusaBuilder_ BuilderReportView",
              "CbusaBuilder/BuilderReport/BuilderReportView/{ContractId}/{QuaterId}",
              new { action = "BuilderReportView", controller = "BuilderReport", ContractId = UrlParameter.Optional, QuaterId = UrlParameter.Optional }
          );

            context.MapRoute(
              "CbusaBuilder_ BuilderReport",
              "CbusaBuilder/BuilderReport/SubmitReport/{ContractId}",
              new { action = "SubmitReport", controller = "BuilderReport", ContractId = UrlParameter.Optional }
          );

            context.MapRoute(
               "CbusaBuilder_Login",
               "CbusaBuilder/Account/Login/{UserId}/{Flag}",
               new { action = "Login", controller = "Account", UserId = UrlParameter.Optional, Flag= UrlParameter.Optional }
           );

            context.MapRoute(
               "CbusaBuilder_Home",
               "CbusaBuilder/Account/Home/{BuilderId}",
               new { action = "Home", controller = "Account", BuilderId = UrlParameter.Optional }
           );
            context.MapRoute(
             "CbusaBuilder_AddProjectStatus",
             "CbusaBuilder/Builder/AddProjectStatus/{ContractId}",
             new { action = "AddProjectStatus", controller = "Builder", ContractId = UrlParameter.Optional }
         );
            context.MapRoute(
           "CbusaBuilder_reporthistory",
           "CbusaBuilder/Builder/ReportHistory/{ContractId}",
           new { action = "ReportHistory", controller = "Builder", ContractId = UrlParameter.Optional }
       );

            context.MapRoute(
                "CbusaBuilder_default",
                "CbusaBuilder/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}