CREATE TABLE [dbo].[Resource] (
    [ResourceId]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [ResourceCategoryId] BIGINT           NOT NULL,
    [FileLocation]       NVARCHAR (250)   NULL,
    [FileName]           NVARCHAR (250)   NULL,
    [Title]              NVARCHAR (150)   NOT NULL,
    [Description]        NVARCHAR (300)   NULL,
    [ContractId]         BIGINT           NULL,
    [DumpId]             NVARCHAR (MAX)   NULL,
    [RowStatusId]        INT              CONSTRAINT [DF__Resource__RowSta__29221CFB] DEFAULT ((0)) NOT NULL,
    [CreatedOn]          DATETIME         CONSTRAINT [DF__Resource__Create__2A164134] DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [CreatedBy]          INT              CONSTRAINT [DF__Resource__Create__2B0A656D] DEFAULT ((0)) NOT NULL,
    [ModifiedOn]         DATETIME         CONSTRAINT [DF__Resource__Modifi__2BFE89A6] DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ModifiedBy]         INT              CONSTRAINT [DF__Resource__Modifi__2CF2ADDF] DEFAULT ((0)) NOT NULL,
    [RowGUID]            UNIQUEIDENTIFIER CONSTRAINT [DF__Resource__RowGUI__2DE6D218] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    CONSTRAINT [PK_dbo.Resource] PRIMARY KEY CLUSTERED ([ResourceId] ASC),
    CONSTRAINT [FK_dbo.Resource_dbo.Contract_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_dbo.Resource_dbo.ResourceCategory_ResourceCategoryId] FOREIGN KEY ([ResourceCategoryId]) REFERENCES [dbo].[ResourceCategory] ([ResourceCategoryId]),
    CONSTRAINT [FK_dbo.Resource_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractId]
    ON [dbo].[Resource]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResourceCategoryId]
    ON [dbo].[Resource]([ResourceCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Resource]([RowStatusId] ASC);

