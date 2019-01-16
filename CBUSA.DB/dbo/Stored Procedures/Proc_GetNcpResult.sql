--1 all 2 - complete 3 - incomplete
--exec Proc_GetNcpResult 51,null,null
CREATE PROCEDURE [dbo].[Proc_GetNcpResult]
	@SurveyId Bigint, @QuestionIdList varchar(max) = null,@RenderType int=0, @MarketId Bigint=0, @BuilderId Bigint=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
  
   --declare @SurveyId bigint = 48
declare @Query nvarchar(max)
declare @ColoumnName nvarchar(max)
--declare @SurveyId bigint = 48
declare @tbl table (uniqueRow Nvarchar(200),SurveyOrder int)

--select @ColoumnName=isnull(@ColoumnName+',','')+QUOTENAME(uniqueRow)
--from @tbl

insert into @tbl
select 
--QuestionId,questionvalue,
--IndexNumberRow,IndexNumberColoumn,
cast(questionid as varchar)+cast(IndexNumberRow as varchar)+cast(IndexNumberColoumn as varchar) as uniqueRow,SurveyOrder
 from(
select top 100 percent questionid,questionvalue,IndexNumberRow,IndexNumberColoumn,SurveyOrder from (
select a.questionid,
 a.QuestionValue + case when b.RowHeaderValue is not null then '~'+ b.RowHeaderValue else '' end questionvalue,
 case when b.IndexNumberRow is null then 0 else b.IndexNumberRow-1 end as IndexNumberRow,
 case when b.IndexNumberColoumn is null then 0 else  b.IndexNumberColoumn-1 end as IndexNumberColoumn,
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


set @Query='
SELECT  ROW_NUMBER() OVER (PARTITION BY Pvt.BuilderId ORDER BY Pvt.BuilderId) as [RowNumber],M.MarketName,
		B.BuilderName as [BuilderCompany],B.FirstName+'' ''+B.LastName as [BuilderName],P.ProjectName,
		concat(p.[Address],'' '', p.City, '' '', isnull(zip, ''''), '' '', p.[State]) as [ProjectAddress],
		pvt.* 
FROM
(
   SELECT Answer, BuilderQuaterContractProjectReportId, ProjectId, BuilderId, ContractId, QuaterId, uniqueRow, 
	      CASE WHEN FileName IS NULL THEN 0 ELSE 1 END As FileUpload
   FROM
   (
	  SELECT tab2.Answer, 	      
	       tab2.BuilderQuaterContractProjectReportId,tab2.ProjectId,tab2.BuilderId,tab2.ContractId,tab2.QuaterId, tab1.uniqueRow,
		   max(tab2.FileName) Over(PARTITION BY BuilderQuaterContractProjectReportId) as FileName 
	  FROM 
	  (
		SELECT QuestionId, questionvalue, IsFileNeedtoUpload, IndexNumberRow, IndexNumberColoumn,
		       cast(questionid as varchar)+cast(IndexNumberRow as varchar)+cast(IndexNumberColoumn as varchar) as uniqueRow
		FROM
		(
			SELECT top 100 percent questionid,questionvalue,IndexNumberRow,IndexNumberColoumn, IsFileNeedtoUpload 
			FROM 
			(
				SELECT a.questionid, 
				       a.QuestionValue + case when b.RowHeaderValue is not null then ''~''+ b.RowHeaderValue else '''' end questionvalue,
					   case when b.IndexNumberRow is null then 0 else b.IndexNumberRow-1 end as IndexNumberRow,
					   case when b.IndexNumberColoumn is null then 0 else  b.IndexNumberColoumn-1 end as IndexNumberColoumn, 
					   SurveyOrder, a.IsFileNeedtoUpload
				FROM
				(
					select a.QuestionId, a.QuestionValue, a.IsFileNeedtoUpload, isnull(s.QuestionGridSettingId,0) QuestionGridSettingId,a.SurveyOrder
					from question a left outer join QuestionGridSetting s on a.QuestionId = s.QuestionId
					WHERE 1=1
					  AND a.SurveyId ='+CONVERT(nvarchar(50),@SurveyId)+'
					  AND a.RowStatusId=1'

if(@QuestionIdList is not null)
begin 

set @Query+='					AND convert(varchar,a.QuestionId) in (select value from SplitQuestionList('+@QuestionIdList+'))'

end


set @Query+='				) a
				LEFT OUTER JOIN 
                (
					SELECT a.QuestionGridSettingHeaderId, a.RowHeaderValue + ''~''+b.ColoumnHeaderValue RowHeaderValue,
                           a.QuestionGridSettingId, b.IndexNumberColoumn, a.IndexNumberRow
					FROM
					(
						SELECT a.QuestionGridSettingHeaderId, a.RowHeaderValue, a.IndexNumber IndexNumberRow, a.QuestionGridSettingId,
							   a.ControlType, a.DropdownTypeOptionValue
                        FROM QuestionGridSettingHeader a 
                        WHERE 1=1
                          --AND a.QuestionGridSettingId = 114
                          AND RowHeaderValue is not null
                    ) a ,
                    (
                       SELECT a.QuestionGridSettingHeaderId, a.RowHeaderValue, a.ColoumnHeaderValue, a.IndexNumber as IndexNumberColoumn,
							  a.QuestionGridSettingId, a.ControlType, a.DropdownTypeOptionValue
                       FROM QuestionGridSettingHeader a 
                       WHERE 1=1
                         --AND a.QuestionGridSettingId = 114
                         AND ColoumnHeaderValue is not null
                    ) b
                    WHERE a.QuestionGridSettingId = b.QuestionGridSettingId
                ) b
                on a.QuestionGridSettingId = b.QuestionGridSettingId
           ) tab0
           ORDER BY SurveyOrder,IndexNumberRow,IndexNumberColoumn
       )tab00
   )tab1
   --ORDER BY IndexNumberRow,IndexNumberColoumn
   INNER JOIN 
   (
		SELECT Answer,RowNumber,ColumnNumber,QuestionId,x.BuilderQuaterContractProjectReportId,x.ProjectId,x.BuilderId,x.ContractId,x.QuaterId, y.FileName  
		FROM BuilderQuaterAdminReport z 
		   inner join BuilderQuaterContractProjectReport x  
		     on z.BuilderQuaterAdminReportId = x.BuilderQuaterAdminReportId 
		   inner join BuilderQuaterContractProjectDetails y  
		     on x.BuilderQuaterContractProjectReportId=y.BuilderQuaterContractProjectReportId '

	--where y.BuilderQuaterContractProjectReportId in (33,34)

	if(@RenderType=2)
	begin
	set @Query+='		WHERE z.IsSubmit=''true'''
	--set @Query+='		WHERE x.IsComplete=''true'''
	end
	else if(@RenderType=3)
	begin
	 set @Query+='		WHERE z.IsSubmit=''false'''
	 --set @Query+='		WHERE x.IsComplete=''false'''
	end



set @Query+='   ) tab2
     on tab1.QuestionId=tab2.QuestionId
     and tab1.IndexNumberRow=tab2.RowNumber and tab1.IndexNumberColoumn=tab2.ColumnNumber
   ) x
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
WHERE 1= 1'

if(@MarketId is not null and @MarketId > 0)
begin 
set @Query+=' AND B.MarketId = '+CONVERT(nvarchar(50),@MarketId)+''
end

if(@BuilderId is not null and @BuilderId > 0)
begin 
set @Query+=' AND pvt.BuilderId = '+CONVERT(nvarchar(50),@BuilderId)+''
end

--print @QUERY
--select @
EXEC(@QUERY)

	

END