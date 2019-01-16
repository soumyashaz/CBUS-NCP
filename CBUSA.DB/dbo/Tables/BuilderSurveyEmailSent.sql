CREATE TABLE [dbo].[BuilderSurveyEmailSent] (
    [BuilderSurveyEmailSentId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [SurveyId]                 BIGINT         NOT NULL,
    [BuilderId]                BIGINT         NOT NULL,
    [GroupId]                  NVARCHAR (250) NULL,
    [SendDate]                 DATETIME       NOT NULL,
    [SendBy]                   BIGINT         NOT NULL,
    [IsMailSent]               BIT            NOT NULL,
    CONSTRAINT [PK_dbo.BuilderSurveyEmailSent] PRIMARY KEY CLUSTERED ([BuilderSurveyEmailSentId] ASC),
    CONSTRAINT [FK_dbo.BuilderSurveyEmailSent_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.BuilderSurveyEmailSent_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[BuilderSurveyEmailSent]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[BuilderSurveyEmailSent]([SurveyId] ASC);

