

CREATE procedure [dbo].[proc_GetCBUSA_Manufacturer](@RawData xml)
as
declare @intCtr int
DECLARE @handle INT  
DECLARE @PrepareXmlStatus INT  
declare @XMLData xml
declare @RawDataVarchar varchar(max)
begin

	EXEC @PrepareXmlStatus = sp_xml_preparedocument @handle OUTPUT, @RawData 

	SELECT  * into #TmpManufacturerData
	FROM    OPENXML(@handle, 'root/ManufacturersDetails', 2)  
		WITH (
		AEManufacturerID bigINT,
		HistoricID bigint,
		ClassID varchar(1500),
		CompanyName varchar(1500),
		MailingAddress varchar(1500),
		MailingCity varchar(1500),
		MailingState varchar(1500),
		MailingZip varchar(1500),
		Website varchar(1500),
		PrimaryContactName varchar(1500),
		PrimaryContactEmail varchar(1500),
		PrimaryContactPhone varchar(1500),
		APContactName varchar(1500),
		APContactEmail varchar(1500),
		APContactPhone varchar(1500),
		PaymentTerms varchar(1500)
		)  

	EXEC sp_xml_removedocument @handle 

	select @intCtr =  count(a.AEManufacturerID) 
	from
	(	
		select AEManufacturerID, APContactName
		from #TmpManufacturerData 
		except
		select ManufacturerId, ManufacturerName
		from Manufacturer
	)a
	
	if @intCtr >0 
		declare @AEManufacturerID bigint
		declare @CompanyName varchar(100)
		declare @HistoricId bigint
		declare @IsActive varchar(15)
		declare @MarketId bigint
		declare @RecordFound varchar(5)

		SET IDENTITY_INSERT dbo.Manufacturer on

		declare CurrManufacturer CURSOR READ_ONLY
		for
		select a.AEManufacturerID, a.CompanyName, --a.HistoricID,
		       case when b.ManufacturerId is null then 'False' else 'True' end  recordfound	
		from #TmpManufacturerData a left outer join Manufacturer b on a.AEManufacturerID = b.ManufacturerId
		
		open CurrManufacturer

		Fetch Next From CurrManufacturer Into @AEManufacturerID, @CompanyName, @RecordFound
		
		While @@Fetch_Status = 0 Begin
			--print @AEBuilderID
			--print @BuilderName			
			if @RecordFound = 'False'	
				insert into Manufacturer (ManufacturerId, ManufacturerName, RowStatusId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, RowGUID)
				select @AEManufacturerID, @CompanyName, 1, getdate(), 1, GETDATE(), 1,  NEWID()
			else
				update Manufacturer 
				SET ManufacturerName = @CompanyName,
					--HistoricID = @HistoricId,
					RowStatusId = 1,
					ModifiedOn = GETDATE()
				where ManufacturerId = @AEManufacturerID
			Fetch Next From CurrManufacturer Into @AEManufacturerID, @CompanyName, @RecordFound
		End -- End of Fetch
		Close CurrManufacturer
		Deallocate CurrManufacturer
		drop table #TmpManufacturerData
		SET IDENTITY_INSERT dbo.Manufacturer off

		-- update tabale for inactive if not modified on current date
		update Manufacturer
		set RowStatusId = 2
		where convert(date,ModifiedOn,101) <> convert(date, getdate(),101)
end




