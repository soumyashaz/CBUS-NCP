CREATE TABLE [dbo].[ContractBuilder] (
    [ContractBuilderId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [ContractId]        BIGINT           NOT NULL,
    [BuilderId]         BIGINT           NOT NULL,
    [JoiningDate]       DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ContractStatusId]  BIGINT           DEFAULT ((0)) NOT NULL,
    [RowStatusId]       INT              DEFAULT ((0)) NOT NULL,
    [CreatedOn]         DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [CreatedBy]         INT              DEFAULT ((0)) NOT NULL,
    [ModifiedOn]        DATETIME         DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ModifiedBy]        INT              DEFAULT ((0)) NOT NULL,
    [RowGUID]           UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    CONSTRAINT [PK_dbo.ContractBuilder] PRIMARY KEY CLUSTERED ([ContractBuilderId] ASC),
    CONSTRAINT [FK_dbo.ContractBuilder_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.ContractBuilder_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.ContractBuilder_dbo.ContractStatus_ContractStatusId] FOREIGN KEY ([ContractStatusId]) REFERENCES [dbo].[ContractStatus] ([ContractStatusId]),
    CONSTRAINT [FK_dbo.ContractBuilder_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[ContractBuilder]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[ContractBuilder]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContractStatusId]
    ON [dbo].[ContractBuilder]([ContractStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[ContractBuilder]([RowStatusId] ASC);

