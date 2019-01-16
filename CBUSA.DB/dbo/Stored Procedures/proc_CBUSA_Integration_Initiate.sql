
CREATE procedure [dbo].[proc_CBUSA_Integration_Initiate] @Markets xml, @Builders xml, @ProdCategory xml, @Prod xml, @Users xml, @Manufacturer xml
as
begin		
	exec [dbo].proc_GetCBUSA_Market @Markets
	exec [dbo].proc_GetCBUSA_Builders @Builders
	exec [dbo].proc_GetCBUSA_Users @Users
	--exec [dbo].proc_GetCBUSA_ProductCategories @ProdCategory
	--exec [dbo].proc_GetCBUSA_Products @Prod
	exec [dbo].proc_GetCBUSA_Manufacturer @Manufacturer
end

