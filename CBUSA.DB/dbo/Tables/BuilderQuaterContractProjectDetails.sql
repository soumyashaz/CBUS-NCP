CREATE TABLE [dbo].[BuilderQuaterContractProjectDetails] (
    [BuilderQuaterContractProjectDetailsId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [Answer]                                NVARCHAR (MAX)   NULL,
    [RowNumber]                             INT              NOT NULL,
    [ColumnNumber]                          INT              NOT NULL,
    [QuestionId]                            BIGINT           NOT NULL,
    [BuilderQuaterContractProjectReportId]  BIGINT           NOT NULL,
    [RowStatusId]                           INT              DEFAULT ((0)) NOT NULL,
    [CreatedOn]                             DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [CreatedBy]                             INT              DEFAULT ((0)) NOT NULL,
    [ModifiedOn]                            DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ModifiedBy]                            INT              DEFAULT ((0)) NOT NULL,
    [RowGUID]                               UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [FileName]                              NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.BuilderQuaterContractProjectDetails] PRIMARY KEY CLUSTERED ([BuilderQuaterContractProjectDetailsId] ASC),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectDetails_dbo.BuilderQuaterContractProjectReport_BuilderQuaterContractProjectReportId] FOREIGN KEY ([BuilderQuaterContractProjectReportId]) REFERENCES [dbo].[BuilderQuaterContractProjectReport] ([BuilderQuaterContractProjectReportId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectDetails_dbo.Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([QuestionId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectDetails_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderQuaterContractProjectReportId]
    ON [dbo].[BuilderQuaterContractProjectDetails]([BuilderQuaterContractProjectReportId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionId]
    ON [dbo].[BuilderQuaterContractProjectDetails]([QuestionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[BuilderQuaterContractProjectDetails]([RowStatusId] ASC);

