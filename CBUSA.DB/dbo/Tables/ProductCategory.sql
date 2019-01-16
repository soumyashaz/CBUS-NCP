CREATE TABLE [dbo].[ProductCategory] (
    [ProductCategoryId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [ProductCategoryName] NVARCHAR (MAX)   NOT NULL,
    [ParentId]            BIGINT           NOT NULL,
    [RowStatusId]         INT              NOT NULL,
    [CreatedOn]           DATETIME         NOT NULL,
    [CreatedBy]           INT              NOT NULL,
    [ModifiedOn]          DATETIME         NOT NULL,
    [ModifiedBy]          INT              NOT NULL,
    [RowGUID]             UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.ProductCategory] PRIMARY KEY CLUSTERED ([ProductCategoryId] ASC),
    CONSTRAINT [FK_dbo.ProductCategory_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[ProductCategory]([RowStatusId] ASC);

