
CREATE procedure [dbo].[proc_GetCBUSA_Market](@RawData xml)
as
declare @intCtr int
DECLARE @handle INT  
DECLARE @PrepareXmlStatus INT  
begin	
	set nocount on
	
	--Select parent_ID,
 --              max(case when name='AELLCID' then convert(bigint,StringValue) else 0 end) as AELLCID,
 --              max(case when name='LLCName' then convert(Varchar(50),StringValue) else '' end) as LLCName,
 --              max(case when name='IsActive' then convert(varchar(50),StringValue) else '' end) as IsActive
	--into #TempMarket  
	--from parseJSON(cast(@xmlOut as varchar(max)))
	--where ValueType = 'string'
	--group by parent_ID

	EXEC @PrepareXmlStatus = sp_xml_preparedocument @handle OUTPUT, @RawData 

	SELECT  * into #TempMarket
	FROM    OPENXML(@handle, 'root/LLCDetails', 2)  
		WITH (
		AELLCID bigINT,
		LLCName varchar(1000),
		IsActive varchar(50)
		)  

	EXEC sp_xml_removedocument @handle 

	select @intCtr =  count(a.AELLCID) 
	from
	(	
		select AELLCID, LLCName, case when lower(IsActive) ='true' then 1 else 0 end IsActive
		from #TempMarket 
		except
		select MarketId, MarketName, RowStatusId
		from Market
	)a
	
	if @intCtr >0 
		declare @AELLCID bigint
		declare @LLCName varchar(100)
		declare @IsActive varchar(15)
		declare @MarketId bigint
		declare @RecordFound varchar(5)

		SET IDENTITY_INSERT dbo.Market on

		declare CurrMarket CURSOR READ_ONLY
		for
		select a.AELLCID, a.LLCName, case when lower(a.IsActive) ='true' then 1 else 0 end IsActive, 
		       case when b.MarketId is null then 'False' else 'True' end  recordfound	
		from #TempMarket a left outer join Market b on a.AELLCID = b.MarketId
		
		open CurrMarket

		Fetch Next From CurrMarket Into @AELLCID, @LLCName, @IsActive, @RecordFound
		
		While @@Fetch_Status = 0 Begin
			--print @AEBuilderID
			--print @BuilderName			
			if @RecordFound = 'False'	
				insert into Market (MarketId, MarketName, RowStatusId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, RowGUID, ZoneId)
				select @AELLCID, @LLCName, @IsActive, getdate(), 1, GETDATE(), 1,  NEWID(), 4
			else
				update Market 
				SET MarketName = @LLCName,
					RowStatusId = @IsActive,
					ModifiedOn = GETDATE()
				where MarketId = @AELLCID
			Fetch Next From CurrMarket Into @AELLCID, @LLCName, @IsActive, @RecordFound
		End -- End of Fetch		
		Close CurrMarket
		Deallocate CurrMarket
		drop table #TempMarket
		SET IDENTITY_INSERT dbo.Market off
		-- update tabale for inactive if not modified on current date
		update Market
		set RowStatusId = 2
		where convert(date,ModifiedOn,101) <> convert(date, getdate(),101)
end



