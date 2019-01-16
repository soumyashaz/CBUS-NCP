CREATE TABLE [dbo].[BuilderQuaterContractProjectReport] (
    [BuilderQuaterContractProjectReportId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [BuilderId]                            BIGINT           NOT NULL,
    [QuaterId]                             BIGINT           NOT NULL,
    [ContractId]                           BIGINT           NOT NULL,
    [ProjectId]                            BIGINT           NOT NULL,
    [ProjectStatusId]                      BIGINT           NOT NULL,
    [BuilderQuaterAdminReportId]           BIGINT           NULL,
    [IsComplete]                           BIT              NOT NULL,
    [CompleteDate]                         DATETIME         NOT NULL,
    [RowStatusId]                          INT              NOT NULL,
    [CreatedOn]                            DATETIME         NOT NULL,
    [CreatedBy]                            INT              NOT NULL,
    [ModifiedOn]                           DATETIME         NOT NULL,
    [ModifiedBy]                           INT              NOT NULL,
    [RowGUID]                              UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.BuilderQuaterContractProjectReport] PRIMARY KEY CLUSTERED ([BuilderQuaterContractProjectReportId] ASC),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectReport_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectReport_dbo.BuilderQuaterAdminReport_BuilderQuaterAdminReportId] FOREIGN KEY ([BuilderQuaterAdminReportId]) REFERENCES [dbo].[BuilderQuaterAdminReport] ([BuilderQuaterAdminReportId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectReport_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectReport_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectReport_dbo.ProjectStatus_ProjectStatusId] FOREIGN KEY ([ProjectStatusId]) REFERENCES [dbo].[ProjectStatus] ([ProjectStatusId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectReport_dbo.Quater_QuaterId] FOREIGN KEY ([QuaterId]) REFERENCES [dbo].[Quater] ([QuaterId]),
    CONSTRAINT [FK_dbo.BuilderQuaterContractProjectReport_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[BuilderQuaterContractProjectReport]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderQuaterAdminReportId]
    ON [dbo].[BuilderQuaterContractProjectReport]([BuilderQuaterAdminReportId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[BuilderQuaterContractProjectReport]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[BuilderQuaterContractProjectReport]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectStatusId]
    ON [dbo].[BuilderQuaterContractProjectReport]([ProjectStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_QuaterId]
    ON [dbo].[BuilderQuaterContractProjectReport]([QuaterId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[BuilderQuaterContractProjectReport]([RowStatusId] ASC);

