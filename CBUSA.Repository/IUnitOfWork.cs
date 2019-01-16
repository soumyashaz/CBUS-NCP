using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Repository.Interface;
namespace CBUSA.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        bool Flag { get; set; }
        //IStudentRepository Students { get; }
        IContractRepository Contracts { get; }

        ICategoryRepository Categories { get; }
        //   IResourceRepository Resources { get; }
        //  IResourceMarketRepository ResourceMarkets { get; }


        //Rabi
        IBuilderRepository Builder { get; }
        IBuilderUserRepository BuilderUser { get; }
        IProductCategoryRepository ProductCategory { get; }
        IProductRepository Product { get; }
        IContractStatusRepository ContractStatus { get; }

        IResourceCategoryRepository ResourceCategory { get; }
        IZoneRepository Zone { get; }

        IResourceRepository Resource { get; }
        IResourceMarketRepository ResourceMarket { get; }

        IManufacturerReposiory Manufacturer { get; }

        IContractProductRepository ContractProduct { get; }

        IContractMarketRepository ContractMarket { get; }
        IContractStatusHistoryRepository ContractStatusHistory { get; }
        IContractRebateRepository ContractRebate { get; }
        IContractRebateBuilderRepository ContractRebateBuilder { get; }

        IContractBuilderRepository ContractBuilder { get; }

        IMarketRepository Market { get; }

        ISurveyRepository Survey { get; }

        ISurveyBuilderRepository SurveyBuilder { get; }

        IQuestionTypeRepository QuestionType { get; }
        IQuestionRepository Question { get; }

        ITextBoxTypeRepository TextBoxType { get; }

        IQuestionTextBoxSettingRepository QuestionTextBoxSetting { get; }
        IQuestionDropdownSettingRepository QuestionDropdownSetting { get; }
        IQuestionGridSettingRepository QuestionGridSetting { get; }
        IQuestionGridSettingHeaderRepository QuestionGridSettingHeader { get; }
        ISurveyMarketRepository SurveyMarket { get; }
        ISurveyEmailSettingRepository SurveyEmailSetting { get; }
        ISurveyInviteEmailSettingRepository SurveyInviteEmailSetting { get; }
        ISurveyRemainderEmailSettingRepository SurveyRemainderEmailSetting { get; }
        ISurveySaveContinueEmailSettingRepository SurveySaveContinueEmailSetting { get; }

        ISurveyBuilderEmailSentRepository BuilderEmailSent { get; }
        ISurveyBuilderUserEmailSentRepository BuilderUserEmailSent { get; }
        IContractComplianceRepository ContractCompliance { get; }
        IContractComplianceBuilderRepository ContractComplianceBuilder { get; }
        ISurveyResultRepository SurveyResult { get; }

        IQuaterRepository Quater { get; }

        ISurveyBuilderEmailSendDetailsRepository SurveyBuilderEmailSendDetails { get; }
        IStateRepository State { get; }
        ICityRepository City { get; }
        IProjectRepository Project { get; }
        IProjectStatusRepository ProjectStatus { get; }
        IBuilderQuaterAdminReportRepository BuilderQuaterAdminReport { get; }
        IBuilderQuaterContractProjectReportRepository BuilderQuaterContractProjectReport { get; }
        IBuilderQuaterContractProjectDetailsRepository BuilderQuaterContractProjectDetails { get; }
        IConstructFormulaRepository ConstructFormula { get; }

        IConstructFormulaMarketRepository ConstructFormulaMarket { get; }

        INonResponderReportRepository NonResponderReport { get; }

        IHolidayRepository Holiday { get; }

        IQuarterReminderRepository QuarterReminder { get; }

        IQuarterEmailTemplateRepository QuarterEmailTemplate { get; }
        // Builder Integration
        //IQBVendorDataReceivedRepository QBVendorDataReceived { get; }
        //IQBBillDataReceivedRepository QBBillDataReceived { get; }
        //IQBCategoryDataReceivedRepository QBCategoryDataReceived { get; }
        //IQBInvoiceDataReceivedRepository QBInvoiceDataReceived { get; }
        //IVendorMappingRepository VendorMapping { get; }
        //IVendorRepository Vendor { get; }
        //IBuilderVendorRemovedRepository BuilderVendorRemoved { get; }
        //ICategoryMappingRepository CategoryMapping { get; }
        //IBuilderCategoryRemovedRepository BuilderCategoryRemoved { get; }
        //ISubmitReportRepository SubmitReport { get; }

        //ISurveyResponseHistoryRepository SurveyResponseHistory { get; }
        //ISurveyResponseEditStatusRepository SurveyResponseEditStatus { get; }
        int Complete();
        IChallengeQuestionRepository ChallengeQuestions { get; }
        IUserChallangeQuestionRepository UserChallangeQuestion { get; }
        IEulaRepository Eulas { get; }
        IPasswordLogRepository PasswordLogs { get; }
        IRoleRepository Role { get; }
        IUserInRoleRepository UserInRole { get; }
        IBuilderQuarterContractStatusRepository BuilderQuarterContractStatus { get;}
        IProjectReportStatusRepository ProjectReportStatus { get; }
        IQuarterContractStatusRepository QuarterContractStatus { get; }


        //******************************* CONTRACT CENTRAL ********************************
        ILuAppContentSectionRepository LuAppContentSection { get; }
        IContentRepository Content { get; }
        IImageRepository Image { get; }
        IAttachmentRepository Attachment { get; }
        ILuAppTagRepository LuAppTag { get; }
        IContentAttachmentRepository ContentAttachment { get; }
        IContentImageRepository ContentImage { get; }
        IContentMarketRepository ContentMarket { get; }
        IAttachmentTagRepository AttachmentTag { get; }
        IImageTagRepository ImageTag { get; }
        //**********************************************************************************
    }
}
