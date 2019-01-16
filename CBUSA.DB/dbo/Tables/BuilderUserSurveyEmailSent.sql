CREATE TABLE [dbo].[BuilderUserSurveyEmailSent] (
    [BuilderUserSurveyEmailSentId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [SurveyId]                     BIGINT         NOT NULL,
    [BuilderId]                    BIGINT         NOT NULL,
    [BuilderUserId]                BIGINT         NOT NULL,
    [GroupId]                      NVARCHAR (250) NULL,
    [SendDate]                     DATETIME       NOT NULL,
    [SendBy]                       BIGINT         NOT NULL,
    [IsMailSent]                   BIT            NOT NULL,
    CONSTRAINT [PK_dbo.BuilderUserSurveyEmailSent] PRIMARY KEY CLUSTERED ([BuilderUserSurveyEmailSentId] ASC),
    CONSTRAINT [FK_dbo.BuilderUserSurveyEmailSent_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.BuilderUserSurveyEmailSent_dbo.BuilderUser_BuilderUserId] FOREIGN KEY ([BuilderUserId]) REFERENCES [dbo].[BuilderUser] ([BuilderUserId]),
    CONSTRAINT [FK_dbo.BuilderUserSurveyEmailSent_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);

