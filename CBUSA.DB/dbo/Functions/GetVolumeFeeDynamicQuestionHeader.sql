
CREATE function [dbo].[GetVolumeFeeDynamicQuestionHeader](@SurveyId varchar(10))
returns Varchar(max)
as
begin
	declare @ReturnCols varchar(max)
	declare @SurveyOrder int,
			@QuestionId bigint,
			@IndexNumber int,
			@QuestionValue varchar(max),
			@Ctr int
	declare CurrQuestionCols CURSOR READ_ONLY
	for
		Select a.SurveyOrder, a.QuestionId, a.IndexNumberColoumn, a.QuestionColumns
		from
		(
			select	questionid, questionvalue, SurveyOrder, IndexNumberColoumn,
					'['+cast(questionid as varchar) + case when len(cast(QuestionGridSettingHeaderId as varchar)) >0 then '_' + cast(IsNull(QuestionGridSettingHeaderId,'') as varchar) +'_'  + cast(IndexNumberColoumn as varchar) else '' end +'] as ''' + substring(questionvalue,0,128) +'''' as QuestionColumns
			from
			(
			select top 100 percent questionid, QuestionGridSettingHeaderId, questionvalue, IndexNumberColoumn,SurveyOrder 
			from 
			(
			select a.questionid, b.QuestionGridSettingHeaderId, case when b.RowHeaderValue is not null then b.RowHeaderValue else a.QuestionValue end questionvalue,
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
			--order by SurveyOrder, IndexNumberColoumn
			)tab00
			--order by SurveyOrder, IndexNumberColoumn
		) a
		order by a.SurveyOrder, a.QuestionId, a.IndexNumberColoumn
	open CurrQuestionCols
	Fetch Next From CurrQuestionCols Into @SurveyOrder, @QuestionId, @IndexNumber, @QuestionValue
	set @Ctr =0
	set @ReturnCols =''
	While @@Fetch_Status = 0 Begin
		set @Ctr+=1
		if @Ctr = 1
			set @ReturnCols+= @QuestionValue
		else
			set @ReturnCols+= ', ' +@QuestionValue	
		Fetch Next From CurrQuestionCols Into @SurveyOrder, @QuestionId, @IndexNumber, @QuestionValue
	End -- End of Fetch		
	Close CurrQuestionCols
return @ReturnCols
end






