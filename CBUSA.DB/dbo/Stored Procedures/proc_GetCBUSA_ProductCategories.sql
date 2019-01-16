

CREATE procedure [dbo].[proc_GetCBUSA_ProductCategories](@RawData xml)
as
declare @intCtr int
DECLARE @handle INT  
DECLARE @PrepareXmlStatus INT  
begin	
	set nocount on
		
	--Select parent_ID,
 --              max(case when name='AEProductCategoryID' then convert(bigint,StringValue) else 0 end) as AEProductCategoryID,
 --              max(case when name='ProductCategoryName' then convert(Varchar(50),StringValue) else '' end) as ProductCategoryName,
 --              max(case when name='ProductCategoryParentID' then convert(bigint,StringValue) else 0 end) as ProductCategoryParentID
	--into #TempProdCategory
	--from parseJSON(@RawData)
	--where ValueType = 'string'
	--group by parent_ID

	EXEC @PrepareXmlStatus = sp_xml_preparedocument @handle OUTPUT, @RawData 

	SELECT  * into #TempProdCategory
	FROM    OPENXML(@handle, 'root/ProductCategories', 2)  
		WITH (
		AEProductCategoryID bigINT,
		ProductCategoryName varchar(1000),
		ProductCategoryParentID BIGINT
		)  

	EXEC sp_xml_removedocument @handle 		

	select @intCtr =  count(a.AEProductCategoryID) 
	from
	(	
		select AEProductCategoryID, ProductCategoryName, ProductCategoryParentID
		from #TempProdCategory 
		except
		select ProductCategoryID, ProductCategoryName, ParentID
		from ProductCategory
	)a
	
	if @intCtr >0 
		declare @AEProductCategoryID bigint
		declare @ProductCategoryName varchar(100)
		declare @ProductCategoryParentID bigint
		declare @RecordFound varchar(5)

		SET IDENTITY_INSERT dbo.ProductCategory on

		declare CurrProdCategory CURSOR READ_ONLY
		for
		select a.AEProductCategoryID, a.ProductCategoryName, a.ProductCategoryParentID, 
		       case when b.ProductCategoryId is null then 'False' else 'True' end  recordfound
		from #TempProdCategory a left outer join ProductCategory b on a.AEProductCategoryID = b.ProductCategoryID
		
		open CurrProdCategory

		Fetch Next From CurrProdCategory Into @AEProductCategoryID, @ProductCategoryName, @ProductCategoryParentID, @RecordFound
		
		While @@Fetch_Status = 0 Begin
			--print @AEBuilderID
			--print @BuilderName			
			if @RecordFound = 'False'	
				insert into ProductCategory (ProductCategoryId, ProductCategoryName, ParentId, RowStatusId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, RowGUID)
				select @AEProductCategoryID, @ProductCategoryName, @ProductCategoryParentID, 1, getdate(), 1, GETDATE(), 1,  NEWID()
			else
				update ProductCategory 
				SET ProductCategoryName = @ProductCategoryName,
				    ParentId = @ProductCategoryParentID,
					RowStatusId = 1,
					ModifiedOn = GETDATE()
				where ProductCategoryId = @AEProductCategoryID
			Fetch Next From CurrProdCategory Into @AEProductCategoryID, @ProductCategoryName, @ProductCategoryParentID, @RecordFound
		End -- End of Fetch		
		Close CurrProdCategory
		Deallocate CurrProdCategory
		drop table #TempProdCategory
		SET IDENTITY_INSERT dbo.ProductCategory off

		-- update tabale for inactive if not modified on current date
		update ProductCategory
		set RowStatusId = 2
		where convert(date,ModifiedOn,101) <> convert(date, getdate(),101)
	
end



