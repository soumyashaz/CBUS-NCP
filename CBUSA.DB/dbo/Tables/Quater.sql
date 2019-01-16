CREATE TABLE [dbo].[Quater] (
    [QuaterId]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [QuaterName] NVARCHAR (MAX) NULL,
    [StartDate]  DATETIME       NOT NULL,
    [EndDate]    DATETIME       NOT NULL,
    [Year]       BIGINT         DEFAULT ((0)) NOT NULL,
    [ReportingStartDate] DATETIME NOT NULL, 
    [ReportingEndDate] DATETIME NOT NULL, 
    [BuilderReportingEndDate] DATETIME NOT NULL, 
    CONSTRAINT [PK_dbo.Quater] PRIMARY KEY CLUSTERED ([QuaterId] ASC)
);

