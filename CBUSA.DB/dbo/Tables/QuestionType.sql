CREATE TABLE [dbo].[QuestionType] (
    [QuestionTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [TypeName]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.QuestionType] PRIMARY KEY CLUSTERED ([QuestionTypeId] ASC)
);

