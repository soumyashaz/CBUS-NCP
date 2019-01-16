CREATE TABLE [dbo].[Builder] (
    [BuilderId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [BuilderName] NVARCHAR (100)   NOT NULL,
    [FirstName]   NVARCHAR (100)   NOT NULL,
    [LastName]    NVARCHAR (100)   NOT NULL,
    [PhoneNo]     NVARCHAR (15)    NULL,
    [Email]       NVARCHAR (50)    NULL,
    [MarketId]    BIGINT           NOT NULL,
    [RowStatusId] INT              NOT NULL,
    [CreatedOn]   DATETIME         NOT NULL,
    [CreatedBy]   INT              NOT NULL,
    [ModifiedOn]  DATETIME         NOT NULL,
    [ModifiedBy]  INT              NOT NULL,
    [RowGUID]     UNIQUEIDENTIFIER NOT NULL,
    [HistoricId]  BIGINT           NOT NULL,
    CONSTRAINT [PK_dbo.Builder] PRIMARY KEY CLUSTERED ([BuilderId] ASC),
    CONSTRAINT [FK_dbo.Builder_dbo.Market_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [dbo].[Market] ([MarketId]),
    CONSTRAINT [FK_dbo.Builder_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_MarketId]
    ON [dbo].[Builder]([MarketId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Builder]([RowStatusId] ASC);

