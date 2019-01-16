CREATE TABLE [dbo].[ContractMarket] (
    [ContractMarketId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ContractId]       BIGINT NOT NULL,
    [MarketId]         BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.ContractMarket] PRIMARY KEY CLUSTERED ([ContractMarketId] ASC),
    CONSTRAINT [FK_dbo.ContractMarket_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.ContractMarket_dbo.Market_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [dbo].[Market] ([MarketId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[ContractMarket]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MarketId]
    ON [dbo].[ContractMarket]([MarketId] ASC);

