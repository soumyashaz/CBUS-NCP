CREATE TABLE [dbo].[ContractRebateBuilder] (
    [ContractRebateBuilderId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [ContractId]              BIGINT           NOT NULL,
    [BuilderId]               BIGINT           NOT NULL,
    [RebatePercentage]        DECIMAL (18, 2)  NOT NULL,
    [RowStatusId]             INT              DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [CreatedBy]               INT              DEFAULT ((0)) NOT NULL,
    [ModifiedOn]              DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ModifiedBy]              INT              DEFAULT ((0)) NOT NULL,
    [RowGUID]                 UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    CONSTRAINT [PK_dbo.ContractRebateBuilder] PRIMARY KEY CLUSTERED ([ContractRebateBuilderId] ASC),
    CONSTRAINT [FK_dbo.ContractRebateBuilder_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.ContractRebateBuilder_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.ContractRebateBuilder_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[ContractRebateBuilder]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[ContractRebateBuilder]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[ContractRebateBuilder]([RowStatusId] ASC);

