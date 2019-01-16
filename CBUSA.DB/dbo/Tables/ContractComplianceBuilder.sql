CREATE TABLE [dbo].[ContractComplianceBuilder] (
    [ContractComplianceBuilderId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [EstimatedValue]              DECIMAL (18, 2) NOT NULL,
    [BuilderId]                   BIGINT          NOT NULL,
    [ContractId]                  BIGINT          NOT NULL,
    [ActualValue]                 DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.ContractComplianceBuilder] PRIMARY KEY CLUSTERED ([ContractComplianceBuilderId] ASC),
    CONSTRAINT [FK_dbo.ContractComplianceBuilder_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.ContractComplianceBuilder_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[ContractComplianceBuilder]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[ContractComplianceBuilder]([ContractId] ASC);

