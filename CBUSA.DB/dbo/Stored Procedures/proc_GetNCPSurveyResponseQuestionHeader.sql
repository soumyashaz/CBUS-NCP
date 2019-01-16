
CREATE procedure [dbo].[proc_GetNCPSurveyResponseQuestionHeader] @SurveyId numeric, @QuestionIdList varchar(max) =null
as
print @QuestionIdList
if @QuestionIdList is null or @QuestionIdList =''
	SELECT 1 SurveyId, 0 QuestionId, 'Status' QuestionValue, 0 ProjectId,
			0 QuestionTypeId, convert(smallint,0.1) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, '' ContractName, 0 ContractId, 0 ColIndex, 0 RowIndex
	union
	SELECT 2 SurveyId, 1 QuestionId, 'Invite full name' QuestionValue, 1 ProjectId,
				1 QuestionTypeId, convert(smallint,0.2) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, '' ContractName, 1 ContractId, 0 IndexNumber, 0 IndexNumberRow
	union
	SELECT 3 SurveyId, 2 QuestionId, 'Invite email' QuestionValue, 2 ProjectId,
				2 QuestionTypeId, convert(smallint,0.3) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, '' ContractName, 2 ContractId, 0 IndexNumber, 0 IndexNumberRow
	union
	SELECT	m.SurveyId, m.QuestionId, isnull(n.ProjectName,'') + '~' + m.QuestionValue QuestionValue, n.ProjectId,
			m.QuestionTypeId, m.SurveyOrder, m.[Row], m.[Column], m.QuestionGridSettingId, n.ContractName, n.ContractId, isnull(m.IndexNumber,0) ColIndex, isnull(m.IndexNumberRow,0) RowIndex
	FROM
	(
		SELECT z.SurveyId, z.QuestionId, z.QuestionValue + case when c.RowHeaderValue is not null then '~' + isnull(c.RowHeaderValue,'') else '' end QuestionValue, 
			   z.QuestionTypeId, z.SurveyOrder, isnull(z.[Row],0) [Row], isnull(z.[Column],0) [Column], isnull(z.QuestionGridSettingId,0) QuestionGridSettingId, c.IndexNumber, c.IndexNumberRow
		FROM
		(
		SELECT q.QuestionId, q.QuestionValue, q.IsMandatory, q.IsFileNeedtoUpload, q.QuestionTypeId, q.SurveyId , q.SurveyOrder, 
				qgs.QuestionGridSettingId, qgs.[Row], qgs.[Column]
		FROM Question q left outer join QuestionGridSetting qgs on q.QuestionId = qgs.QuestionId 
		WHERE surveyid = @SurveyId
		AND q.RowStatusId = 1
		)z left outer join
		(
		SELECT a.QuestionGridSettingHeaderId,
			   a.RowHeaderValue + '~'+b.ColoumnHeaderValue RowHeaderValue,
			   a.QuestionGridSettingId, b.IndexNumber, a.IndexNumberRow
		FROM
		(
		SELECT a.QuestionGridSettingHeaderId
			  ,a.RowHeaderValue
			  ,a.IndexNumber IndexNumberRow
			  ,a.QuestionGridSettingId
			  ,a.ControlType
			  ,a.DropdownTypeOptionValue
		FROM QuestionGridSettingHeader a 
		WHERE 1=1
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
		  WHERE 1=1
		--and a.QuestionGridSettingId = 10010
		  and ColoumnHeaderValue is not null
		) b
		WHERE a.QuestionGridSettingId = b.QuestionGridSettingId
		) c
		on z.QuestionGridSettingId = c.QuestionGridSettingId
	) m ,
	(
		SELECT distinct a.contractid, a.QuaterId, a.ProjectId, c.ContractName, p.ProjectName
		FROM vw_NCPSurveyResponseResult a inner join Contract c on a.ContractId = c.ContractId inner join Project p on a.ProjectId = p.ProjectId
		WHERE SurveyId = @SurveyId
	) n
	order by contractid, ProjectId, SurveyOrder, RowIndex, ColIndex
