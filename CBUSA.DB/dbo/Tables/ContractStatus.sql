CREATE TABLE [dbo].[ContractStatus] (
    [ContractStatusId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [ContractStatusName] NVARCHAR (MAX)   NULL,
    [RowStatusId]        INT              NOT NULL,
    [CreatedOn]          DATETIME         NOT NULL,
    [CreatedBy]          INT              NOT NULL,
    [ModifiedOn]         DATETIME         NOT NULL,
    [ModifiedBy]         INT              NOT NULL,
    [RowGUID]            UNIQUEIDENTIFIER NOT NULL,
    [IsNonEditable]      BIT              CONSTRAINT [DF__ContractS__IsNon__2739D489] DEFAULT ((0)) NOT NULL,
    [Order]              INT              CONSTRAINT [DF__ContractS__Order__282DF8C2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.ContractStatus] PRIMARY KEY CLUSTERED ([ContractStatusId] ASC),
    CONSTRAINT [FK_dbo.ContractStatus_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[ContractStatus]([RowStatusId] ASC);

