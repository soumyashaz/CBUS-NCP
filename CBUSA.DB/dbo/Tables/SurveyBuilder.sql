CREATE TABLE [dbo].[SurveyBuilder] (
    [SurveyBuilderId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [SurveyStartDate]    DATETIME         NOT NULL,
    [SurveyId]           BIGINT           NOT NULL,
    [BuilderId]          BIGINT           NOT NULL,
    [IsSurveyCompleted]  BIT              NOT NULL,
    [SurveyCompleteDate] DATETIME         NOT NULL,
    [RowStatusId]        INT              NOT NULL,
    [CreatedOn]          DATETIME         NOT NULL,
    [CreatedBy]          INT              NOT NULL,
    [ModifiedOn]         DATETIME         NOT NULL,
    [ModifiedBy]         INT              NOT NULL,
    [RowGUID]            UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.SurveyBuilder] PRIMARY KEY CLUSTERED ([SurveyBuilderId] ASC),
    CONSTRAINT [FK_dbo.SurveyBuilder_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.SurveyBuilder_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId]),
    CONSTRAINT [FK_dbo.SurveyBuilder_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[SurveyBuilder]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[SurveyBuilder]([RowStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[SurveyBuilder]([SurveyId] ASC);

