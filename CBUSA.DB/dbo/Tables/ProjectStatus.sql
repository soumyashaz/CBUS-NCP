CREATE TABLE [dbo].[ProjectStatus] (
    [ProjectStatusId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [StatusName]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ProjectStatus] PRIMARY KEY CLUSTERED ([ProjectStatusId] ASC)
);

