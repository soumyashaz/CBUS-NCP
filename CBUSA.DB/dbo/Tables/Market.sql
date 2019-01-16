CREATE TABLE [dbo].[Market] (
    [MarketId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [MarketName]  NVARCHAR (250)   NOT NULL,
    [RowStatusId] INT              NOT NULL,
    [CreatedOn]   DATETIME         NOT NULL,
    [CreatedBy]   INT              NOT NULL,
    [ModifiedOn]  DATETIME         NOT NULL,
    [ModifiedBy]  INT              NOT NULL,
    [RowGUID]     UNIQUEIDENTIFIER NOT NULL,
    [ZoneId]      INT              NOT NULL,
    CONSTRAINT [PK_dbo.Market] PRIMARY KEY CLUSTERED ([MarketId] ASC),
    CONSTRAINT [FK_dbo.Market_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId]),
    CONSTRAINT [FK_dbo.Market_dbo.Zone_Zone_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zone] ([ZoneId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Market]([RowStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[Market]([ZoneId] ASC);

