CREATE TABLE [dbo].[CBUSAIntegrationStatus] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [StartTime]    DATETIME      NOT NULL,
    [EndTime]      DATETIME      NULL,
    [Status]       VARCHAR (50)  NOT NULL,
    [ErrorDetails] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CBUSAIntegrationStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

