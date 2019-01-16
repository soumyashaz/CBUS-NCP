CREATE TABLE [dbo].[ContractCompliance] (
    [ContractComplianceId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [EstimatedValue]       BIT              NOT NULL,
    [ActualValue]          BIT              NOT NULL,
    [QuestionId]           BIGINT           NULL,
    [ComplianceFormula]    XML              NULL,
    [ContractId]           BIGINT           NOT NULL,
    [SurveyId]             BIGINT           NOT NULL,
    [RowStatusId]          INT              DEFAULT ((0)) NOT NULL,
    [CreatedOn]            DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [CreatedBy]            INT              DEFAULT ((0)) NOT NULL,
    [ModifiedOn]           DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ModifiedBy]           INT              DEFAULT ((0)) NOT NULL,
    [RowGUID]              UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [IsDirectQuestion]     BIT              DEFAULT ((0)) NOT NULL,
    [QuaterId]             BIGINT           NULL,
    CONSTRAINT [PK_dbo.ContractCompliance] PRIMARY KEY CLUSTERED ([ContractComplianceId] ASC),
    CONSTRAINT [FK_dbo.ContractCompliance_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.ContractCompliance_dbo.Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([QuestionId]),
    CONSTRAINT [FK_dbo.ContractCompliance_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId]),
    CONSTRAINT [FK_dbo.ContractCompliance_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId]),
    CONSTRAINT [fk_Quater_QuaterId] FOREIGN KEY ([QuaterId]) REFERENCES [dbo].[Quater] ([QuaterId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[ContractCompliance]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionId]
    ON [dbo].[ContractCompliance]([QuestionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[ContractCompliance]([RowStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[ContractCompliance]([SurveyId] ASC);

