
CREATE view [dbo].[vw_NCPSurveyResponseResult]
as
SELECT	r.BuilderQuaterAdminReportId, r.BuilderId, r.QuaterId, r.IsSubmit,
		r.BuilderQuaterContractProjectReportId, r.ContractId, r.ProjectId, r.ProjectStatusId, r.IsComplete,
		r.ContractName, r.Label, r.[Year],
		r.QuestionId, r.Answer, r.RowNumber, r.ColumnNumber, r.SurveyId, r.IsEnrolment, r.IsNcpSurvey, r.SurveyOrder
FROM
(
	SELECT	q.BuilderQuaterAdminReportId, q.BuilderId, q.QuaterId, q.IsSubmit,
			q.BuilderQuaterContractProjectReportId, q.ContractId, q.ProjectId, q.ProjectStatusId, q.IsComplete,
			q.ContractName, q.Label, q.[Year],
			x.QuestionId, x.Answer, x.RowNumber, x.ColumnNumber,  s.SurveyId, s.IsEnrolment, s.IsNcpSurvey, t.SurveyOrder
	FROM
	(
		SELECT a.BuilderQuaterAdminReportId, a.BuilderId, a.QuaterId, a.IsSubmit,
			   b.BuilderQuaterContractProjectReportId, b.ContractId, b.ProjectId, b.ProjectStatusId, b.IsComplete,
			   c.ContractName, c.Label, q.[Year]
		FROM BuilderQuaterAdminReport a inner join BuilderQuaterContractProjectReport b cross join Contract c cross join Quater q
		on a.BuilderQuaterAdminReportId = b.BuilderQuaterAdminReportId and b.ContractId = c.ContractId and a.QuaterId = q.QuaterId
		WHERE a.RowStatusId = 1
		AND b.RowStatusId = 1
		and c.RowStatusId = 1
	) q join BuilderQuaterContractProjectDetails x cross join Survey s cross join Question t
	on q.BuilderQuaterContractProjectReportId = x.BuilderQuaterContractProjectReportId and q.ContractId = s.ContractId and q.[Year] = s.[Year] /*and q.QuaterId = s.Quater*/
	and x.QuestionId = t.QuestionId
	WHERE x.RowStatusId = 1
	AND s.RowStatusId = 1
	AND t.RowStatusId = 1
) r
--order by r.ContractId, r.SurveyId, r.ProjectId, r.[Year], r.QuaterId, r.SurveyOrder



