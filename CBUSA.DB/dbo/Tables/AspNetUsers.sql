CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[MiddleInit] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Gender] [nvarchar](max) NULL,
	[DOB] [datetime2](7) NOT NULL,
	[ContactUsOnBehalf] [nvarchar](max) NULL,
	[CompanyTypeName] [nvarchar](max) NULL,
	[AddressLine1] [nvarchar](max) NULL,
	[AddressLine2] [nvarchar](max) NULL,
	[Zip] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[NoteBox] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[RowStatusId] [int] NOT NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[IsEulaChecked] [bit] NOT NULL DEFAULT ((0)),
	[Fax] [nvarchar](max) NULL,
	[SSN] [nvarchar](max) NULL,
	[DrivingLic] [nvarchar](max) NULL,
	[Other] [nvarchar](max) NULL,
	[IsSecondTimeLogin] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUsers_dbo.RowStatus_RowStatusId] FOREIGN KEY([RowStatusId])
REFERENCES [dbo].[RowStatus] ([RowStatusId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_dbo.AspNetUsers_dbo.RowStatus_RowStatusId]
GO