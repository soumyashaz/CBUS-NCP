

CREATE procedure [dbo].[proc_GetCBUSA_Products](@RawData xml)
as
declare @intCtr int
DECLARE @handle INT  
DECLARE @PrepareXmlStatus INT  
declare @XMLData xml
declare @RawDataVarchar varchar(max)
begin
	
	declare @XML1 xml
	set @RawDataVarchar = replace(replace(convert(varchar(max), @RawData),'</ProductProductCategory>',''),'<ProductProductCategory>','')

	set @XMLData = convert(xml, @RawDataVarchar)
	
	--set nocount on
	EXEC @PrepareXmlStatus= sp_xml_preparedocument @handle OUTPUT, @XMLData 

	SELECT  * into #TempProduct
	FROM    OPENXML(@handle, 'root/ProductDetails', 2)  
		WITH (
		AEProductID bigINT,
		ProductName varchar(1000),
		AEProductCategoryID BIGINT,
		ProductCategoryName VARCHAR(1000)
		)  

	EXEC sp_xml_removedocument @handle 		
	
	select @intCtr =  count(a.AEProductID) 
	from
	(		
		select AEProductID, ProductName, AEProductCategoryID --, ProductCategoryName
		from #TempProduct a 
		except
		select ProductId, ProductName, ProductCategoryId
		from Product
	)a
	
	if @intCtr >0 
		declare @AEProductID bigint
		declare @ProductName varchar(100)
		declare @AEProductCategoryID bigint
		declare @RecordFound varchar(5)

		SET IDENTITY_INSERT dbo.Product on

		declare CurrProduct CURSOR READ_ONLY
		for
		select a.AEProductID, a.ProductName, a.AEProductCategoryID,
		       case when b.ProductId is null then 'False' else 'True' end  recordfound --, AEBuilderHistoricID, BuilderMarket	
		from #TempProduct a left outer join Product b on a.AEProductID = b.ProductId
		where a.AEProductCategoryID is not null

		open CurrProduct

		Fetch Next From CurrProduct Into @AEProductID, @ProductName, @AEProductCategoryID, @RecordFound
		
		While @@Fetch_Status = 0 Begin
			--print @AEBuilderID
			--print @BuilderName			
			if @RecordFound = 'False'	
				insert into Product(ProductId, ProductName, ProductCategoryId, RowStatusId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, RowGUID)
				select @AEProductID, @ProductName, @AEProductCategoryID, 1 RowStatusId, GETDATE(), 1, GETDATE(), 1, NEWID()
			else
				update Product 
				SET ProductName = @ProductName,
					ProductCategoryId = @AEProductCategoryID,
					RowStatusId = 1,
					ModifiedOn = GETDATE()
				where ProductId = @AEProductID
			Fetch Next From CurrProduct Into @AEProductID, @ProductName, @AEProductCategoryID, @RecordFound
		End -- End of Fetch		
		Close CurrProduct
		Deallocate CurrProduct
		drop table #TempProduct
		SET IDENTITY_INSERT dbo.Product off

		-- update tabale for inactive if not modified on current date
		update Product
		set RowStatusId = 2
		where convert(date,ModifiedOn,101) <> convert(date, getdate(),101)
end



