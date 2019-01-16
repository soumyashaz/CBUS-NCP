CREATE TABLE [dbo].[SurveyRemainderEmailSetting] (
    [SurveyRemainderEmailSettingId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Subject]                       NVARCHAR (250) NULL,
    [EmailContent]                  NVARCHAR (MAX) NULL,
    [SurveyEmailSettingId]          BIGINT         NULL,
    [DumpId]                        NVARCHAR (100) NULL,
    CONSTRAINT [PK_dbo.SurveyRemainderEmailSetting] PRIMARY KEY CLUSTERED ([SurveyRemainderEmailSettingId] ASC),
    CONSTRAINT [FK_dbo.SurveyRemainderEmailSetting_dbo.SurveyEmailSetting_SurveyEmailSettingId] FOREIGN KEY ([SurveyEmailSettingId]) REFERENCES [dbo].[SurveyEmailSetting] ([SurveyEmailSettingId])
);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyEmailSettingId]
    ON [dbo].[SurveyRemainderEmailSetting]([SurveyEmailSettingId] ASC);

