
--exec DataProcJoinBuilderToContract 23,44,24131
CREATE Proc [dbo].[DataProcJoinBuilderToContract]
(
  @ContractId bigint,
  @SurveyId bigint,
  @BuilderId bigint
 
)
as
begin
	
	declare @ContractStatusId BigInt
	declare @ComplianceQuestionId bigint
	declare @ComplianceQuestionValue nvarchar(max)
	declare @Answer varchar(100)
	declare @GridRow int
	declare @GridColoumn int
	declare  @IsJoin varchar(50)
	declare @ContractCurrentStatus nvarchar(100)
	declare @BuilderName nvarchar(500)
	set @BuilderName=(select BuilderName from Builder where BuilderId=@BuilderId)
	set @ComplianceQuestionValue=(select QuestionText from TblEnrollmentSurveyAnswer where SurveyId=@SurveyId and [Company Name]=@BuilderName)
	--print @ComplianceQuestionValue
	set @ComplianceQuestionId=(select QuestionId from Question where QuestionValue=@ComplianceQuestionValue and RowStatusId=1 and SurveyId=@SurveyId)
	--print @SurveyId
	--print @BuilderId
	--print @ComplianceQuestionId
	if(@ComplianceQuestionId is not null)
	 begin

				declare @Questionid bigint,@QuestionTypeId bigint
				declare @Questiontable table(Id int identity(1,1), QuestionId bigint,QuestionTypeId bigint)
 			 insert into @Questiontable (QuestionId,QuestionTypeId)
					select QuestionId,QuestionTypeId from Question 
						where SurveyId=@SurveyId and RowStatusId=1

			if((select count(*) from @Questiontable)>0)
			  begin


			  
				
				declare @i int =1
				declare @count int =(select count(*) from @Questiontable)

				declare @iR int =0
				declare @iC int =0

				while(@i <=@count)
				begin
					select @Questionid=QuestionId,@QuestionTypeId=QuestionTypeId from @Questiontable where Id=@i
					set @Answer=''
					if(@ComplianceQuestionId=@Questionid)
					begin
					  set @Answer=(select QuestionValue from TblEnrollmentSurveyAnswer where SurveyId=@SurveyId and [Company Name]=@BuilderName)
					end
					if(@QuestionTypeId=1)  -- TextBox
					begin
		
					insert into SurveyResult ([Answer],[RowNumber],[ColumnNumber],[QuestionId],[RowStatusId],
					[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[RowGUID],[SurveyId],[BuilderId])
					values (@Answer,0,0,@Questionid,1,GETDATE(),1,GETDATE(),1,NEWID(),@SurveyId,@BuilderId)

					--print 'ssss'
					end
					else if(@QuestionTypeId=2) -- drop down
					Begin

					insert into SurveyResult ([Answer],[RowNumber],[ColumnNumber],[QuestionId],[RowStatusId],
					[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[RowGUID],[SurveyId],[BuilderId])
					values ('',0,0,@Questionid,1,GETDATE(),1,GETDATE(),1,NEWID(),@SurveyId,@BuilderId)
					--print 'ddd'
					End
					else   -- Grid
					Begin

						set @GridRow=(select [Row] from QuestionGridSetting where QuestionId=@Questionid)
						set @GridColoumn=(select [Column] from QuestionGridSetting where QuestionId=@Questionid)
			
						set @iR=0
			
			
						while(@iR<@GridRow)
						begin
							set @iC=0
							 while(@iC<@GridColoumn)
							 begin
				  
								insert into SurveyResult ([Answer],[RowNumber],[ColumnNumber],[QuestionId],[RowStatusId],
								[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[RowGUID],[SurveyId],[BuilderId])
								values ('',@iR,@iC,@Questionid,1,GETDATE(),1,GETDATE(),1,NEWID(),@SurveyId,@BuilderId)

								

							 set @iC=@iC+1
							 end
						set @iR=@iR+1
						end




					End

					set @i=@i+1
				end


				--- Survey Builder 

			if not exists (select 1 from SurveyBuilder where SurveyId=@SurveyId and BuilderId=@BuilderId)
			 begin
				
				insert Into SurveyBuilder ([SurveyStartDate],[SurveyId],[BuilderId],[IsSurveyCompleted],[SurveyCompleteDate],[RowStatusId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[RowGUID])
				values (GETDATE(),@SurveyId,@BuilderId,1,GETDATE(),1,GETDATE(),1,getdate(),1,NEWID())


			 end
				

				---

				-- Join In Contract
	
				set @IsJoin= (select IsJoin from TblContractBuilderAssociation where [Survey ID]=@SurveyId and  [Company Name]=@BuilderName and [Contract ID]=@ContractId)
				set @ContractCurrentStatus= (select [Contract State] from TblContractBuilderAssociation where [Survey ID]=@SurveyId and  [Company Name]=@BuilderName and [Contract ID]=@ContractId)
				set @ContractStatusId =(select ContractStatusId from ContractStatus where ContractStatusName=@ContractCurrentStatus)
			
			if(LOWER(@IsJoin)='yes')
				begin

				--Print 'Contract Builder'
	  if not exists (select 1 from ContractBuilder where ContractId=@ContractId and BuilderId=@BuilderId)
			 begin
				  insert into ContractBuilder([ContractId],[BuilderId],[JoiningDate],[ContractStatusId],[RowStatusId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[RowGUID])
				  values (@ContractId,@BuilderId,GETDATE(),@ContractStatusId,1,GETDATE(),1,GETDATE(),1,NEWID())
			end
				end
				else
				begin

			--	Print 'Contract Builder'
			 if not exists (select 1 from ContractBuilder where ContractId=@ContractId and BuilderId=@BuilderId)
			 begin
				 insert into ContractBuilder([ContractId],[BuilderId],[JoiningDate],[ContractStatusId],[RowStatusId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[RowGUID])
				  values (@ContractId,@BuilderId,GETDATE(),@ContractStatusId,2,GETDATE(),1,GETDATE(),1,NEWID())
			end	
				end
			end
	end

 end



