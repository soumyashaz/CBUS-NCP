CREATE TABLE [dbo].[ContractProduct] (
    [ContractProductId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ContractId]        BIGINT NOT NULL,
    [ProductId]         BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.ContractProduct] PRIMARY KEY CLUSTERED ([ContractProductId] ASC),
    CONSTRAINT [FK_dbo.ContractProduct_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.ContractProduct_dbo.Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[ContractProduct]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductId]
    ON [dbo].[ContractProduct]([ProductId] ASC);

