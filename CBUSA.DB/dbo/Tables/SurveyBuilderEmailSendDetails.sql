CREATE TABLE [dbo].[SurveyBuilderEmailSendDetails] (
    [SurveyBuilderEmailSendDetailsId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [BuilderId]                       BIGINT           NOT NULL,
    [SendDate]                        DATETIME         NOT NULL,
    [RowStatusId]                     INT              NOT NULL,
    [CreatedOn]                       DATETIME         NOT NULL,
    [CreatedBy]                       INT              NOT NULL,
    [ModifiedOn]                      DATETIME         NOT NULL,
    [ModifiedBy]                      INT              NOT NULL,
    [RowGUID]                         UNIQUEIDENTIFIER NOT NULL,
    [SurveyId]                        BIGINT           NOT NULL,
    CONSTRAINT [PK_dbo.SurveyBuilderEmailSendDetails] PRIMARY KEY CLUSTERED ([SurveyBuilderEmailSendDetailsId] ASC),
    CONSTRAINT [FK_dbo.SurveyBuilderEmailSendDetails_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.SurveyBuilderEmailSendDetails_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId]),
    CONSTRAINT [FK_dbo.SurveyBuilderEmailSendDetails_dbo.Survey_Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[SurveyBuilderEmailSendDetails]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[SurveyBuilderEmailSendDetails]([RowStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[SurveyBuilderEmailSendDetails]([SurveyId] ASC);

