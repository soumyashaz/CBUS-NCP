using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using CBUSA.Repository;
using CBUSA.Services;
using CBUSA.Services.Interface;
using CBUSA.Services.Model;

namespace CBUSA.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            // container.RegisterType<CBUSADbContext>(new);
            // conta
            container.RegisterType<CBUSADbContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            //container.RegisterType<IStudentRepository, StudentRepository>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            //container.RegisterType<IStudentServices, StudentServices>();
            container.RegisterType<IContractServices, ContractServices>();
            //  container.RegisterType<IStatusServices, StatusServices>();
            container.RegisterType<ICategoryServices, CategoryServices>();
            container.RegisterType<IResourceServices, ResourceServices>();
            container.RegisterType<IResourceMarketServices, ResourceMarketServices>();
            container.RegisterType<IBuilderService, BuilderService>();
            container.RegisterType<IProductCategoryService, ProductCategoryService>();
            container.RegisterType<IContractStatusService, ContractStatusService>();
            container.RegisterType<IResourceCategoryService, ResourceCategoryService>();
            container.RegisterType<IZoneService, ZoneService>();
            container.RegisterType<IResourceService, ResourceService>();
            container.RegisterType<IRandom, RandomNo>();
            container.RegisterType<IManufacturerService, ManufacturerService>();
            container.RegisterType<IContractRebateService, ContractRebateService>();
            container.RegisterType<IContractBuilderService, ContractBuilderService>();
            container.RegisterType<ISurveyService, SurveyService>();
            container.RegisterType<IQuestionService, QuestionService>();
            container.RegisterType<ISurveyBuilderEmailSentService, SurveyBuilderEmailSentService>();
            container.RegisterType<IContractComplianceService, ContractComplianceService>();
            container.RegisterType<ISurveyBuilderService, SurveyBuilderService>();
            container.RegisterType<IStateService, StateService>();
            container.RegisterType<ICityService, CityService>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IProjectStatusService, ProjectStatusService>();
            container.RegisterType<IQuaterService,QuaterService>();
            container.RegisterType<IBuilderQuaterAdminReportService, BuilderQuaterAdminReportService>();
            container.RegisterType<IBuilderQuarterContractStatusService, BuilderQuarterContractStatusService>();
            container.RegisterType<IBuilderQuaterContractProjectReportService, BuilderQuaterContractProjectReportService>();
            container.RegisterType<IBuilderQuaterContractProjectDetailsService, BuilderQuaterContractProjectDetailsService>();
            container.RegisterType<ISurveyResponseService, SurveyResponseService>();
            container.RegisterType<IMarketService, MarketService>();
            container.RegisterType<IConstructFormulaService, ConstructFormulaService>();
            container.RegisterType<INonResponderReportService, NonResponderReportService>();
            container.RegisterType<IHolidayService, HolidayService>();
            container.RegisterType<IQuarterReminderService, QuarterReminderService>();
            container.RegisterType<IQuarterEmailTemplateService, QuarterEmailTemplateService>();
            container.RegisterType<IChallengeQuestionServices, ChallengeQuestionServices>();
            container.RegisterType<IEulaServices, EulaServices>();
            container.RegisterType<IPasswordLogServices, PasswordLogServices>();
            container.RegisterType<IUserInRoleServices, UserInRoleServices>();
            container.RegisterType<IRoleServices, RoleServices>();
            container.RegisterType<IAdminDashboardService, AdminDashboardService>();
            container.RegisterType<IContractCentralService, ContractCentralService>();
        }
    }
}
