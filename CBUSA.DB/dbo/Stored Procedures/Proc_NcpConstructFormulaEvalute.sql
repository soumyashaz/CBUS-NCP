--1 all 2 - complete 3 - incomplete
--exec Proc_NcpConstructFormulaEvalute 51,null,null
CREATE PROCEDURE [dbo].[Proc_NcpConstructFormulaEvalute]
	@SurveyId Bigint, @QuestionIdList varchar(max) = null, @RenderType int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
  
   --declare @SurveyId bigint = 48
declare @QUERY nvarchar(max)
declare @ColoumnName nvarchar(max)
--declare @SurveyId bigint = 48
declare @tbl table (uniqueRow Nvarchar(200),SurveyOrder int)

--select @ColoumnName=isnull(@ColoumnName+',','')+QUOTENAME(uniqueRow)
--from @tbl

insert into @tbl
select 
--QuestionId,questionvalue,
--IndexNumberRow,IndexNumberColoumn,
cast(questionid as varchar) +'_' + cast(QuestionGridSettingHeaderId as varchar) +'_' + cast(IndexNumberRow as varchar) +'_'+ cast(IndexNumberColoumn as varchar) as uniqueRow, SurveyOrder
 from(
select top 100 percent questionid, QuestionGridSettingHeaderId, questionvalue,IndexNumberRow,IndexNumberColoumn,SurveyOrder 
from 
(
select a.questionid, b.QuestionGridSettingHeaderId, a.QuestionValue + case when b.RowHeaderValue is not null then '~'+ b.RowHeaderValue else '' end questionvalue,
 case when b.IndexNumberRow is null then 0 else b.IndexNumberRow-1 end as IndexNumberRow,
 case when b.IndexNumberColoumn is null then 0 else b.IndexNumberColoumn-1 end as IndexNumberColoumn,
 SurveyOrder
from
(
select a.QuestionId, a.QuestionValue, isnull(s.QuestionGridSettingId,0) QuestionGridSettingId,a.SurveyOrder
from question a left outer join QuestionGridSetting s on a.QuestionId = s.QuestionId
where 1=1
--and a.questionid = 162
and a.SurveyId = @SurveyId
and a.RowStatusId=1
and 2=case  when @QuestionIdList is null 
			then 2 
			else 
			case when convert(varchar,a.QuestionId) in (select value from SplitQuestionList(@QuestionIdList)) 
			then 2 
			else 0
			end
	 end
)a
left outer join 
(
 select a.QuestionGridSettingHeaderId,
     a.RowHeaderValue + '~'+b.ColoumnHeaderValue RowHeaderValue,
     a.QuestionGridSettingId, b.IndexNumberColoumn, a.IndexNumberRow
 from
 (
 SELECT a.QuestionGridSettingHeaderId
    ,a.RowHeaderValue
    ,a.IndexNumber IndexNumberRow
    ,a.QuestionGridSettingId
    ,a.ControlType
    ,a.DropdownTypeOptionValue
 FROM QuestionGridSettingHeader a 
 where 1=1
 --and a.QuestionGridSettingId = 114
 and RowHeaderValue is not null
 ) a ,
 (
 SELECT a.QuestionGridSettingHeaderId
    ,a.RowHeaderValue
    ,a.ColoumnHeaderValue
    ,a.IndexNumber as IndexNumberColoumn
    ,a.QuestionGridSettingId
    ,a.ControlType
    ,a.DropdownTypeOptionValue
   FROM QuestionGridSettingHeader a 
   where 1=1
   --and a.QuestionGridSettingId = 114
   and ColoumnHeaderValue is not null
 ) b
 where a.QuestionGridSettingId = b.QuestionGridSettingId
) b
on a.QuestionGridSettingId = b.QuestionGridSettingId
) tab0
order by SurveyOrder,IndexNumberRow,IndexNumberColoumn
)tab00
order by SurveyOrder,uniqueRow
--select * from @tbl order by SurveyOrder,uniqueRow

select @ColoumnName=isnull(@ColoumnName+',','')+QUOTENAME(uniqueRow)
from @tbl order by SurveyOrder,uniqueRow
declare @AlterOrReplace as varchar(50)

IF OBJECT_ID('dbo.NcpEvaluteConstructFormula') IS NULL
BEGIN
    set @AlterOrReplace =' CREATE View dbo.NcpEvaluteConstructFormula as '
END
ELSE
BEGIN
    set @AlterOrReplace =' Alter View dbo.NcpEvaluteConstructFormula as '
END

