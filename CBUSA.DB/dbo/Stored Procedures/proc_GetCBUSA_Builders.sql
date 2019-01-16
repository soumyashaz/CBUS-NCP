
CREATE procedure [dbo].[proc_GetCBUSA_Builders](@RawData xml)
as
declare @intCtr int
DECLARE @handle INT
DECLARE @PrepareXmlStatus INT
begin
	
	set nocount on

	--Select parent_ID,
 --              max(case when name='AEBuilderID' then convert(bigint,StringValue) else 0 end) as AEBuilderID,
	--		   max(case when name='AEBuilderHistoricID' then convert(bigint,StringValue) else 0 end) as AEBuilderHistoricID,
 --              max(case when name='BuilderName' then convert(Varchar(100),StringValue) else '' end) as BuilderName,
 --              max(case when name='PhoneNumber' then convert(Varchar(50),StringValue) else 0 end) as PhoneNumber,
	--		   max(case when name='BuilderEmail' then convert(Varchar(50),StringValue) else 0 end) as BuilderEmail,
	--		   max(case when name='BuilderMarket' then convert(Varchar(500),StringValue) else 0 end) as BuilderMarket
	--into #TmpBuilderData
	--from parseJSON(cast(@xmlOut as varchar(max)))
	--where ValueType = 'string'
	--group by parent_ID

	EXEC @PrepareXmlStatus = sp_xml_preparedocument @handle OUTPUT, @RawData 

	SELECT  * into #TmpBuilderData
	FROM    OPENXML(@handle, 'root/BuildersDetails', 2)  
		WITH (
		AEBuilderID bigINT,
		AEBuilderHistoricID bigint,
		BuilderName varchar(100),
		PhoneNumber varchar(15),
		BuilderEmail varchar(100),
		BuilderMarket varchar(100)
		)  

	EXEC sp_xml_removedocument @handle 

	select @intCtr =  count(a.AEBuilderID) 
	from
	(		
		select AEBuilderID, BuilderName, PhoneNumber, BuilderEmail, m.MarketId --, AEBuilderHistoricID
		from #TmpBuilderData a inner join Market m on a.BuilderMarket = m.MarketName
		except
		select BuilderID, BuilderName, PhoneNo, Email, MarketId --, BuilderHistoricID
		from Builder
	)a
	
	if @intCtr >0 
		declare @AEBuilderID bigint
		declare @BuilderName varchar(100)
		declare @PhoneNumber varchar(15)
		declare @BuilderEmail varchar(50)
		--declare @AEBuilderHistoricID varchar(50)
		declare @MarketId bigint
		declare @FirstName varchar(1000)
		declare @LastName varchar(1000)
		declare @MiddleName varchar(1000)
		declare @RecordFound varchar(5)

		SET IDENTITY_INSERT dbo.Builder on

		declare CurrBuilder CURSOR READ_ONLY
		for
		--select u.AEUSERID AEBuilderID, a.BuilderName, a.PhoneNumber, u.Email BuilderEmail, m.MarketId, u.FirstName, u.LastName, u.MiddleName,
		--       case when b.BuilderId is null then 'False' else 'True' end  recordfound --, AEBuilderHistoricID, BuilderMarket	
		--from #TmpBuilderData a inner join Market m on a.BuilderMarket = m.MarketName inner join TmpUserData u on a.AEBuilderID = u.AEBuilderID left outer join Builder b on u.AEUSERID = b.BuilderId
		--where m.RowStatusId =1
		select a.AEBuilderID, a.BuilderName, a.PhoneNumber, '' BuilderEmail, m.MarketId, '' FirstName, '' LastName, '' MiddleName,
		       case when b.BuilderId is null then 'False' else 'True' end  recordfound --, AEBuilderHistoricID, BuilderMarket	
		from #TmpBuilderData a inner join Market m on a.BuilderMarket = m.MarketName left outer join Builder b on a.AEBuilderID = b.BuilderId
		where m.RowStatusId =1
		open CurrBuilder

		Fetch Next From CurrBuilder Into @AEBuilderID, @BuilderName, @PhoneNumber, @BuilderEmail, @MarketId, @FirstName, @LastName, @MiddleName, @RecordFound
		
		While @@Fetch_Status = 0 Begin
			--print @AEBuilderID
			--print @BuilderName			
			if @RecordFound = 'False'	
				insert into Builder(BuilderID, BuilderName, FirstName, LastName, RowStatusId, PhoneNo, Email, MarketId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, RowGUID)
				select @AEBuilderID, @BuilderName, @FirstName, @LastName, 1 RowStatusId, @PhoneNumber, @BuilderEmail, @MarketId, GETDATE(), 1, GETDATE(), 1, NEWID()
			else
				update Builder 
				SET BuilderName = @BuilderName,
					FirstName = @FirstName,
					LastName = @LastName,
					PhoneNo = @PhoneNumber,
					Email = @BuilderEmail,
					MarketId = @MarketId,
					RowStatusId = 1,
					ModifiedOn = GETDATE()
				where BuilderID = @AEBuilderID
			Fetch Next From CurrBuilder Into @AEBuilderID, @BuilderName, @PhoneNumber, @BuilderEmail, @MarketId, @FirstName, @LastName, @MiddleName, @RecordFound
		End -- End of Fetch		
		Close CurrBuilder
		Deallocate CurrBuilder
		drop table #TmpBuilderData
		drop table TmpUserData
		SET IDENTITY_INSERT dbo.Builder off

		-- update tabale for inactive if not modified on current date
		update Builder
		set RowStatusId = 2
		where convert(date,ModifiedOn,101) <> convert(date, getdate(),101)

end


