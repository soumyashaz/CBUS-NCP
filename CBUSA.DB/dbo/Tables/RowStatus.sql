CREATE TABLE [dbo].[RowStatus] (
    [RowStatusId]          INT              NOT NULL,
    [RowStatusDescription] NVARCHAR (MAX)   NULL,
    [RowGUID]              UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.RowStatus] PRIMARY KEY CLUSTERED ([RowStatusId] ASC)
);

