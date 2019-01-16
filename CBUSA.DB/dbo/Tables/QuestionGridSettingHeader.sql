CREATE TABLE [dbo].[QuestionGridSettingHeader] (
    [QuestionGridSettingHeaderId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [RowHeaderValue]              NVARCHAR (MAX) NULL,
    [ColoumnHeaderValue]          NVARCHAR (MAX) NULL,
    [IndexNumber]                 INT            NOT NULL,
    [QuestionGridSettingId]       BIGINT         NOT NULL,
    [ControlType]                 NVARCHAR (MAX) NULL,
    [DropdownTypeOptionValue]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.QuestionGridSettingHeader] PRIMARY KEY CLUSTERED ([QuestionGridSettingHeaderId] ASC),
    CONSTRAINT [FK_dbo.QuestionGridSettingHeader_dbo.QuestionGridSetting_QuestionGridSettingId] FOREIGN KEY ([QuestionGridSettingId]) REFERENCES [dbo].[QuestionGridSetting] ([QuestionGridSettingId])
);


GO
CREATE NONCLUSTERED INDEX [IX_QuestionGridSettingId]
    ON [dbo].[QuestionGridSettingHeader]([QuestionGridSettingId] ASC);

