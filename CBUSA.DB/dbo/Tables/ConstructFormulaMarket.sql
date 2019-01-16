CREATE TABLE [dbo].[ConstructFormulaMarket] (
    [ConstructFormulaMarketId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ConstructFormulaId]       BIGINT NOT NULL,
    [MarketId]                 BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.ConstructFormulaMarket] PRIMARY KEY CLUSTERED ([ConstructFormulaMarketId] ASC),
    CONSTRAINT [FK_dbo.ConstructFormulaMarket_dbo.ConstructFormula_ConstructFormulaId] FOREIGN KEY ([ConstructFormulaId]) REFERENCES [dbo].[ConstructFormula] ([ConstructFormulaId]),
    CONSTRAINT [FK_dbo.ConstructFormulaMarket_dbo.Market_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [dbo].[Market] ([MarketId])
);

