CREATE TABLE [dbo].[State] (
    [StateId]   INT            IDENTITY (1, 1) NOT NULL,
    [StateName] NVARCHAR (MAX) NULL,
    [IsActive]  INT            NOT NULL,
    CONSTRAINT [PK_dbo.State] PRIMARY KEY CLUSTERED ([StateId] ASC)
);

