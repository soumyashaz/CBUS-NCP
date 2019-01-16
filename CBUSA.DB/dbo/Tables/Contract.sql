CREATE TABLE [dbo].[Contract] (
    [ContractId]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [ContractName]         NVARCHAR (150)   NOT NULL,
    [Label]                NVARCHAR (150)   NOT NULL,
    [EstimatedStartDate]   DATETIME         NULL,
    [EntryDeadline]        DATETIME         NULL,
    [ContrctFrom]          DATETIME         NULL,
    [ContrctTo]            DATETIME         NULL,
    [ContractDeliverables] NVARCHAR (MAX)   NULL,
    [PrimaryManufacturer]  NVARCHAR (MAX)   NULL,
    [ManufacturerId]       BIGINT           NULL,
    [Website]              NVARCHAR (MAX)   NULL,
    [ContractStatusId]     BIGINT           NOT NULL,
    [RowStatusId]          INT              NOT NULL,
    [CreatedOn]            DATETIME         NOT NULL,
    [CreatedBy]            INT              NOT NULL,
    [ModifiedOn]           DATETIME         NOT NULL,
    [ModifiedBy]           INT              NOT NULL,
    [RowGUID]              UNIQUEIDENTIFIER NOT NULL,
    [ContractIcon]         VARBINARY (MAX)  NULL,
    [IsReportable] BIT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.Contract] PRIMARY KEY CLUSTERED ([ContractId] ASC),
    CONSTRAINT [FK_dbo.Contract_dbo.ContractStatus_ContractStatusId] FOREIGN KEY ([ContractStatusId]) REFERENCES [dbo].[ContractStatus] ([ContractStatusId]),
    CONSTRAINT [FK_dbo.Contract_dbo.Manufacturer_ManufacturerId] FOREIGN KEY ([ManufacturerId]) REFERENCES [dbo].[Manufacturer] ([ManufacturerId]),
    CONSTRAINT [FK_dbo.Contract_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractStatusId]
    ON [dbo].[Contract]([ContractStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ManufacturerId]
    ON [dbo].[Contract]([ManufacturerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Contract]([RowStatusId] ASC);

