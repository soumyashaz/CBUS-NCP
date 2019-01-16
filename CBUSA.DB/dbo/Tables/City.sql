CREATE TABLE [dbo].[City] (
    [CityId]   INT            IDENTITY (1, 1) NOT NULL,
    [CityName] NVARCHAR (MAX) NULL,
    [StateId]  INT            NOT NULL,
    [IsActive] INT            NOT NULL,
    CONSTRAINT [PK_dbo.City] PRIMARY KEY CLUSTERED ([CityId] ASC)
);

