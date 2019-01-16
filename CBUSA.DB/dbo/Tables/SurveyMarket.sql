CREATE TABLE [dbo].[SurveyMarket] (
    [SurveyMarketId] BIGINT IDENTITY (1, 1) NOT NULL,
    [SurveyId]       BIGINT NOT NULL,
    [MarketId]       BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.SurveyMarket] PRIMARY KEY CLUSTERED ([SurveyMarketId] ASC),
    CONSTRAINT [FK_dbo.SurveyMarket_dbo.Market_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [dbo].[Market] ([MarketId]),
    CONSTRAINT [FK_dbo.SurveyMarket_dbo.Survey_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_MarketId]
    ON [dbo].[SurveyMarket]([MarketId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[SurveyMarket]([SurveyId] ASC);

