CREATE TABLE [dbo].[SurveySaveContinueEmailSetting] (
    [SurveySaveContinueEmailSettingId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Subject]                          NVARCHAR (250) NULL,
    [EmailContent]                     NVARCHAR (MAX) NULL,
    [SurveyEmailSettingId]             BIGINT         NULL,
    [DumpId]                           NVARCHAR (100) NULL,
    CONSTRAINT [PK_dbo.SurveySaveContinueEmailSetting] PRIMARY KEY CLUSTERED ([SurveySaveContinueEmailSettingId] ASC),
    CONSTRAINT [FK_dbo.SurveySaveContinueEmailSetting_dbo.SurveyEmailSetting_SurveyEmailSettingId] FOREIGN KEY ([SurveyEmailSettingId]) REFERENCES [dbo].[SurveyEmailSetting] ([SurveyEmailSettingId])
);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyEmailSettingId]
    ON [dbo].[SurveySaveContinueEmailSetting]([SurveyEmailSettingId] ASC);

