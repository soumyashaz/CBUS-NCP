CREATE function [dbo].[GetVolumeFeeDynamicQuestionWhereClause](@SurveyId varchar(10), @ColumnValueToCheck varchar(100))
returns Varchar(max)
as
begin
	declare @ReturnCols varchar(max)
	declare @SurveyOrder int,
			@QuestionId bigint,
			@IndexNumber int,
			@QuestionValue varchar(max),
			@DropListQuestion varchar(50),
			@DropListGridQuestion varchar(50),
			@Ctr int
	declare CurrQuestionCols CURSOR READ_ONLY
	for
		Select a.SurveyOrder, a.QuestionId, a.IndexNumberColoumn, a.QuestionColumns, DropListQuestion, DropListGridQuestion
		from
		(
			select	questionid, questionvalue, SurveyOrder, IndexNumberColoumn, DropListQuestion, DropListGridQuestion,
					'['+cast(questionid as varchar) + case when len(cast(QuestionGridSettingHeaderId as varchar)) >0 then '_' + cast(IsNull(QuestionGridSettingHeaderId,'') as varchar) +'_'  + cast(IndexNumberColoumn as varchar) else '' end +'] ''' as QuestionColumns
			from
			(
			select top 100 percent questionid, QuestionGridSettingHeaderId, questionvalue, IndexNumberColoumn,SurveyOrder, DropListQuestion, DropListGridQuestion
			from 
			(
			select a.questionid, b.QuestionGridSettingHeaderId, case when b.RowHeaderValue is not null then b.RowHeaderValue else a.QuestionValue end questionvalue,
			 case when b.IndexNumberColoumn is null then 0 else b.IndexNumberColoumn-1 end as IndexNumberColoumn,
			 SurveyOrder, b.DropListGridQuestion, case when a.QuestionTypeId = 2 then 'DropList' else '' end DropListQuestion
			from
			(
			select a.QuestionId, a.QuestionValue, isnull(s.QuestionGridSettingId,0) QuestionGridSettingId,a.SurveyOrder, a.QuestionTypeId
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
				 b.QuestionGridSettingId, b.IndexNumberColoumn,
				 case when (b.ControlType = 2 or b.ControlType = 1) then 'DropListGrid' else '' end DropListGridQuestion
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
	Fetch Next From CurrQuestionCols Into @SurveyOrder, @QuestionId, @IndexNumber, @QuestionValue, @DropListQuestion, @DropListGridQuestion
	set @Ctr =0
	set @ReturnCols =''
	While @@Fetch_Status = 0 Begin
		if @DropListGridQuestion = 'DropListGrid'
		begin
			if len(@ReturnCols) > 0
				set @ReturnCols+= ' and ' + '('+ @QuestionValue + '<>~N/A~ or ' + @QuestionValue + ' not like 00000 '')'
			else
				set @ReturnCols+= '('+ @QuestionValue + '<>~N/A~ or ' + @QuestionValue + ' not like 00000 '')'
		end
		
		Fetch Next From CurrQuestionCols Into @SurveyOrder, @QuestionId, @IndexNumber, @QuestionValue, @DropListQuestion, @DropListGridQuestion
	End -- End of Fetch	

	set @ReturnCols = replace(replace(replace(@ReturnCols,'''',''),'~',''''),'00000','''0%''')
	Close CurrQuestionCols
return @ReturnCols
end





