CREATE TABLE [dbo].[QuarterEmailTemplate]
(
	[QuarterEmailTemplateId] BIGINT NOT NULL PRIMARY KEY, 
    [QuaterId] BIGINT NOT NULL, 
    [InvitationEmailTemplate] NVARCHAR(MAX) NULL, 
    [ReminderEmailTemplate] NVARCHAR(MAX) NULL, 
    [InvitationEmailSubject] NVARCHAR(250) NULL, 
    [ReminderEmailSubject] NVARCHAR(250) NULL, 
    CONSTRAINT [FK_QuarterEmailTemplate_ToTable] FOREIGN KEY ([QuaterId]) REFERENCES [Quater]([QuaterId])
)
