CREATE TABLE [dbo].[Product] (
    [ProductId]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [ProductName]       NVARCHAR (MAX)   NOT NULL,
    [ProductCategoryId] BIGINT           NOT NULL,
    [RowStatusId]       INT              NOT NULL,
    [CreatedOn]         DATETIME         NOT NULL,
    [CreatedBy]         INT              NOT NULL,
    [ModifiedOn]        DATETIME         NOT NULL,
    [ModifiedBy]        INT              NOT NULL,
    [RowGUID]           UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.Product] PRIMARY KEY CLUSTERED ([ProductId] ASC),
    CONSTRAINT [FK_dbo.Product_dbo.ProductCategory_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[ProductCategory] ([ProductCategoryId]),
    CONSTRAINT [FK_dbo.Product_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProductCategoryId]
    ON [dbo].[Product]([ProductCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Product]([RowStatusId] ASC);

