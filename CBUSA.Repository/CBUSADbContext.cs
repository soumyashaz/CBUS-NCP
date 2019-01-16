using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public class CBUSADbContext : DbContext
    {
        public CBUSADbContext()
            : base("name=CBUSA")
        {
            //Database.SetInitializer<CBUSADbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder ModelBuilder)
        {
            ModelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            ModelBuilder.Entity<Role>().ToTable("AspNetRoles");
            ModelBuilder.Entity<UserInRole>().ToTable("AspNetUserRoles");
            ModelBuilder.Entity<User>().ToTable("AspNetUsers");
            ModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //ModelBuilder.Entity<SurveyResponse>().ma

        }

        //NCP report
        public DbSet<BuilderQuaterAdminReport> DbBuilderQuaterAdminReport { get; set; }
        public DbSet<BuilderQuaterContractProjectDetails> DbBuilderQuaterContractProjectDetails { get; set; }
        public DbSet<BuilderQuaterContractProjectReport> DbBuilderQuaterContractProjectReport { get; set; }
        public DbSet<BuilderQuarterContractStatus> DbBuilderQuarterContractStatus { get; set; }
        public DbSet<QuarterContractStatus> DbQuarterContractStatus { get; set; }
        public DbSet<ProjectReportStatus> DbProjectReportStatus { get; set; }
        

        public DbSet<Project> DbProject { get; set; }
        //Survey

        
        public DbSet<SurveyResult> DbSurveyResult { get; set; }
        public DbSet<SurveyMarket> DbSurveyMarket { get; set; }
        public DbSet<SurveyBuilder> DbSurveyBuilder { get; set; }
        public DbSet<QuestionGridSettingHeader> DbQuestionGridSettingHeader { get; set; }
        public DbSet<QuestionGridSetting> DbQuestionGridSetting { get; set; }
        public DbSet<QuestionDropdownSetting> DbQuestionDropdownSetting { get; set; }
        public DbSet<QuestionTextBoxSetting> DbQuestionTextBoxSetting { get; set; }
        public DbSet<Survey> DbSurvey { get; set; }
        public DbSet<TextBoxType> DbTextBoxType { get; set; }
        public DbSet<Question> DbQuestion { get; set; }
        public DbSet<QuestionType> DbQuestionType { get; set; }
        //

        public DbSet<ContractStatusHistory> DbContractStatusHistory { get; set; }
        public DbSet<ContractProduct> DbContractProduct { get; set; }
        public DbSet<ContractMarket> DbContractMarket { get; set; }
        public DbSet<ContractBuilder> DbContractBuilder { get; set; }
        public DbSet<Contract> DbContract { get; set; }
        public DbSet<Product> DbProduct { get; set; }
        public DbSet<ProductCategory> DbProductCategory { get; set; }
        public DbSet<ResourceMarket> DbResourceMarket { get; set; }
        public DbSet<Resource> DbResource { get; set; }
        public DbSet<ResourceCategory> DbResourceCategory { get; set; }
        public DbSet<ContractStatus> DbContractStatus { get; set; }
        public DbSet<Manufacturer> DbManufacturer { get; set; }
        public DbSet<Builder> DbBuilder { get; set; }

        public DbSet<BuilderUser> DbBuilderUser { get; set; }
        public DbSet<Market> DbMarket { get; set; }
        public DbSet<Zone> DbZone { get; set; }
        public DbSet<State> DbState { get; set; }
        public DbSet<City> DbCity { get; set; }
        public DbSet<RowStatus> DbRowStatus { get; set; }
        public DbSet<Quater> DbQuater { get; set; }
        public DbSet<ContractRebate> DbContractRebate { get; set; }
        public DbSet<ContractRebateBuilder> DbContractRebateBuilder { get; set; }
        public DbSet<SurveyEmailSetting> DbSurveyEmailSetting { get; set; }
        public DbSet<SurveyInviteEmailSetting> DbSurveyInviteEmailSetting { get; set; }
        public DbSet<SurveyRemainderEmailSetting> DbSurveyRemainderEmailSetting { get; set; }
        public DbSet<SurveySaveContinueEmailSetting> DbSurveySaveContinueEmailSetting { get; set; }
        public DbSet<BuilderSurveyEmailSent> DbBuilderSurveyEmailSent { get; set; }

        public DbSet<BuilderUserSurveyEmailSent> DbBuilderUserSurveyEmailSent { get; set; }

        public DbSet<ContractCompliance> DbContractCompliance { get; set; }
        public DbSet<ContractComplianceBuilder> DbContractComplianceBuilder { get; set; }
        public DbSet<SurveyBuilderEmailSendDetails> DbSurveyBuilderEmailSendDetails { get; set; }
        public DbSet<ConstructFormula> DbConstructFormula { get; set; }
        public DbSet<ConstructFormulaMarket> DbConstructFormulaMarket { get; set; }

        //public DbSet<SurveyResponseHistory> DbSurveyResponseHistory { get; set; }

        // Builder Integration
        public DbSet<QBVendorDataReceived> DBQBVendorDataReceived { get; set; }
        public DbSet<QBCategoryDataReceived> DBQBCategoryDataReceived { get; set; }
        public DbSet<QBBillDataReceived> DBQBBillDataReceived { get; set; }

        public DbSet<Holiday> DbHoliday { get; set; }

        public DbSet<QuarterReminder> DbQuarterReminder { get; set; }

        public DbSet<QuarterEmailTemplate> DbQuarterEmailTemplate { get; set; }
        //public DbSet<SurveyResponseEditStatus> DbSurveyResponseEditStatus { get; set; }

        //public DbSet<QBInvoiceDataReceived> DBQBInvoiceDataReceived { get; set; }

        //public DbSet<VendorMapping> VendorMapping { get; set; }
        //public DbSet<Vendor> Vendor { get; set; }
        //public DbSet<BuilderVendorRemoved> BuilderVendorRemoved { get; set; }
        //public DbSet<CategoryMapping> CategoryMapping { get; set; }
        //public DbSet<BuilderCategoryRemoved> BuilderCategoryRemoved { get; set; }
        //public DbSet<SubmitReport> SubmitReport { get; set; }

        public DbSet<ChallengeQuestion> DbsChallengeQuestion { get; set; }
        public DbSet<UserChallangeQuestion> DbsUserChallangeQuestion { get; set; }
        public DbSet<Eula> DbsEula { get; set; }
        public DbSet<Role> DbsRole { get; set; }
        public DbSet<UserInRole> DbsUserInRole { get; set; }

        //******************* CONTRACT CENTRAL *********************
        public DbSet<Lu_App_ContentSection> DbLuAppContentSection { get; set; }
        public DbSet<Content> DbContent { get; set; }
        public DbSet<Image> DbImage { get; set; }
        public DbSet<Attachment> DbAttachment { get; set; }
        public DbSet<Lu_App_Tag> DbLuAppTag { get; set; }
        public DbSet<ContentAttachment> DbContentAttachment { get; set; }
        public DbSet<ContentImage> DbContentImage { get; set; }
        public DbSet<ContentMarket> DbContentMarket { get; set; }
        public DbSet<AttachmentTag> DbAttachmentTag { get; set; }
        public DbSet<ImageTag> DbImageTag { get; set; }
        //***********************************************************
    }
}
