CREATE TABLE [dbo].[QuestionGridSetting] (
    [QuestionGridSettingId] BIGINT IDENTITY (1, 1) NOT NULL,
    [Row]                   INT    NOT NULL,
    [Column]                INT    NOT NULL,
    [QuestionId]            BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.QuestionGridSetting] PRIMARY KEY CLUSTERED ([QuestionGridSettingId] ASC),
    CONSTRAINT [FK_dbo.QuestionGridSetting_dbo.Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([QuestionId])
);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionId]
    ON [dbo].[QuestionGridSetting]([QuestionId] ASC);

