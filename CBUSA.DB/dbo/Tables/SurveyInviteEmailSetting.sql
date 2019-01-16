CREATE TABLE [dbo].[SurveyInviteEmailSetting] (
    [SurveyInviteEmailSettingId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Subject]                    NVARCHAR (250) NULL,
    [EmailContent]               NVARCHAR (MAX) NULL,
    [SurveyEmailSettingId]       BIGINT         NULL,
    [DumpId]                     NVARCHAR (100) NULL,
    CONSTRAINT [PK_dbo.SurveyInviteEmailSetting] PRIMARY KEY CLUSTERED ([SurveyInviteEmailSettingId] ASC),
    CONSTRAINT [FK_dbo.SurveyInviteEmailSetting_dbo.SurveyEmailSetting_SurveyEmailSettingId] FOREIGN KEY ([SurveyEmailSettingId]) REFERENCES [dbo].[SurveyEmailSetting] ([SurveyEmailSettingId])
);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyEmailSettingId]
    ON [dbo].[SurveyInviteEmailSetting]([SurveyEmailSettingId] ASC);

