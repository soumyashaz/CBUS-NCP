CREATE TABLE [dbo].[BuilderUser] (
    [BuilderUserId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [Email]         NVARCHAR (100)   NOT NULL,
    [FirstName]     NVARCHAR (100)   NOT NULL,
    [MiddleName]    NVARCHAR (100)   NOT NULL,
    [LastName]      NVARCHAR (100)   NOT NULL,
    [BuilderId]     BIGINT           NOT NULL,
    [RowStatusId]   INT              NOT NULL,
    [CreatedOn]     DATETIME         NOT NULL,
    [CreatedBy]     INT              NOT NULL,
    [ModifiedOn]    DATETIME         NOT NULL,
    [ModifiedBy]    INT              NOT NULL,
    [RowGUID]       UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.BuilderUser] PRIMARY KEY CLUSTERED ([BuilderUserId] ASC),
    CONSTRAINT [FK_dbo.BuilderUser_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.BuilderUser_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);

