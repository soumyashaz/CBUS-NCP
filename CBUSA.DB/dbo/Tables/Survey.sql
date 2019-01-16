CREATE TABLE [dbo].[Survey] (
    [SurveyId]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [SurveyName]            NVARCHAR (MAX)   NOT NULL,
    [Label]                 NVARCHAR (MAX)   NOT NULL,
    [StartDate]             DATETIME         NULL,
    [EndDate]               DATETIME         NULL,
    [IsEnrolment]           BIT              NOT NULL,
    [IsPublished]           BIT              NOT NULL,
    [RowStatusId]           INT              NOT NULL,
    [CreatedOn]             DATETIME         NOT NULL,
    [CreatedBy]             INT              NOT NULL,
    [ModifiedOn]            DATETIME         NOT NULL,
    [ModifiedBy]            INT              NOT NULL,
    [RowGUID]               UNIQUEIDENTIFIER NOT NULL,
    [ContractId]            BIGINT           NOT NULL,
    [ContractCurrentStatus] BIGINT           DEFAULT ((0)) NOT NULL,
    [IsNcpSurvey]           BIT              DEFAULT ((0)) NOT NULL,
    [Publishdate]           DATETIME         NULL,
    [ArchivedDate]          DATETIME         NULL,
    [Year]                  NVARCHAR (250)   NULL,
    [Quater]                NVARCHAR (250)   NULL,
    CONSTRAINT [PK_dbo.Survey] PRIMARY KEY CLUSTERED ([SurveyId] ASC),
    CONSTRAINT [FK_dbo.Survey_dbo.Contract_Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.Survey_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[Survey]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Survey]([RowStatusId] ASC);

