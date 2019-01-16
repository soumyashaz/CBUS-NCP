CREATE TABLE [dbo].[ConstructFormula] (
    [ConstructFormulaId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [ContractId]            BIGINT           NOT NULL,
    [SurveyId]              BIGINT           NOT NULL,
    [Quarter]               NVARCHAR (MAX)   NOT NULL,
    [Year]                  NVARCHAR (MAX)   NULL,
    [QuestionColumnId]      BIGINT           NOT NULL,
    [QuestionColumnValueId] BIGINT           NOT NULL,
    [FormulaDescription]    NVARCHAR (MAX)   NULL,
    [FormulaBuild]          NVARCHAR (MAX)   NULL,
    [RowStatusId]           INT              NOT NULL,
    [CreatedOn]             DATETIME         NOT NULL,
    [CreatedBy]             INT              NOT NULL,
    [ModifiedOn]            DATETIME         NOT NULL,
    [ModifiedBy]            INT              NOT NULL,
    [RowGUID]               UNIQUEIDENTIFIER NOT NULL,
    [Quater_QuaterId]       BIGINT           NULL,
    CONSTRAINT [PK_dbo.ConstructFormula] PRIMARY KEY CLUSTERED ([ConstructFormulaId] ASC),
    CONSTRAINT [FK_dbo.ConstructFormula_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.ConstructFormula_dbo.Quater_Quater_QuaterId] FOREIGN KEY ([Quater_QuaterId]) REFERENCES [dbo].[Quater] ([QuaterId]),
    CONSTRAINT [FK_dbo.ConstructFormula_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId]),
    CONSTRAINT [FK_dbo.ConstructFormula_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);

