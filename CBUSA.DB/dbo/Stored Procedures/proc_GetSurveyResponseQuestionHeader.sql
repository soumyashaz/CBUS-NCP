--sp_helptext proc_GetSurveyResponseQuestionHeader


CREATE procedure [dbo].[proc_GetSurveyResponseQuestionHeader] @SurveyId bigint, @QuestionIdList varchar(max) =null
as
if @QuestionIdList is null or @QuestionIdList =''
	SELECT 1 SurveyId, 0 QuestionId, 'Status' QuestionValue, 
			0 QuestionTypeId, convert(smallint,0.1) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, convert(bigint, 0) ProjectId, 0 ColIndex, 0 RowIndex
	union
	SELECT 2 SurveyId, 1 QuestionId, 'Market' QuestionValue, 
				1 QuestionTypeId, convert(smallint,0.2) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, convert(bigint, 0) ProjectId, 0 ColIndex, 0 RowIndex
	union
	SELECT 3 SurveyId, 1 QuestionId, 'Builder Id' QuestionValue, 
				1 QuestionTypeId, convert(smallint,0.2) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, convert(bigint, 0) ProjectId, 0 ColIndex, 0 RowIndex
	union
	SELECT 4 SurveyId, 2 QuestionId, 'Company name' QuestionValue, 
				2 QuestionTypeId, convert(smallint,0.3) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, convert(bigint, 0) ProjectId, 0 ColIndex, 0 RowIndex
	union	
	select z.SurveyId, z.QuestionId, z.QuestionValue + case when c.RowHeaderValue is not null then '~' + isnull(c.RowHeaderValue,'') else '' end QuestionValue, 
		   z.QuestionTypeId, z.SurveyOrder, isnull(z.[Row],0) [Row], isnull(z.[Column],0) [Column], isnull(z.QuestionGridSettingId,0) QuestionGridSettingId, convert(bigint, 0) ProjectId, isnull(c.IndexNumber,0) ColIndex, isnull(c.IndexNumberRow,0) RowIndex
	from
	(
	SELECT q.QuestionId, q.QuestionValue, q.IsMandatory, q.IsFileNeedtoUpload, q.QuestionTypeId, q.SurveyId , q.SurveyOrder, 
			qgs.QuestionGridSettingId, qgs.[Row], qgs.[Column]
	FROM Question q left outer join QuestionGridSetting qgs on q.QuestionId = qgs.QuestionId 
	WHERE surveyid = @SurveyId	
	AND q.RowStatusId = 1
	)z left outer join
	(
	select a.QuestionGridSettingHeaderId,
		   a.RowHeaderValue + '~'+b.ColoumnHeaderValue RowHeaderValue,
		   a.QuestionGridSettingId, b.IndexNumber, a.IndexNumberRow
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
	--and a.QuestionGridSettingId = 10010
	and RowHeaderValue is not null
	) a ,
	(
	SELECT a.QuestionGridSettingHeaderId
		  ,a.RowHeaderValue
		  ,a.ColoumnHeaderValue
		  ,a.IndexNumber
		  ,a.QuestionGridSettingId
		  ,a.ControlType
		  ,a.DropdownTypeOptionValue
	  FROM QuestionGridSettingHeader a 
	  where 1=1
	--and a.QuestionGridSettingId = 10010
	  and ColoumnHeaderValue is not null
	) b
	where a.QuestionGridSettingId = b.QuestionGridSettingId
	) c
	on z.QuestionGridSettingId = c.QuestionGridSettingId
	order by surveyorder, QuestionId
else
	SELECT  SurveyId, QuestionId, QuestionValue,
			QuestionTypeId, SurveyOrder, [Row], [Column], QuestionGridSettingId, convert(bigint, 0) ProjectId, 0 ColIndex, 0 RowIndex
	FROM
	(
	SELECT 1 SurveyId, 0 QuestionId, 'Status' QuestionValue,
			0 QuestionTypeId, convert(smallint,0.1) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId
	union
	SELECT 2 SurveyId, 1 QuestionId, 'Market' QuestionValue, 
				1 QuestionTypeId, convert(smallint,0.2) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId
				union
	SELECT 3 SurveyId, 1 QuestionId, 'Builder Id' QuestionValue, 
				1 QuestionTypeId, convert(smallint,0.2) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId
	union
	SELECT 4 SurveyId, 2 QuestionId, 'Company name' QuestionValue,
				2 QuestionTypeId, convert(smallint,0.3) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId
	) a
	WHERE convert(varchar,QuestionTypeId) in (select value from SplitQuestionList(@QuestionIdList) )
	union
	select z.SurveyId, z.QuestionId, z.QuestionValue + case when c.RowHeaderValue is not null then '~' + isnull(c.RowHeaderValue,'') else '' end QuestionValue, 
		   z.QuestionTypeId, z.SurveyOrder, isnull(z.[Row],0) [Row], isnull(z.[Column],0) [Column], isnull(z.QuestionGridSettingId,0) QuestionGridSettingId, convert(bigint, 0) ProjectId, isnull(c.IndexNumber,0) ColIndex, isnull(c.IndexNumberRow,0) RowIndex
	from
	(
	SELECT q.QuestionId, q.QuestionValue, q.IsMandatory, q.IsFileNeedtoUpload, q.QuestionTypeId, q.SurveyId , q.SurveyOrder, 
			qgs.QuestionGridSettingId, qgs.[Row], qgs.[Column]
	FROM Question q left outer join QuestionGridSetting qgs on q.QuestionId = qgs.QuestionId 
	WHERE surveyid = @SurveyId
	and charindex(convert(varchar,q.QuestionId), (@QuestionIdList)) >0
	AND q.RowStatusId = 1
	)z left outer join
	(
	select a.QuestionGridSettingHeaderId,
		   a.RowHeaderValue + '~'+b.ColoumnHeaderValue RowHeaderValue,
		   a.QuestionGridSettingId, b.IndexNumber, a.IndexNumberRow
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
	--and a.QuestionGridSettingId = 10010
	and RowHeaderValue is not null
	) a ,
	(
	SELECT a.QuestionGridSettingHeaderId
		  ,a.RowHeaderValue
		  ,a.ColoumnHeaderValue
		  ,a.IndexNumber
		  ,a.QuestionGridSettingId
		  ,a.ControlType
		  ,a.DropdownTypeOptionValue
	  FROM QuestionGridSettingHeader a 
	  where 1=1
	--and a.QuestionGridSettingId = 10010
	  and ColoumnHeaderValue is not null
	) b
	where a.QuestionGridSettingId = b.QuestionGridSettingId
	) c
	on z.QuestionGridSettingId = c.QuestionGridSettingId
	order by surveyorder, QuestionId





