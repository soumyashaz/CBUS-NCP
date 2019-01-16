CREATE TABLE [dbo].[TextBoxType] (
    [TextBoxTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [TextBoxTypeName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.TextBoxType] PRIMARY KEY CLUSTERED ([TextBoxTypeId] ASC)
);

