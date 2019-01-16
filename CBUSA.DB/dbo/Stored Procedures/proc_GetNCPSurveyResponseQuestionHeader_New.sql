--sp_helptext proc_GetNCPSurveyResponseQuestionHeader_New


CREATE procedure [dbo].[proc_GetNCPSurveyResponseQuestionHeader_New] @SurveyId bigint, @QuestionIdList varchar(max) =null
as
if @QuestionIdList is null or @QuestionIdList =''
 --select 'Contract Name' QuestionValue, convert(varchar,1.1) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 --union 
 select 'Builder Market' QuestionValue, convert(varchar,1.2) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
  select 'Builder Id' QuestionValue, convert(varchar,1.3) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
 select 'Company' QuestionValue, convert(varchar,1.4) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all

 select 'Project Name' QuestionValue, convert(varchar,1.5) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
 select 'Project Address' QuestionValue, convert(varchar,1.6) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
 select 
 QuestionValue,
 cast(SurveyOrder as varchar) + cast(questionid as varchar)+cast(IndexNumberRow as varchar)+cast(IndexNumberColoumn as varchar) as uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
  from(
 select top 100 percent questionid,questionvalue,IndexNumberRow,IndexNumberColoumn,SurveyOrder from (
 select a.questionid,
  case when b.RowHeaderValue is not null then b.RowHeaderValue else a.QuestionValue end questionvalue,
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
 ) a
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
 order by uniqueRow
else
 --select 'Contract Name' ContractName, convert(varchar,1.1) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 --union 
 select 'Builder Market' QuestionValue, convert(varchar,1.2) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
  select 'Builder Id' QuestionValue, convert(varchar,1.3) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
 select 'Company' QuestionValue, convert(varchar,1.4) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
 --select 'Builder Name' QuestionValue, convert(varchar,1.4) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 --union all
 select 'Project Name' QuestionValue, convert(varchar,1.5) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
 select 'Project Address' QuestionValue, convert(varchar,1.6) uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
 union all
 select 
 QuestionValue,
 cast(SurveyOrder as varchar) + cast(questionid as varchar)+cast(IndexNumberRow as varchar)+cast(IndexNumberColoumn as varchar) as uniqueRow, convert(bigint,0) ProjectId, convert(bigint,0) QuestionId, 0 RowIndex, 0 ColIndex, 0 QuestionTypeId
  from(
 select top 100 percent questionid,questionvalue,IndexNumberRow,IndexNumberColoumn,SurveyOrder from (
 select a.questionid,
  case when b.RowHeaderValue is not null then b.RowHeaderValue else a.QuestionValue end questionvalue,
  case when b.IndexNumberRow is null then 0 else b.IndexNumberRow-1 end as IndexNumberRow,
  case when b.IndexNumberColoumn is null then 0 else  b.IndexNumberColoumn-1 end as IndexNumberColoumn,
  SurveyOrder
 from
 (
 select a.QuestionId, a.QuestionValue, isnull(s.QuestionGridSettingId,0) QuestionGridSettingId,a.SurveyOrder
 from question a left outer join QuestionGridSetting s on a.QuestionId = s.QuestionId
 where 1=1
 and convert(varchar,a.QuestionId) in (select value from SplitQuestionList(@QuestionIdList) )
 and a.SurveyId = @SurveyId
 and a.RowStatusId=1
 ) a
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
 order by uniqueRow


