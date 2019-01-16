using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Repository.Interface;
using CBUSA.Repository.Model;
namespace CBUSA.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        bool _Flag = true;
        private readonly CBUSADbContext _Context;
        //private Lazy<IStudentRepository> _Student;
        private Lazy<IContractRepository> _Contract;

        private Lazy<ICategoryRepository> _Category;
        // private Lazy<IResourceRepository> _Resource;
        //  private Lazy<IResourceMarketRepository> _ResourceMarket;

        //Rabi
        private Lazy<IBuilderRepository> _Builder;
        private Lazy<IBuilderUserRepository> _BuilderUser;
        private Lazy<IProductCategoryRepository> _ProductCategory;
        private Lazy<IProductRepository> _Product;

        private Lazy<IContractStatusRepository> _ContractStatus;

        private Lazy<IResourceCategoryRepository> _ResourceCategory;
        private Lazy<IZoneRepository> _Zone;
        private Lazy<IResourceRepository> _Resource;
        private Lazy<IResourceMarketRepository> _ResourceMarket;
        private Lazy<IManufacturerReposiory> _Manufacturer;

        private Lazy<IContractProductRepository> _ContractProduct;
        private Lazy<IContractMarketRepository> _ContractMarket;
        private Lazy<IContractStatusHistoryRepository> _ContractStatusHistory;

        private Lazy<IContractRebateRepository> _ContractRebate;

        private Lazy<IContractRebateBuilderRepository> _ContractRebateBuilder;
        private Lazy<IContractBuilderRepository> _ContractBuilder;
        private Lazy<IMarketRepository> _Market;
        private Lazy<ISurveyRepository> _Survey;

        private Lazy<IQuestionTypeRepository> _QuestionType;
        private Lazy<IQuestionRepository> _Question;
        private Lazy<ITextBoxTypeRepository> _TextBoxType;
        private Lazy<IQuestionTextBoxSettingRepository> _QuestionTextBoxSetting;
        private Lazy<IQuestionDropdownSettingRepository> _QuestionDropdownSetting;
        private Lazy<IQuestionGridSettingRepository> _QuestionGridSetting;
        private Lazy<IQuestionGridSettingHeaderRepository> _QuestionGridSettingHeader;
        private Lazy<ISurveyMarketRepository> _SurveyMarket;

        private Lazy<ISurveyEmailSettingRepository> _SurveyEmailSetting;
        private Lazy<ISurveyInviteEmailSettingRepository> _SurveyInviteEmailSetting;
        private Lazy<ISurveyRemainderEmailSettingRepository> _SurveyRemainderEmailSetting;
        private Lazy<ISurveySaveContinueEmailSettingRepository> _SurveySaveContinueEmailSetting;


        private Lazy<ISurveyBuilderRepository> _SurveyBuilder;
        private Lazy<ISurveyBuilderEmailSentRepository> _SurveyBuilderEmailSent;
        private Lazy<ISurveyBuilderUserEmailSentRepository> _SurveyBuilderUserEmailSent;
        private Lazy<IContractComplianceRepository> _ContractCompliance;
        private Lazy<IContractComplianceBuilderRepository> _ContractComplianceBuilder;

        private Lazy<ISurveyResultRepository> _SurveyResult;
        private Lazy<IQuaterRepository> _Quater;
        private Lazy<ISurveyBuilderEmailSendDetailsRepository> _SurveyBuilderEmailSendDetails;

        private Lazy<IStateRepository> _State;
        private Lazy<ICityRepository> _City;
        private Lazy<IProjectRepository> _Project;
        private Lazy<IProjectStatusRepository> _ProjectStatus;
        private Lazy<IBuilderQuaterAdminReportRepository> _BuilderQuaterAdminReport;
        private Lazy<IBuilderQuaterContractProjectReportRepository> _BuilderQuaterContractProjectReport;
        private Lazy<IBuilderQuaterContractProjectDetailsRepository> _BuilderQuaterContractProjectDetails;
        private Lazy<IConstructFormulaRepository> _ConstructFormula;
        private Lazy<IConstructFormulaMarketRepository> _ConstructFormulaMarket;

        private Lazy<INonResponderReportRepository> _NonResponderReport;

        private Lazy<IHolidayRepository> _Holiday;

        private Lazy<IQuarterReminderRepository> _QuarterReminder;
        private Lazy<IQuarterEmailTemplateRepository> _QuarterEmailTemplate;
        //private Lazy<ISurveyResponseHistoryRepository> _SurveyResponseHistory;
        //private Lazy<ISurveyResponseEditStatusRepository> _SurveyResponseEditStatus;

        // builder integration

        //private Lazy<IQBVendorDataReceivedRepository> _QBVendorDataReceived;

        //private Lazy<IQBCategoryDataReceivedRepository> _QBCategoryDataReceived;
        //private Lazy<IQBBillDataReceivedRepository> _QBBillDataReceived;
        //private Lazy<IQBInvoiceDataReceivedRepository> _QBInvoiceDataReceived;
        //private Lazy<IVendorMappingRepository> _VendorMapping;
        //private Lazy<IVendorRepository> _Vendor;
        //private Lazy<IBuilderVendorRemovedRepository> _BuilderVendorRemoved;
        private Lazy<IBuilderQuarterContractStatusRepository> _BuilderQuarterContractStatus;
        private Lazy<IProjectReportStatusRepository> _ProjectReportStatus;
        private Lazy<IQuarterContractStatusRepository> _QuarterContractStatus;

        private Lazy<IChallengeQuestionRepository> _ChallengeQuestion;

        //Store and Retrive Eula in other Table
        private Lazy<IEulaRepository> _Eula;

        //Store Password Logs in other Table
        private Lazy<IPasswordLogRepository> _PasswordLog;

        private Lazy<IUserInRoleRepository> _UserInRole;

        private Lazy<IRoleRepository> _Role;

        private Lazy<IUserChallangeQuestionRepository> _UserChallangeQuestion;

        private Lazy<ILuAppContentSectionRepository> _LuAppContentSection;
        private Lazy<IContentRepository> _Content;
        private Lazy<IContentAttachmentRepository> _ContentAttachment;
        private Lazy<IContentMarketRepository> _ContentMarket;
        private Lazy<IContentImageRepository> _ContentImage;
        private Lazy<ILuAppTagRepository> _LuAppTag;
        private Lazy<IImageRepository> _Image;
        private Lazy<IImageTagRepository> _ImageTag;
        private Lazy<IAttachmentRepository> _Attachment;
        private Lazy<IAttachmentTagRepository> _AttachmentTag;
        
        public UnitOfWork(CBUSADbContext Context)
        {
            _Context = Context;
            _Context.Database.CommandTimeout = 500;
            //_Student = new Lazy<IStudentRepository>(() =>
            //{
            //    return new StudentRepository(_Context);
            //});



            //prasenjit adhikari

            _State = new Lazy<IStateRepository>(() =>
            {
                return new StateRepository(_Context);
            });
            _City = new Lazy<ICityRepository>(() =>
            {
                return new CityRepository(_Context);
            });
            _Project = new Lazy<IProjectRepository>(() =>
            {
                return new ProjectRepository(_Context);
            });
            _ProjectStatus = new Lazy<IProjectStatusRepository>(() =>
            {
                return new ProjectStatusRepository(_Context);
            });

            _BuilderQuaterAdminReport = new Lazy<IBuilderQuaterAdminReportRepository>(() =>
            {
                return new BuilderQuaterAdminReportRepository(_Context);
            });
            _BuilderQuaterContractProjectReport = new Lazy<IBuilderQuaterContractProjectReportRepository>(() =>
            {
                return new BuilderQuaterContractProjectReportRepository(_Context);
            });
            _Contract = new Lazy<IContractRepository>(() =>
            {
                return new ContractRepository(_Context);
            });


            _Category = new Lazy<ICategoryRepository>(() =>
            {
                return new CategoryRepository(_Context);
            });

            // end prasenjit 
            //Rabi

            _Builder = new Lazy<IBuilderRepository>(() =>
            {
                return new BuilderRepository(_Context);
            });


            _BuilderUser = new Lazy<IBuilderUserRepository>(() =>
            {
                return new BuilderUserRepository(_Context);
            });

            _ProductCategory = new Lazy<IProductCategoryRepository>(() =>
            {
                return new ProductCategoryRepository(_Context);
            });

            _Product = new Lazy<IProductRepository>(() =>
            {
                return new ProductRepository(_Context);
            });

            _ContractStatus = new Lazy<IContractStatusRepository>(() =>
            {
                return new ContractStatusRepository(_Context);
            });

            _ResourceCategory = new Lazy<IResourceCategoryRepository>(() =>
            {
                return new ResourceCategoryRepository(_Context);
            });

            _Zone = new Lazy<IZoneRepository>(() =>
            {
                return new ZoneRepository(_Context);
            });

            _Resource = new Lazy<IResourceRepository>(() =>
            {
                return new ResourceRepository(_Context);
            });
            _ResourceMarket = new Lazy<IResourceMarketRepository>(() =>
            {
                return new ResourceMarketRepository(_Context);
            });
            _Manufacturer = new Lazy<IManufacturerReposiory>(() =>
            {
                return new ManufacturerReposiory(_Context);
            });

            _ContractProduct = new Lazy<IContractProductRepository>(() =>
            {
                return new ContractProductRepository(_Context);
            });

            _ContractMarket = new Lazy<IContractMarketRepository>(() =>
            {
                return new ContractMarketRepository(_Context);
            });

            _ContractStatusHistory = new Lazy<IContractStatusHistoryRepository>(() =>
            {
                return new ContractStatusHistoryRepository(_Context);
            });

            _ContractRebate = new Lazy<IContractRebateRepository>(() =>
           {
               return new ContractRebateRepository(_Context);
           });

            _ContractRebateBuilder = new Lazy<IContractRebateBuilderRepository>(() =>
           {
               return new ContractRebateBuilderRepository(_Context);
           });


            _ContractBuilder = new Lazy<IContractBuilderRepository>(() =>
           {
               return new ContractBuilderRepository(_Context);
           });


            _Market = new Lazy<IMarketRepository>(() =>
             {
                 return new MarketRepository(_Context);
             });

            _Survey = new Lazy<ISurveyRepository>(() =>
            {
                return new SurveyRepository(_Context);
            });


            _QuestionType = new Lazy<IQuestionTypeRepository>(() =>
            {
                return new QuestionTypeRepository(_Context);
            });

            _Question = new Lazy<IQuestionRepository>(() =>
            {
                return new QuestionRepository(_Context);
            });

            _TextBoxType = new Lazy<ITextBoxTypeRepository>(() =>
            {
                return new TextBoxTypeRepository(_Context);
            });

            _QuestionTextBoxSetting = new Lazy<IQuestionTextBoxSettingRepository>(() =>
            {
                return new QuestionTextBoxSettingRepository(_Context);
            });

            _QuestionDropdownSetting = new Lazy<IQuestionDropdownSettingRepository>(() =>
            {
                return new QuestionDropdownSettingRepository(_Context);
            });

            _QuestionGridSetting = new Lazy<IQuestionGridSettingRepository>(() =>
            {
                return new QuestionGridSettingRepository(_Context);
            });

            _QuestionGridSettingHeader = new Lazy<IQuestionGridSettingHeaderRepository>(() =>
            {
                return new QuestionGridSettingHeaderRepository(_Context);
            });

            _SurveyMarket = new Lazy<ISurveyMarketRepository>(() =>
            {
                return new SurveyMarketRepository(_Context);
            });

            _SurveyEmailSetting = new Lazy<ISurveyEmailSettingRepository>(() =>
           {
               return new SurveyEmailSettingRepository(_Context);
           });

            _SurveyInviteEmailSetting = new Lazy<ISurveyInviteEmailSettingRepository>(() =>
           {
               return new SurveyInviteEmailSettingRepository(_Context);
           });
            _SurveyRemainderEmailSetting = new Lazy<ISurveyRemainderEmailSettingRepository>(() =>
           {
               return new SurveyRemainderEmailSettingRepository(_Context);
           });
            _SurveySaveContinueEmailSetting = new Lazy<ISurveySaveContinueEmailSettingRepository>(() =>
           {
               return new SurveySaveContinueEmailSettingRepository(_Context);
           });



            _SurveyBuilder = new Lazy<ISurveyBuilderRepository>(() =>
            {
                return new SurveyBuilderRepository(_Context);
            });


            _SurveyBuilderEmailSent = new Lazy<ISurveyBuilderEmailSentRepository>(() =>
            {
                return new SurveyBuilderEmailSentRepository(_Context);
            });


            _SurveyBuilderUserEmailSent = new Lazy<ISurveyBuilderUserEmailSentRepository>(() =>
            {
                return new SurveyBuilderUserEmailSentRepository(_Context);
            });

            _ContractCompliance = new Lazy<IContractComplianceRepository>(() =>
            {
                return new ContractComplianceRepository(_Context);
            });


            _ContractComplianceBuilder = new Lazy<IContractComplianceBuilderRepository>(() =>
            {
                return new ContractComplianceBuilderRepository(_Context);
            });


            _SurveyResult = new Lazy<ISurveyResultRepository>(() =>
            {
                return new SurveyResultRepository(_Context);
            });


            _Quater = new Lazy<IQuaterRepository>(() =>
            {
                return new QuaterRepository(_Context);
            });

            _SurveyBuilderEmailSendDetails = new Lazy<ISurveyBuilderEmailSendDetailsRepository>(() =>
            {
                return new SurveyBuilderEmailSendDetailsRepository(_Context);
            });

            _BuilderQuaterContractProjectDetails = new Lazy<IBuilderQuaterContractProjectDetailsRepository>(() =>
            {
                return new BuilderQuaterContractProjectDetailsRepository(_Context);
            });
            _ConstructFormula = new Lazy<IConstructFormulaRepository>(() =>
            {
                return new ConstructFormulaRepository(_Context);
            });


            _ConstructFormulaMarket = new Lazy<IConstructFormulaMarketRepository>(() =>
            {
                return new ConstructFormulaMarketRepository(_Context);
            });


            _NonResponderReport = new Lazy<INonResponderReportRepository>(() => 
            {
                return new NonResponderReportRepository(_Context);
            });

            _Holiday = new Lazy<IHolidayRepository>(() =>
            {
                return new HolidayRepository(_Context);
            });

            _QuarterReminder = new Lazy<IQuarterReminderRepository>(() =>
            {
                return new QuarterReminderRepository(_Context);
            });

            _QuarterEmailTemplate = new Lazy<IQuarterEmailTemplateRepository>(() =>
            {
                return new QuarterEmailTemplateRepository(_Context);
            });
            //_SurveyResponseHistory = new Lazy<ISurveyResponseHistoryRepository>(() =>
            //{
            //    return new SurveyResponseHistoryRepository(_Context);
            //});

            //_SurveyResponseEditStatus = new Lazy<ISurveyResponseEditStatusRepository>(() =>
            //{
            //    return new SurveyResponseEditStatusRepository(_Context);
            //});

            // builder integration
            //_BuilderUser = new Lazy<IBuilderUserRepository>(() =>
            //{
            //    return new BuilderUserRepository(_Context);
            //});

            //_QBVendorDataReceived = new Lazy<IQBVendorDataReceivedRepository>(() =>
            //{
            //    return new QBVendorDataReceivedRepository(_Context);
            //});

            //_QBCategoryDataReceived = new Lazy<IQBCategoryDataReceivedRepository>(() =>
            //{
            //    return new QBCategoryDataReceivedRepository(_Context);
            //});


            //_QBBillDataReceived = new Lazy<IQBBillDataReceivedRepository>(() =>
            //{
            //    return new QBBillDataReceivedRepository(_Context);
            //});
            //_QBInvoiceDataReceived = new Lazy<IQBInvoiceDataReceivedRepository>(() =>
            //{
            //    return new QBInvoiceDataReceivedRepository(_Context);
            //});

            //_VendorMapping = new Lazy<IVendorMappingRepository>(() =>
            //{
            //    return new VendorMappingReposiroty(_Context);
            //});
            //_Vendor = new Lazy<IVendorRepository>(() =>
            //{
            //    return new VendorRepository(_Context);
            //});
            //_BuilderVendorRemoved = new Lazy<IBuilderVendorRemovedRepository>(() =>
            //{
            //    return new BuilderVendorRemovedRepository(_Context);
            //});
            //_CategoryMapping = new Lazy<ICategoryMappingRepository>(() =>
            //{
            //    return new CategoryMappingRepository(_Context);
            //});
            //_BuilderCategoryRemoved = new Lazy<IBuilderCategoryRemovedRepository>(() =>
            //{
            //    return new BuilderCategoryRemovedRepository(_Context);
            //});
            //_SubmitReport = new Lazy<ISubmitReportRepository>(() =>
            //{
            //    return new SubmitReportRepository(_Context);
            //});

            //For store and retrive ChallengeQuestion
            _ChallengeQuestion = new Lazy<IChallengeQuestionRepository>(() =>
            {
                return new ChallengeQuestionRepository(_Context);
            });

            //For Storing and retrive Eula
            _Eula = new Lazy<IEulaRepository>(() =>
            {
                return new EulaRepository(_Context);
            });

            //For Storing and retrive password Logs
            _PasswordLog = new Lazy<IPasswordLogRepository>(() =>
            {
                return new PasswordLogRepository(_Context);
            });

            _UserInRole = new Lazy<IUserInRoleRepository>(() =>
            {
                return new UserInRoleRepository(_Context);
            });

            _Role = new Lazy<IRoleRepository>(() =>
            {
                return new RoleRepository(_Context);
            });

            _UserChallangeQuestion = new Lazy<IUserChallangeQuestionRepository>(() =>
            {
                return new UserChallangeQuestionRepository(_Context);
            });
            _BuilderQuarterContractStatus = new Lazy<IBuilderQuarterContractStatusRepository>(() =>
            {
                return new BuilderQuarterContractStatusRepository(_Context);
            });
            _ProjectReportStatus = new Lazy<IProjectReportStatusRepository>(() =>
            {
                return new ProjectReportStatusRepository(_Context);
            });
            _QuarterContractStatus = new Lazy<IQuarterContractStatusRepository>(() =>
            {
                return new QuarterContractStatusRepository(_Context);
            });


            //************************* CONTRACT CENTRAL *************************
            _LuAppContentSection = new Lazy<ILuAppContentSectionRepository>(() =>
            {
                return new LuAppContentSectionRepository(_Context);
            });

            _LuAppTag = new Lazy<ILuAppTagRepository>(() =>
            {
                return new LuAppTagRepository(_Context);
            });

            _Content = new Lazy<IContentRepository>(() =>
            {
                return new ContentRepository(_Context);
            });

            _ContentAttachment = new Lazy<IContentAttachmentRepository>(() =>
            {
                return new ContentAttachmentRepository(_Context);
            });
            
            _ContentImage = new Lazy<IContentImageRepository>(() =>
            {
                return new ContentImageRepository(_Context);
            });
            
            _ContentMarket = new Lazy<IContentMarketRepository>(() =>
            {
                return new ContentMarketRepository(_Context);
            });

            _Image = new Lazy<IImageRepository>(() =>
            {
                return new ImageRepository(_Context);
            });

            _ImageTag = new Lazy<IImageTagRepository>(() =>
            {
                return new ImageTagRepository(_Context);
            });

            _Attachment = new Lazy<IAttachmentRepository>(() =>
            {
                return new AttachmentRepository(_Context);
            });

            _AttachmentTag = new Lazy<IAttachmentTagRepository>(() =>
            {
                return new AttachmentTagRepository(_Context);
            });
            //********************************************************************
        }
        //public IStudentRepository Students
        //{
        //    get
        //    {
        //        return _Student.Value;
        //    }
        //}

        //public IContractRepository Contracts
        //{
        //    get
        //    {
        //        return _Contract.Value;
        //    }
        //}
        public bool Flag
        {
            get
            {
                return _Flag;
            }
            set
            {
                _Flag = value;
            }
        }

        public int Complete()
        {
            return _Context.SaveChanges();
        }

        public void Dispose()
        {
            if (_Flag)
            {
                _Context.Dispose();
            }
        }


        IContractRepository IUnitOfWork.Contracts
        {
            get
            {
                return _Contract.Value;
            }
        }



        ICategoryRepository IUnitOfWork.Categories
        {
            get
            {
                return _Category.Value;
            }
        }






        /// Rabi
        /// 

        public IBuilderRepository Builder
        {
            get
            {
                return _Builder.Value;
            }
        }


        public IBuilderUserRepository BuilderUser
        {
            get
            {
                return _BuilderUser.Value;
            }
        }

        public IProductCategoryRepository ProductCategory
        {
            get
            {
                return _ProductCategory.Value;
            }
        }

        public IProductRepository Product
        {
            get
            {
                return _Product.Value;
            }
        }

        public IContractStatusRepository ContractStatus
        {
            get
            {
                return _ContractStatus.Value;
            }
        }

        public IResourceCategoryRepository ResourceCategory
        {
            get
            {
                return _ResourceCategory.Value;
            }
        }

        public IZoneRepository Zone
        {
            get
            {
                return _Zone.Value;
            }
        }

        public IResourceRepository Resource
        {
            get
            {
                return _Resource.Value;
            }
        }

        public IResourceMarketRepository ResourceMarket
        {
            get
            {
                return _ResourceMarket.Value;
            }
        }

        public IManufacturerReposiory Manufacturer
        {
            get
            {
                return _Manufacturer.Value;
            }
        }

        public IContractProductRepository ContractProduct
        {
            get
            {
                return _ContractProduct.Value;
            }
        }


        public IContractMarketRepository ContractMarket
        {
            get
            {
                return _ContractMarket.Value;
            }
        }

        public IContractStatusHistoryRepository ContractStatusHistory
        {
            get
            {
                return _ContractStatusHistory.Value;
            }
        }


        public IContractRebateRepository ContractRebate
        {
            get
            {
                return _ContractRebate.Value;
            }
        }


        public IContractRebateBuilderRepository ContractRebateBuilder
        {
            get
            {
                return _ContractRebateBuilder.Value;
            }
        }

        public IContractBuilderRepository ContractBuilder
        {
            get
            {
                return _ContractBuilder.Value;
            }
        }


        public IMarketRepository Market
        {
            get
            {
                return _Market.Value;
            }
        }


        public ISurveyRepository Survey
        {
            get
            {
                return _Survey.Value;
            }
        }


        public IQuestionTypeRepository QuestionType
        {
            get
            {
                return _QuestionType.Value;
            }
        }


        public IQuestionRepository Question
        {
            get
            {
                return _Question.Value;
            }
        }


        public ITextBoxTypeRepository TextBoxType
        {
            get
            {
                return _TextBoxType.Value;
            }
        }

        public IQuestionTextBoxSettingRepository QuestionTextBoxSetting
        {
            get
            {
                return _QuestionTextBoxSetting.Value;
            }
        }

        public IQuestionDropdownSettingRepository QuestionDropdownSetting
        {
            get
            {
                return _QuestionDropdownSetting.Value;
            }
        }

        public IQuestionGridSettingRepository QuestionGridSetting
        {
            get
            {
                return _QuestionGridSetting.Value;
            }
        }

        public IQuestionGridSettingHeaderRepository QuestionGridSettingHeader
        {
            get
            {
                return _QuestionGridSettingHeader.Value;
            }
        }

        public ISurveyMarketRepository SurveyMarket
        {
            get
            {
                return _SurveyMarket.Value;
            }
        }


        public ISurveyEmailSettingRepository SurveyEmailSetting
        {
            get
            {
                return _SurveyEmailSetting.Value;
            }
        }

        public ISurveyInviteEmailSettingRepository SurveyInviteEmailSetting
        {
            get
            {
                return _SurveyInviteEmailSetting.Value;
            }
        }


        public ISurveyRemainderEmailSettingRepository SurveyRemainderEmailSetting
        {
            get
            {
                return _SurveyRemainderEmailSetting.Value;
            }
        }


        public ISurveySaveContinueEmailSettingRepository SurveySaveContinueEmailSetting
        {
            get
            {
                return _SurveySaveContinueEmailSetting.Value;
            }
        }
        public ISurveyBuilderRepository SurveyBuilder
        {
            get
            {
                return _SurveyBuilder.Value;
            }
        }
        public ISurveyBuilderEmailSentRepository BuilderEmailSent
        {
            get
            {
                return _SurveyBuilderEmailSent.Value;
            }
        }


        public ISurveyBuilderUserEmailSentRepository BuilderUserEmailSent
        {
            get
            {
                return _SurveyBuilderUserEmailSent.Value;
            }
        }

        public IContractComplianceRepository ContractCompliance
        {
            get
            {
                return _ContractCompliance.Value;
            }
        }

        public IContractComplianceBuilderRepository ContractComplianceBuilder
        {
            get
            {
                return _ContractComplianceBuilder.Value;
            }
        }

        public ISurveyResultRepository SurveyResult
        {
            get
            {
                return _SurveyResult.Value;
            }
        }
        public IQuaterRepository Quater
        {
            get
            {
                return _Quater.Value;
            }
        }

        public ISurveyBuilderEmailSendDetailsRepository SurveyBuilderEmailSendDetails
        {
            get
            {
                return _SurveyBuilderEmailSendDetails.Value;
            }
        }

        // prasenjit adhikari

        public IStateRepository State
        {
            get
            {
                return _State.Value;
            }
        }
        public ICityRepository City
        {
            get
            {
                return _City.Value;
            }
        }
        public IProjectRepository Project
        {
            get
            {
                return _Project.Value;
            }
        }
        public IProjectStatusRepository ProjectStatus
        {
            get
            {
                return _ProjectStatus.Value;
            }
        }
        public IBuilderQuaterAdminReportRepository BuilderQuaterAdminReport
        {
            get
            {
                return _BuilderQuaterAdminReport.Value;
            }
        }
        public IBuilderQuaterContractProjectReportRepository BuilderQuaterContractProjectReport
        {
            get
            {
                return _BuilderQuaterContractProjectReport.Value;
            }
        }

        public IBuilderQuaterContractProjectDetailsRepository BuilderQuaterContractProjectDetails
        {
            get
            {
                return _BuilderQuaterContractProjectDetails.Value;
            }
        }
        public IConstructFormulaRepository ConstructFormula
        {
            get
            {
                return _ConstructFormula.Value;
            }
        }

        public IConstructFormulaMarketRepository ConstructFormulaMarket
        {
            get
            {
                return _ConstructFormulaMarket.Value;
            }
        }

        public INonResponderReportRepository NonResponderReport
        {
            get
            {
                return _NonResponderReport.Value;
            }
        }

        public IHolidayRepository Holiday
        {
            get
            {
                return _Holiday.Value;
            }
        }

        public IQuarterReminderRepository QuarterReminder
        {
            get
            {
                return _QuarterReminder.Value;
            }
        }

        public IQuarterEmailTemplateRepository QuarterEmailTemplate
        {
            get
            {
                return _QuarterEmailTemplate.Value;
            }
        }
        //public ISurveyResponseHistoryRepository SurveyResponseHistory
        //{
        //    get
        //    {
        //        return _SurveyResponseHistory.Value;
        //    }
        //}

        //public ISurveyResponseEditStatusRepository SurveyResponseEditStatus
        //{
        //    get
        //    {
        //        return _SurveyResponseEditStatus.Value;
        //    }
        //}

        // builder integration

        //public IQBVendorDataReceivedRepository QBVendorDataReceived
        //{
        //    get
        //    {
        //        return _QBVendorDataReceived.Value;
        //    }
        //}

        //public IQBCategoryDataReceivedRepository QBCategoryDataReceived
        //{
        //    get
        //    {
        //        return _QBCategoryDataReceived.Value;
        //    }
        //}
        //public IQBBillDataReceivedRepository QBBillDataReceived
        //{
        //    get
        //    {
        //        return _QBBillDataReceived.Value;
        //    }
        //}
        //public IQBInvoiceDataReceivedRepository QBInvoiceDataReceived
        //{
        //    get
        //    {
        //        return _QBInvoiceDataReceived.Value;
        //    }
        //}
        //public IVendorMappingRepository VendorMapping
        //{
        //    get { return _VendorMapping.Value; }
        //}
        //public IVendorRepository Vendor
        //{
        //    get { return _Vendor.Value; }
        //}
        //public IBuilderVendorRemovedRepository BuilderVendorRemoved
        //{
        //    get { return _BuilderVendorRemoved.Value; }
        //}
        //public ICategoryMappingRepository CategoryMapping
        //{
        //    get { return _CategoryMapping.Value; }
        //}
        //public IBuilderCategoryRemovedRepository BuilderCategoryRemoved
        //{
        //    get { return _BuilderCategoryRemoved.Value; }
        //}
        //public ISubmitReportRepository SubmitReport
        //{
        //    get { return _SubmitReport.Value; }
        //}

        //For store and retrive ChallengeQuestion
        public IChallengeQuestionRepository ChallengeQuestions
        {
            get
            {
                return _ChallengeQuestion.Value;
            }
        }

        //For Storing Eula
        public IEulaRepository Eulas
        {
            get
            {
                return _Eula.Value;
            }
        }

        //For Storing password Logs
        public IPasswordLogRepository PasswordLogs
        {
            get
            {
                return _PasswordLog.Value;
            }
        }

        public IUserInRoleRepository UserInRole
        {
            get
            {
                return _UserInRole.Value;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                return _Role.Value;
            }
        }

        public IUserChallangeQuestionRepository UserChallangeQuestion
        {
            get
            {
                return _UserChallangeQuestion.Value;
            }
        }
        public IBuilderQuarterContractStatusRepository BuilderQuarterContractStatus
        {
            get
            {
                return _BuilderQuarterContractStatus.Value;
            }
        }
        public IProjectReportStatusRepository ProjectReportStatus
        {
            get
            {
                return _ProjectReportStatus.Value;
            }
        }
        public IQuarterContractStatusRepository QuarterContractStatus
        {
            get
            {
                return _QuarterContractStatus.Value;
            }
        }

        //************************* CONTRACT CENTRAL **************************
        public ILuAppContentSectionRepository LuAppContentSection
        {
            get
            {
                return _LuAppContentSection.Value;
            }
        }

        public ILuAppTagRepository LuAppTag
        {
            get
            {
                return _LuAppTag.Value;
            }
        }

        public IContentRepository Content
        {
            get
            {
                return _Content.Value;
            }
        }

        public IContentAttachmentRepository ContentAttachment
        {
            get
            {
                return _ContentAttachment.Value;
            }
        }

        public IContentMarketRepository ContentMarket
        {
            get
            {
                return _ContentMarket.Value;
            }
        }

        public IContentImageRepository ContentImage
        {
            get
            {
                return _ContentImage.Value;
            }
        }

        public IImageRepository Image
        {
            get
            {
                return _Image.Value;
            }
        }

        public IImageTagRepository ImageTag
        {
            get
            {
                return _ImageTag.Value;
            }
        }
        
        public IAttachmentRepository Attachment
        {
            get
            {
                return _Attachment.Value;
            }
        }

        public IAttachmentTagRepository AttachmentTag
        {
            get
            {
                return _AttachmentTag.Value;
            }
        }
        //***********************************************************************
    }
}
