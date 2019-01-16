CREATE TABLE [dbo].[BuilderQuaterAdminReport] (
    [BuilderQuaterAdminReportId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [BuilderId]                  BIGINT           NOT NULL,
    [QuaterId]                   BIGINT           NOT NULL,
    [SubmitDate]                 DATETIME         NOT NULL,
    [IsSubmit]                   BIT              NOT NULL,
    [RowStatusId]                INT              NOT NULL,
    [CreatedOn]                  DATETIME         NOT NULL,
    [CreatedBy]                  INT              NOT NULL,
    [ModifiedOn]                 DATETIME         NOT NULL,
    [ModifiedBy]                 INT              NOT NULL,
    [RowGUID]                    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.BuilderQuaterAdminReport] PRIMARY KEY CLUSTERED ([BuilderQuaterAdminReportId] ASC),
    CONSTRAINT [FK_dbo.BuilderQuaterAdminReport_dbo.Builder_BuilderId] FOREIGN KEY ([BuilderId]) REFERENCES [dbo].[Builder] ([BuilderId]),
    CONSTRAINT [FK_dbo.BuilderQuaterAdminReport_dbo.Quater_QuaterId] FOREIGN KEY ([QuaterId]) REFERENCES [dbo].[Quater] ([QuaterId]),
    CONSTRAINT [FK_dbo.BuilderQuaterAdminReport_dbo.RowStatus_RowStatusId] FOREIGN KEY ([RowStatusId]) REFERENCES [dbo].[RowStatus] ([RowStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BuilderId]
    ON [dbo].[BuilderQuaterAdminReport]([BuilderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_QuaterId]
    ON [dbo].[BuilderQuaterAdminReport]([QuaterId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RowStatusId]
    ON [dbo].[BuilderQuaterAdminReport]([RowStatusId] ASC);

