CREATE TABLE [dbo].[SurveyEmailSetting] (
    [SurveyEmailSettingId]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [SenderEmail]                  NVARCHAR (100) NULL,
    [RemainderForTakeSurvey]       BIT            NOT NULL,
    [DayBeforeSurveyEnd]           INT            NOT NULL,
    [RemainderForContinueSurvey]   BIT            NOT NULL,
    [DayAfterSurveyEnd]            INT            NOT NULL,
    [SurveyId]                     BIGINT         NOT NULL,
    [RemainderForTakeSurveySecond] BIT            DEFAULT ((0)) NOT NULL,
    [DayBeforeSurveyEndSecond]     INT            DEFAULT ((0)) NOT NULL,
    [RemainderForTakeSurveyThird]  BIT            DEFAULT ((0)) NOT NULL,
    [DayBeforeSurveyEndThird]      INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.SurveyEmailSetting] PRIMARY KEY CLUSTERED ([SurveyEmailSettingId] ASC),
    CONSTRAINT [FK_dbo.SurveyEmailSetting_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[SurveyEmailSetting]([SurveyId] ASC);