else
	SELECT  SurveyId, QuestionId, QuestionValue, ProjectId, 
			QuestionTypeId, SurveyOrder, [Row], [Column], QuestionGridSettingId, ContractName, ContractId, 0 ColIndex, 0 RowIndex
	FROM
	(
	SELECT 1 SurveyId, 0 QuestionId, 'Status' QuestionValue, 0 ProjectId,
			0 QuestionTypeId, convert(smallint,0.1) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, '' ContractName, 0 ContractId
	union
	SELECT 2 SurveyId, 1 QuestionId, 'Invite full name' QuestionValue, 1 ProjectId,
				1 QuestionTypeId, convert(smallint,0.2) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, '' ContractName, 1 ContractId
	union
	SELECT 3 SurveyId, 2 QuestionId, 'Invite email' QuestionValue, 2 ProjectId,
				2 QuestionTypeId, convert(smallint,0.3) SurveyOrder, 0 [Row], 0 [Column], 0 QuestionGridSettingId, '' ContractName, 2 ContractId
	) a
	WHERE convert(varchar,QuestionTypeId) in (select value from SplitQuestionList(@QuestionIdList) )
	union
	SELECT	m.SurveyId, m.QuestionId, isnull(n.ProjectName,'') +'~'+m.QuestionValue QuestionValue, n.ProjectId,
			m.QuestionTypeId, m.SurveyOrder, m.[Row], m.[Column], m.QuestionGridSettingId, n.ContractName, n.ContractId, isnull(m.IndexNumber,0) ColIndex, isnull(m.IndexNumberRow,0) RowIndex
	FROM
	(
		SELECT z.SurveyId, z.QuestionId, z.QuestionValue + case when c.RowHeaderValue is not null then '~' + isnull(c.RowHeaderValue,'') else '' end QuestionValue, 
			   z.QuestionTypeId, z.SurveyOrder, isnull(z.[Row],0) [Row], isnull(z.[Column],0) [Column], isnull(z.QuestionGridSettingId,0) QuestionGridSettingId, c.IndexNumber, c.IndexNumberRow
		FROM
		(
		SELECT q.QuestionId, q.QuestionValue, q.IsMandatory, q.IsFileNeedtoUpload, q.QuestionTypeId, q.SurveyId , q.SurveyOrder, 
				qgs.QuestionGridSettingId, qgs.[Row], qgs.[Column]
		FROM Question q left outer join QuestionGridSetting qgs on q.QuestionId = qgs.QuestionId 
		WHERE surveyid = @SurveyId
		--and charindex(convert(varchar,q.QuestionId), (@QuestionIdList)) >0
		and convert(varchar,q.QuestionId) in (select value from SplitQuestionList(@QuestionIdList) )
		AND q.RowStatusId = 1
		)z left outer join
		(
		SELECT a.QuestionGridSettingHeaderId,
			   a.RowHeaderValue + '~'+b.ColoumnHeaderValue RowHeaderValue,
			   a.QuestionGridSettingId, b.IndexNumber, a.IndexNumberRow
		FROM
		(
		SELECT a.QuestionGridSettingHeaderId
			  ,a.RowHeaderValue
			  ,a.IndexNumber IndexNumberRow
			  ,a.QuestionGridSettingId
			  ,a.ControlType
			  ,a.DropdownTypeOptionValue
		FROM QuestionGridSettingHeader a 
		WHERE 1=1
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
		  WHERE 1=1
		--and a.QuestionGridSettingId = 10010
		  and ColoumnHeaderValue is not null
		) b
		WHERE a.QuestionGridSettingId = b.QuestionGridSettingId
		) c
		on z.QuestionGridSettingId = c.QuestionGridSettingId
	) m ,
	(
		SELECT distinct a.contractid, a.QuaterId, a.ProjectId, c.ContractName, p.ProjectName
		FROM vw_NCPSurveyResponseResult a inner join Contract c on a.ContractId = c.ContractId inner join Project p on a.ProjectId = p.ProjectId
		WHERE SurveyId = @SurveyId
		and convert(varchar, QuestionId) in (select value from SplitQuestionList(@QuestionIdList) )
	) n
	--WHERE charindex(convert(varchar,m.QuestionId), (@QuestionIdList)) >0
	order by ContractId, ProjectId, SurveyOrder, RowIndex, ColIndex


