CREATE TABLE [dbo].[Zone] (
    [ZoneId]      INT              IDENTITY (1, 1) NOT NULL,
    [ZoneName]    NVARCHAR (250)   NOT NULL,
    [RowStatusId] INT              NOT NULL,
    [CreatedOn]   DATETIME         NOT NULL,
    [CreatedBy]   INT              NOT NULL,
    [ModifiedOn]  DATETIME         NOT NULL,
    [ModifiedBy]  INT              NOT NULL,
    [RowGUID]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.Zone] PRIMARY KEY CLUSTERED ([ZoneId] ASC),
    CONSTRAINT [FK_dbo.Zone_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Zone]([RowStatusId] ASC);

