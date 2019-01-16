CREATE TABLE [dbo].[Holiday](
	[HolidayID] [bigint] IDENTITY(1,1) NOT NULL,
	[HolidayDate] [datetime] NOT NULL,
	[Year] [bigint] NOT NULL,
 CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED 
(
	[HolidayID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)