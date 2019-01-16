--1 all 2 - complete 3 - incomplete
--exec Proc_NcpConstructFormulaEvalute_Cols 51,null,null
CREATE PROCEDURE [dbo].[Proc_NcpConstructFormulaEvalute_Cols]
	@SurveyId Bigint, @QuestionIdList varchar(max) = null, @RenderType int = 0
AS
BEGIN
	SET NOCOUNT ON;
  
--declare @SurveyId bigint = 48
declare @QUERY nvarchar(max)
declare @ColoumnName nvarchar(max)
declare @tbl table (uniqueRow Nvarchar(200),SurveyOrder int)

insert into @tbl
select 
cast(questionid as varchar) + case when len(cast(QuestionGridSettingHeaderId as varchar)) >0 then '_' + cast(IsNull(QuestionGridSettingHeaderId,'') as varchar) +'_'  + cast(IndexNumberColoumn as varchar) else '' end  as uniqueRow, SurveyOrder
 from(
select top 100 percent questionid, QuestionGridSettingHeaderId, questionvalue, IndexNumberColoumn,SurveyOrder 
from 
(
select a.questionid, b.QuestionGridSettingHeaderId, a.QuestionValue + case when b.RowHeaderValue is not null then '~'+ b.RowHeaderValue else '' end questionvalue,
 case when b.IndexNumberColoumn is null then 0 else b.IndexNumberColoumn-1 end as IndexNumberColoumn,
 SurveyOrder
from
(
select a.QuestionId, a.QuestionValue, isnull(s.QuestionGridSettingId,0) QuestionGridSettingId,a.SurveyOrder
from question a left outer join QuestionGridSetting s on a.QuestionId = s.QuestionId
where 1=1
and a.SurveyId = @SurveyId
and a.RowStatusId=1
--and 2 = case  when @QuestionIdList is null 
--			then 2 
--			else 
--			case when convert(varchar,a.QuestionId) in (select value from SplitQuestionList(@QuestionIdList)) 
--			then 2 
--			else 0
--			end
--	 end
)a
left outer join 
(
 select b.QuestionGridSettingHeaderId,
     b.ColoumnHeaderValue RowHeaderValue,
     b.QuestionGridSettingId, b.IndexNumberColoumn
 from
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
   and ColoumnHeaderValue is not null
  ) b
) b
on a.QuestionGridSettingId = b.QuestionGridSettingId
) tab0
order by SurveyOrder, IndexNumberColoumn
)tab00
order by SurveyOrder,uniqueRow

select @ColoumnName=isnull(@ColoumnName+',','')+QUOTENAME(uniqueRow)
from @tbl order by SurveyOrder,uniqueRow
declare @AlterOrReplace as varchar(50)

IF OBJECT_ID('dbo.NcpEvaluteConstructFormula') IS NULL
BEGIN
    set @AlterOrReplace =' Create View dbo.NcpEvaluteConstructFormula as '
END
ELSE
BEGIN
    set @AlterOrReplace =' Alter View dbo.NcpEvaluteConstructFormula as '
END

set @QUERY = @AlterOrReplace + ' select  ROW_NUMBER() OVER (PARTITION BY Pvt.BuilderId ORDER BY Pvt.BuilderId) as [RowNumber], M.MarketId, M.MarketName,
	B.BuilderName as [BuilderCompany],B.FirstName+'' ''+B.LastName as [BuilderName],P.ProjectName,
	concat(p.[Address],'' '', p.City, '' '', isnull(zip, ''''), '' '', p.[State]) as [ProjectAddress], q.QuaterName, q.Year,
	pvt.* from(
	select tab1.SurveyId, tab2.Answer, tab2.AnswerRowIndex, tab2.BuilderQuaterContractProjectReportId,tab2.ProjectId,tab2.BuilderId,tab2.ContractId,tab2.QuaterId,
	tab1.uniqueRow from (
	select SurveyId, QuestionId, isnull(QuestionGridSettingHeaderId, 0) QuestionGridSettingHeaderId, questionvalue,
	IndexNumberColoumn,
	cast(questionid as varchar) + case when len(cast(QuestionGridSettingHeaderId as varchar)) >0 then ''_'' + cast(IsNull(QuestionGridSettingHeaderId,'''') as varchar) +''_''  + cast(IndexNumberColoumn as varchar) else '''' end  as uniqueRow
	from(
	select top 100 percent SurveyId, questionid, QuestionGridSettingHeaderId, questionvalue, IndexNumberColoumn from (
	select a.SurveyId, a.questionid,  QuestionGridSettingHeaderId,
	 a.QuestionValue + case when b.RowHeaderValue is not null then ''~''+ b.RowHeaderValue else '''' end questionvalue,	 
	 case when b.IndexNumberColoumn is null then 0 else  b.IndexNumberColoumn-1 end as IndexNumberColoumn,
	 SurveyOrder
	from
	(
	select a.SurveyId, a.QuestionId, a.QuestionValue, isnull(s.QuestionGridSettingId,0) QuestionGridSettingId,a.SurveyOrder
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
	 select b.QuestionGridSettingHeaderId,
		 b.ColoumnHeaderValue RowHeaderValue,
		 b.QuestionGridSettingId, b.IndexNumberColoumn
	 from
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
	) b
	on a.QuestionGridSettingId = b.QuestionGridSettingId
	) tab0
	order by SurveyOrder, IndexNumberColoumn
	)tab00
	)tab1
	--order by IndexNumberColoumn
	 inner join 
	 (
		select Answer, RowNumber AnswerRowIndex, ColumnNumber, y.QuestionId, x.BuilderQuaterContractProjectReportId, x.ProjectId, x.BuilderId, x.ContractId, x.QuaterId, h.QuestionGridSettingHeaderId, h.ColoumnHeaderValue
		from BuilderQuaterAdminReport z inner join 
		BuilderQuaterContractProjectReport x  on z.BuilderQuaterAdminReportId=x.BuilderQuaterAdminReportId inner join 
		BuilderQuaterContractProjectDetails y  on x.BuilderQuaterContractProjectReportId=y.BuilderQuaterContractProjectReportId 
		left outer join QuestionGridSetting g on g.QuestionId = y.QuestionId 
		left outer join QuestionGridSettingHeader h on g.QuestionGridSettingId = h.QuestionGridSettingId and (h.IndexNumber -1) = y.ColumnNumber and h.ColoumnHeaderValue is not null
		'

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
	 and /*tab1.IndexNumberRow=tab2.RowNumber and*/ tab1.IndexNumberColoumn=tab2.ColumnNumber
	 and isnull(tab1.QuestionGridSettingHeaderId,0) = isnull(tab2.QuestionGridSettingHeaderId,0)
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
