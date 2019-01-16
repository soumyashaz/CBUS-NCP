CREATE TABLE [dbo].[Question] (
    [QuestionId]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [QuestionValue]      NVARCHAR (MAX)   NOT NULL,
    [IsMandatory]        BIT              NOT NULL,
    [IsFileNeedtoUpload] BIT              NOT NULL,
    [QuestionTypeId]     INT              NOT NULL,
    [SurveyId]           BIGINT           NOT NULL,
    [RowStatusId]        INT              DEFAULT ((0)) NOT NULL,
    [CreatedOn]          DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [CreatedBy]          INT              DEFAULT ((0)) NOT NULL,
    [ModifiedOn]         DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ModifiedBy]         INT              DEFAULT ((0)) NOT NULL,
    [RowGUID]            UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [SurveyOrder]        INT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Question] PRIMARY KEY CLUSTERED ([QuestionId] ASC),
    CONSTRAINT [FK_dbo.Question_dbo.QuestionType_QuestionTypeId] FOREIGN KEY ([QuestionTypeId]) REFERENCES [dbo].[QuestionType] ([QuestionTypeId]),
    CONSTRAINT [FK_dbo.Question_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId]),
    CONSTRAINT [FK_dbo.Question_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionTypeId]
    ON [dbo].[Question]([QuestionTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Question]([RowStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[Question]([SurveyId] ASC);

