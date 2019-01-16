
--exec [DataProcContractSurvey]
CREATE proc [dbo].[DataProcContractSurvey]
as
begin
SET NOCOUNT ON;
Declare @ContractId bigint,@ContractName nvarchar(300),@SurveyId bigint,@SurveyName nvarchar(300),@BuilderId bigint
Declare @IsSurveyPublish bit
--Declare @ContractName nvarchar(300),@SurveyName nvarchar(300)
DECLARE cursor__contract CURSOR
	LOCAL SCROLL STATIC
	FOR
	
	select [Contract Name],[Survey Name] from TblContractBuilderAssociation
	group by [Contract Name],[Survey Name]
	--,[Builder ID]

	OPEN cursor__contract
	IF @@CURSOR_ROWS>0
	BEGIN
		FETCH NEXT FROM cursor__contract  INTO	
		@ContractName,@SurveyName
		WHILE @@FETCH_STATUS=0
		BEGIN
		
			set @ContractId=(select ContractId from Contract where ContractName=@ContractName and RowStatusId=1)
			if(@ContractId is not null)
			begin
				set  @SurveyId=(select SurveyId from Survey where SurveyName=@SurveyName and RowStatusId=1 and IsEnrolment=1 and ContractId=@ContractId)
						if(@SurveyId is not null)
						begin
						  
						  set @IsSurveyPublish=(select IsPublished from Survey where SurveyId=@SurveyId and RowStatusId=1 and IsEnrolment=1 and ContractId=@ContractId)

						  if(@IsSurveyPublish=0)
						  begin
						  update Survey set IsPublished=1,ModifiedOn=GETDATE(),Publishdate=GETDATE() where SurveyId=@SurveyId
						  end
						  update TblContractBuilderAssociation set [Survey ID]=@SurveyId,
						  [Contract ID]=@ContractId where [Survey Name]=@SurveyName and [Contract Name]=@ContractName

						  update TblEnrollmentSurveyAnswer set [SurveyID]=@SurveyId
						  where [SurveyName]=@SurveyName 


						  exec [dbo].DataProcSendEmailtobuilder @ContractId,@SurveyId


						end
			end
    	FETCH NEXT FROM cursor__contract  INTO	
		@ContractName,@SurveyName
		End
  	
	end


 CLOSE cursor__contract
 DEALLOCATE cursor__contract
	
	print 'Sucess'

End

