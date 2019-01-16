CREATE TABLE [dbo].[ResourceCategory] (
    [ResourceCategoryId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [ResourceCategoryName] NVARCHAR (MAX)   NULL,
    [RowStatusId]          INT              NOT NULL,
    [CreatedOn]            DATETIME         NOT NULL,
    [CreatedBy]            INT              NOT NULL,
    [ModifiedOn]           DATETIME         NOT NULL,
    [ModifiedBy]           INT              NOT NULL,
    [RowGUID]              UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.ResourceCategory] PRIMARY KEY CLUSTERED ([ResourceCategoryId] ASC),
    CONSTRAINT [FK_dbo.ResourceCategory_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[ResourceCategory]([RowStatusId] ASC);