set @QUERY = @AlterOrReplace + ' select  ROW_NUMBER() OVER (PARTITION BY Pvt.BuilderId ORDER BY Pvt.BuilderId) as [RowNumber],M.MarketName,
	B.BuilderName as [BuilderCompany],B.FirstName+'' ''+B.LastName as [BuilderName],P.ProjectName,
	concat(p.[Address],'' '', p.City, '' '', isnull(zip, ''''), '' '', p.[State]) as [ProjectAddress], q.QuaterName, q.Year,
	pvt.* from(
	select tab2.Answer,tab2.BuilderQuaterContractProjectReportId,tab2.ProjectId,tab2.BuilderId,tab2.ContractId,tab2.QuaterId,
	tab1.uniqueRow from (
	select QuestionId, QuestionGridSettingHeaderId, questionvalue,
	IndexNumberRow,IndexNumberColoumn,
	cast(questionid as varchar) +''_'' + cast(QuestionGridSettingHeaderId as varchar) +''_'' + cast(IndexNumberRow as varchar) +''_'' + cast(IndexNumberColoumn as varchar) as uniqueRow
	 from(
	select top 100 percent questionid, QuestionGridSettingHeaderId, questionvalue,IndexNumberRow,IndexNumberColoumn from (
	select a.questionid,  QuestionGridSettingHeaderId,
	 a.QuestionValue + case when b.RowHeaderValue is not null then ''~''+ b.RowHeaderValue else '''' end questionvalue,
	 case when b.IndexNumberRow is null then 0 else b.IndexNumberRow-1 end as IndexNumberRow,
	 case when b.IndexNumberColoumn is null then 0 else  b.IndexNumberColoumn-1 end as IndexNumberColoumn,
	 SurveyOrder
	from
	(
	select a.QuestionId, a.QuestionValue, isnull(s.QuestionGridSettingId,0) QuestionGridSettingId,a.SurveyOrder
	from question a left outer join QuestionGridSetting s on a.QuestionId = s.QuestionId
	where 1=1
	and a.SurveyId ='+CONVERT(nvarchar(50),@SurveyId)+'
	and a.RowStatusId=1'

if(@QuestionIdList is not null)
begin 

set @QUERY+='	and convert(varchar,a.QuestionId) in (select value from SplitQuestionList('+@QuestionIdList+'))'

end


set @QUERY+='	) a
	left outer join 
	(
	 select a.QuestionGridSettingHeaderId,
		 a.RowHeaderValue + ''~''+b.ColoumnHeaderValue RowHeaderValue,
		 a.QuestionGridSettingId, b.IndexNumberColoumn, a.IndexNumberRow
	 from
	 (
	 SELECT a.QuestionGridSettingHeaderId
		,a.RowHeaderValue
		,a.IndexNumber IndexNumberRow
		,a.QuestionGridSettingId
		,a.ControlType
		,a.DropdownTypeOptionValue
	 FROM QuestionGridSettingHeader a 
	 where 1=1
	 --and a.QuestionGridSettingId = 114
	 and RowHeaderValue is not null
	 ) a ,
	 (
	 SELECT a.QuestionGridSettingHeaderId
		,a.RowHeaderValue
		,a.ColoumnHeaderValue
		,a.IndexNumber as IndexNumberColoumn
		,a.QuestionGridSettingId
		,a.ControlType
		,a.DropdownTypeOptionValue
	   FROM QuestionGridSettingHeader a 
	   where 1=1
	   --and a.QuestionGridSettingId = 114
	   and ColoumnHeaderValue is not null
	 ) b
	 where a.QuestionGridSettingId = b.QuestionGridSettingId
	) b
	on a.QuestionGridSettingId = b.QuestionGridSettingId
	) tab0
	order by SurveyOrder,IndexNumberRow,IndexNumberColoumn
	)tab00
	)tab1
	--order by IndexNumberRow,IndexNumberColoumn
	 inner join 
	 (
		select Answer,RowNumber,ColumnNumber,QuestionId,x.BuilderQuaterContractProjectReportId,x.ProjectId,x.BuilderId,x.ContractId,x.QuaterId from
		BuilderQuaterAdminReport z inner join 
		BuilderQuaterContractProjectReport x  on z.BuilderQuaterAdminReportId=x.BuilderQuaterAdminReportId inner join 
		BuilderQuaterContractProjectDetails y  on x.BuilderQuaterContractProjectReportId=y.BuilderQuaterContractProjectReportId '

	--where y.BuilderQuaterContractProjectReportId in (33,34)

	if(@RenderType=2)
	begin
	set @QUERY+='	where z.IsSubmit=''true'''
	end
	else if(@RenderType=3)
	begin
	 set @QUERY+='	where z.IsSubmit=''false'''
	end



set @QUERY+='	) tab2
	 on tab1.QuestionId=tab2.QuestionId
	 and tab1.IndexNumberRow=tab2.RowNumber and tab1.IndexNumberColoumn=tab2.ColumnNumber
	 )tab3
	  PIVOT
	(
		max(answer)
		FOR uniqueRow IN ('+@ColoumnName+')
	)AS pvt
	inner join Builder B
	on B.BuilderId=pvt.BuilderId
	inner join Project P 
	on P.ProjectId=pvt.ProjectId
	inner join Market M
	on B.MarketId=M.MarketId
	inner join Quater q
	on pvt.QuaterId = q.QuaterId'

--print @QUERY
--select @QUERY
EXEC(@QUERY)

	

END










