
CREATE procedure [dbo].[proc_GetCBUSA_Users](@RawData xml)
as
declare @intCtr int
DECLARE @handle INT  
DECLARE @PrepareXmlStatus INT  
declare @XMLData xml
declare @RawDataVarchar varchar(max)
begin

	EXEC @PrepareXmlStatus = sp_xml_preparedocument @handle OUTPUT, @RawData 
	if exists(select * from sysobjects where name ='TmpUserData')
	drop table TmpUserData

	SELECT  * into TmpUserData
	FROM    OPENXML(@handle, 'root/BuilderAccountDetails', 2)  
		WITH (
		AEBuilderID bigint,
		AEUSERID bigINT,
		UserName varchar(1000),
		FirstName varchar(1500),
		LastName varchar(1500),
		MiddleName varchar(1500),
		Email varchar(1500)
		)  

	EXEC sp_xml_removedocument @handle 

	select @intCtr =  count(a.AEUSERID) 
	from
	(	
		select AEBuilderID, AEUSERID, FirstName, LastName, MiddleName, Email
		from TmpUserData 
		except
		select BuilderId, BuilderUserId, FirstName, LastName, MiddleName, Email
		from BuilderUser
	)a
	
	if @intCtr >0 
		declare @AEBuilderID bigint
		declare @AEUSERID bigINT
		declare @UserName varchar(1000)
		declare @FirstName varchar(1500)
		declare @LastName varchar(1500)
		declare @MiddleName varchar(1500)
		declare @Email varchar(1500)
		declare @RecordFound varchar(10)

		SET IDENTITY_INSERT dbo.BuilderUser on

		declare CurrUser CURSOR READ_ONLY
		for
		select a.AEBuilderID, a.AEUSERID, a.FirstName, a.LastName, a.MiddleName, a.Email,
		       case when b.BuilderUserId is null then 'False' else 'True' end  recordfound	
		from TmpUserData a inner join Builder v on a.AEBuilderID = v.BuilderId  left outer join BuilderUser b on a.AEUSERID = b.BuilderUserId and a.AEBuilderID = b.BuilderId 
		--where b.RowStatusId = 1

		open CurrUser

		Fetch Next From CurrUser Into @AEBuilderID, @AEUSERID, @FirstName, @LastName, @MiddleName, @Email, @RecordFound
		
		While @@Fetch_Status = 0 Begin
			--print @AEBuilderID
			--print @BuilderName			
			if @RecordFound = 'False'	
				insert into BuilderUser (BuilderId, BuilderUserId, FirstName, LastName, MiddleName, Email, RowStatusId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, RowGUID)
				select @AEBuilderID, @AEUSERID, @FirstName, @LastName, @MiddleName, @Email, 1, GETDATE(), 1, GETDATE(), 1, NEWID()
			else
				update BuilderUser 
				SET FirstName = @FirstName,
					LastName = @LastName,
					MiddleName = @MiddleName,
					Email = @Email,
					RowStatusId = 1,
					ModifiedOn = GETDATE()
				where BuilderId = @AEBuilderID
				  and BuilderUserId = @AEUSERID
			Fetch Next From CurrUser Into @AEBuilderID, @AEUSERID, @FirstName, @LastName, @MiddleName, @Email, @RecordFound
		End -- End of Fetch		
		Close CurrUser
		Deallocate CurrUser
		--drop table TmpUserData
		SET IDENTITY_INSERT dbo.BuilderUser off
		-- update tabale for inactive if not modified on current date
		update BuilderUser
		set RowStatusId = 2
		where convert(date,ModifiedOn,101) <> convert(date, getdate(),101)
end

