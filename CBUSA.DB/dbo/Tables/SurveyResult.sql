CREATE TABLE [dbo].[SurveyResult] (
    [SurveyResultId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [Answer]         NVARCHAR (MAX)   NULL,
    [RowNumber]      INT              NOT NULL,
    [ColumnNumber]   INT              NOT NULL,
    [QuestionId]     BIGINT           NOT NULL,
    [RowStatusId]    INT              NOT NULL,
    [CreatedOn]      DATETIME         NOT NULL,
    [CreatedBy]      INT              NOT NULL,
    [ModifiedOn]     DATETIME         NOT NULL,
    [ModifiedBy]     INT              NOT NULL,
    [RowGUID]        UNIQUEIDENTIFIER NOT NULL,
    [SurveyId]       BIGINT           DEFAULT ((0)) NOT NULL,
    [BuilderId]      BIGINT           DEFAULT ((0)) NOT NULL,
    [FileName]       NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.SurveyResult] PRIMARY KEY CLUSTERED ([SurveyResultId] ASC),
    CONSTRAINT [FK_dbo.SurveyResult_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.SurveyResult_dbo.Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([QuestionId]),
    CONSTRAINT [FK_dbo.SurveyResult_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId]),
    CONSTRAINT [FK_dbo.SurveyResult_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[SurveyResult]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionId]
    ON [dbo].[SurveyResult]([QuestionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[SurveyResult]([RowStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[SurveyResult]([SurveyId] ASC);

