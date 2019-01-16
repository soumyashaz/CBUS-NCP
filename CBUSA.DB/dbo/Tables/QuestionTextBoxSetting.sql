CREATE TABLE [dbo].[QuestionTextBoxSetting] (
    [QuestionTextBoxSettingId] BIGINT IDENTITY (1, 1) NOT NULL,
    [IsAlphabets]              BIT    NOT NULL,
    [IsNumber]                 BIT    NOT NULL,
    [IsSpecialCharecter]       BIT    NOT NULL,
    [LowerLimit]               INT    NOT NULL,
    [UpperLimit]               INT    NOT NULL,
    [TextBoxTypeId]            INT    NOT NULL,
    [QuestionId]               BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.QuestionTextBoxSetting] PRIMARY KEY CLUSTERED ([QuestionTextBoxSettingId] ASC),
    CONSTRAINT [FK_dbo.QuestionTextBoxSetting_dbo.Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([QuestionId]),
    CONSTRAINT [FK_dbo.QuestionTextBoxSetting_dbo.TextBoxType_TextBoxTypeId] FOREIGN KEY ([TextBoxTypeId]) REFERENCES [dbo].[TextBoxType] ([TextBoxTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionId]
    ON [dbo].[QuestionTextBoxSetting]([QuestionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TextBoxTypeId]
    ON [dbo].[QuestionTextBoxSetting]([TextBoxTypeId] ASC);

