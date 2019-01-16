CREATE TABLE [dbo].[ResourceMarket] (
    [ResourceMarketId] BIGINT IDENTITY (1, 1) NOT NULL,
    [MarketId]         BIGINT NOT NULL,
    [ResourceId]       BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.ResourceMarket] PRIMARY KEY CLUSTERED ([ResourceMarketId] ASC),
    CONSTRAINT [FK_dbo.ResourceMarket_dbo.Market_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [dbo].[Market] ([MarketId]),
    CONSTRAINT [FK_dbo.ResourceMarket_dbo.Resource_ResourceId] FOREIGN KEY ([ResourceId]) REFERENCES [dbo].[Resource] ([ResourceId])
);


GO
CREATE NONCLUSTERED INDEX [IX_MarketId]
    ON [dbo].[ResourceMarket]([MarketId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResourceId]
    ON [dbo].[ResourceMarket]([ResourceId] ASC);

