CREATE TABLE [dbo].[ContractStatusHistory] (
    [ContractStatusHistoryId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [ContractId]              BIGINT   NOT NULL,
    [ContractStatusId]        BIGINT   NOT NULL,
    [EntryDate]               DATETIME NULL,
    CONSTRAINT [PK_dbo.ContractStatusHistory] PRIMARY KEY CLUSTERED ([ContractStatusHistoryId] ASC),
    CONSTRAINT [FK_dbo.ContractStatusHistory_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.ContractStatusHistory_dbo.ContractStatus_ContractStatusId] FOREIGN KEY ([ContractStatusId]) REFERENCES [dbo].[ContractStatus] ([ContractStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[ContractStatusHistory]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContractStatus_ContractStatusId]
    ON [dbo].[ContractStatusHistory]([ContractStatusId] ASC);

