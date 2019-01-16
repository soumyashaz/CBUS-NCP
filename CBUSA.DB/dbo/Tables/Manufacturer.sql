CREATE TABLE [dbo].[Manufacturer] (
    [ManufacturerId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [ManufacturerName] NVARCHAR (150)   NOT NULL,
    [RowStatusId]      INT              NOT NULL,
    [CreatedOn]        DATETIME         NOT NULL,
    [CreatedBy]        INT              NOT NULL,
    [ModifiedOn]       DATETIME         NOT NULL,
    [ModifiedBy]       INT              NOT NULL,
    [RowGUID]          UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.Manufacturer] PRIMARY KEY CLUSTERED ([ManufacturerId] ASC),
    CONSTRAINT [FK_dbo.Manufacturer_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Manufacturer]([RowStatusId] ASC);

