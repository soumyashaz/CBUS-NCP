CREATE TABLE [dbo].[Project] (
    [ProjectId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [ProjectName] NVARCHAR (MAX)   NULL,
    [LotNo]       NVARCHAR (MAX)   NULL,
    [Address]     NVARCHAR (MAX)   NULL,
    [Zip]         NVARCHAR (50)    NULL,
    [BuilderId]   BIGINT           CONSTRAINT [DF__Project__Builder__2917FB5A] DEFAULT ((0)) NOT NULL,
    [RowStatusId] INT              CONSTRAINT [DF__Project__RowStat__2A0C1F93] DEFAULT ((0)) NOT NULL,
    [CreatedOn]   DATETIME         CONSTRAINT [DF__Project__Created__2B0043CC] DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [CreatedBy]   INT              CONSTRAINT [DF__Project__Created__2BF46805] DEFAULT ((0)) NOT NULL,
    [ModifiedOn]  DATETIME         CONSTRAINT [DF__Project__Modifie__2CE88C3E] DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ModifiedBy]  INT              CONSTRAINT [DF__Project__Modifie__2DDCB077] DEFAULT ((0)) NOT NULL,
    [RowGUID]     UNIQUEIDENTIFIER CONSTRAINT [DF__Project__RowGUID__2ED0D4B0] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [State]       NVARCHAR (250)   NULL,
    [City]        NVARCHAR (250)   NULL,
    CONSTRAINT [PK_dbo.Project] PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_dbo.Project_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.Project_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[Project]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[Project]([RowStatusId] ASC);

