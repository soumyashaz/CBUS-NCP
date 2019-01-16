CREATE TABLE [dbo].[QuestionDropdownSetting] (
    [QuestionDropdownSettingId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Value]                     NVARCHAR (MAX) NULL,
    [QuestionId]                BIGINT         NOT NULL,
    CONSTRAINT [PK_dbo.QuestionDropdownSetting] PRIMARY KEY CLUSTERED ([QuestionDropdownSettingId] ASC),
    CONSTRAINT [FK_dbo.QuestionDropdownSetting_dbo.Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([QuestionId])
);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionId]
    ON [dbo].[QuestionDropdownSetting]([QuestionId] ASC);

