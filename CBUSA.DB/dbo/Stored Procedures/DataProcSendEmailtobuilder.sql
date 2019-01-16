
CREATE Proc [dbo].[DataProcSendEmailtobuilder]
(
  @ContractId bigint,
  @SurveyId bigint
  --@IsJoin varchar(50)
)
as
begin

	declare @Buildertable table(Id int identity(1,1), BuilderName nvarchar(500))
	declare @Buildermarket bigint,@BuilderId bigint,@BuilderName nvarchar(500)
	declare @GroupId uniqueidentifier

	insert into @Buildertable (BuilderName)
	select [Company Name] from TblContractBuilderAssociation where [Survey ID]=@SurveyId and [Contract ID]=@ContractId

	declare @i int =1
	declare @count int =(select count(*) from @Buildertable)

	while(@i <=@count)
	begin
	set @BuilderName=(select BuilderName from @Buildertable where Id=@i)
	set @BuilderId=(select BuilderId from Builder where BuilderName=@BuilderName)
	set @Buildermarket=(select MarketId from Builder where BuilderId=@BuilderId)
			if(@Buildermarket is not null)
			begin
				---SurveyMarket
				if not exists( select 1 from SurveyMarket where SurveyId=@SurveyId and MarketId=@Buildermarket)
				  begin
					insert into SurveyMarket(SurveyId,MarketId) values(@SurveyId,@Buildermarket)
				  end
				  ----End
				
				  --BuilderUserSurveyEmailSent
				  set @GroupId=NEWID()
				  insert into BuilderUserSurveyEmailSent (SurveyId,BuilderId,BuilderUserId,GroupId,Senddate,SendBy,IsMailSent)
					  select @SurveyId,@BuilderId,BuilderUserId,@GroupId,GETDATE(),1,1 from BuilderUser
					  where BuilderUserId not in 
					  (
							select BuilderUserId
							 from BuilderUserSurveyEmailSent where SurveyId=@SurveyId and BuilderId=@BuilderId
					  ) and BuilderId=@BuilderId

				  --End

				  ----BuilderSurveyEmailSent
				  insert into BuilderSurveyEmailSent(SurveyId,BuilderId,GroupId,SendDate,SendBy,IsMailSent)
				  values(@SurveyId,@BuilderId,@GroupId,GETDATE(),1,1)
				  ---End

				  exec [dbo].DataProcJoinBuilderToContract @ContractId,@SurveyId,@BuilderId



			end
	set @i=@i+1
	end
End




