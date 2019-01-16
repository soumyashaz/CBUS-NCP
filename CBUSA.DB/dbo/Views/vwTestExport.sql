create view [dbo].[vwTestExport]
as
select 1 Id, 'Test 1' Name, 'Address' Address
Union
select 2 Id, 'Test 2' Name, 'Address 2' Address
