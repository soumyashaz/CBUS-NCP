SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChallengeQuestion](
	[ChallengeQuestionId] [int] IDENTITY(1,1) NOT NULL,
	[ChallengeQuestionDescription] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowStatusId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ChallengeQuestion] PRIMARY KEY CLUSTERED 
(
	[ChallengeQuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ChallengeQuestion]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ChallengeQuestion_dbo.RowStatus_RowStatusId] FOREIGN KEY([RowStatusId])
REFERENCES [dbo].[RowStatus] ([RowStatusId])
GO

ALTER TABLE [dbo].[ChallengeQuestion] CHECK CONSTRAINT [FK_dbo.ChallengeQuestion_dbo.RowStatus_RowStatusId]
GO