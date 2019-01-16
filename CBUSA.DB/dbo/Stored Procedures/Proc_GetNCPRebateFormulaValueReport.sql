
CREATE procedure [dbo].[Proc_GetNCPRebateFormulaValueReport](@ContractId as varchar(10), @QuarterName as varchar(10), @Year as varchar(10), @MarketId as varchar(10) = null)
AS
BEGIN
	DECLARE @QryStr VARCHAR(max)
	Declare @ColumnValue VARCHAR(max)
	Declare @Result varchar(max), @Formula as varchar(max), @ColIndex int
	declare @ConstructFormulaId as bigint, @MarketName as varchar(200), @FormulaBuild as varchar(max), @FormulaDescription as varchar(max)
	declare @ContractIdChar varchar(10), @GetSplitedFormula varchar(max), @DynamicQuestionCols varchar(max), @SurveyId varchar(10)
	declare @ContIdCurr varchar(10), @QtrIdCurr varchar(10), @QtrNameCurr varchar(50), @YearCurr varchar(10), @MktIdCurr varchar(10), @MktNameCurr Varchar(3000)
	declare @GridQuestionRowsWhereClause varchar(max), @NCPResponseorFormulaFound varchar(1)
	set @ContractIdChar = cast(@ContractId as varchar)
	set @ColIndex = 0
	IF CURSOR_STATUS('global','CurrFormulaRep')>=-1
	BEGIN
		DEALLOCATE CurrFormulaRep
	END
	set @FormulaBuild =''
	declare CurrFormulaRep CURSOR READ_ONLY	
	for
		select distinct b.ContractId, q.QuaterId, b.Quarter, b.Year, c.MarketId, b.SurveyId, isnull(b.FormulaBuild,'No Data') FormulaBuild
		from ConstructFormula b
		inner join ConstructFormulaMarket c on b.ConstructFormulaId = c.ConstructFormulaId
		Inner join Quater q on b.Quarter = q.QuaterName and b.Year = q.Year
		where b.ContractId = @ContractId
			and b.Quarter = @QuarterName
			and b.Year = @Year
			and isnull(@MarketId, c.MarketId) = c.MarketId
		open CurrFormulaRep
		Fetch Next From CurrFormulaRep Into @ContIdCurr, @QtrIdCurr, @QtrNameCurr, @YearCurr, @MktIdCurr, @SurveyId, @FormulaBuild
		set @ColIndex+= 0
		set @NCPResponseorFormulaFound = 'N'
		set @QryStr = ' Select a.* from ('
		While @@Fetch_Status = 0 Begin
			set @ColIndex+= 1
			set @NCPResponseorFormulaFound = 'Y'		
			--select @FormulaBuild = isnull(a.FormulaBuild,'No Data')
			--from ConstructFormula a inner join ConstructFormulaMarket b on a.ConstructFormulaId = b.ConstructFormulaId
			--	 inner join Market m on b.MarketId = m.MarketId
			--where a.ContractId = @ContIdCurr
			--  and a.Quarter = @QtrNameCurr
			--  and a.Year = @YearCurr
			--  and isnull(@MktIdCurr, b.MarketId) = b.MarketId
			if @FormulaBuild <> 'No Data' and len(@FormulaBuild) >0
			begin 
				select @DynamicQuestionCols = dbo.GetVolumeFeeDynamicQuestionHeader(@SurveyId)
				select @GridQuestionRowsWhereClause = dbo.GetVolumeFeeDynamicQuestionWhereClause(@SurveyId,'')
				if @ColIndex > 1
				begin
					set @QryStr+= ' Union All '
				end
				set @QryStr+= ' Select a.MarketName as ''Market '', a.BuilderId as ''BuilderId'', a.BuilderCompany as ''Company'', a.ProjectName as ''Project'', a.ProjectAddress as ''Project Address'', a.QuaterName as Quarter, a.Year '
				if len(ltrim(rtrim(@DynamicQuestionCols))) >0 
				begin 
					set @QryStr+='	,' + @DynamicQuestionCols 
				end
				--print 'dynamic ques cols --> ' + @DynamicQuestionCols
				set @GetSplitedFormula = dbo.SplitVolumeFeeFormula(replace(replace(replace(@FormulaBuild,'%','/100'),'[[','['),']]',']'))
				--set @GetSplitedFormula = replace(replace(@GetSplitedFormula,'[','Cast(['),']','] as Decimal(18,2)) ')
				set @QryStr+= ',	Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) as ''Gross Rebate'' '
				set @QryStr+= '    ,Cast(Round((case when IsNull(br.RebatePercentage,0) >0 then br.RebatePercentage else IsNull(r.RebatePercentage,0) end),2) as decimal(18,2)) as ''Builder Rate '' '
				set @QryStr+= '    ,Cast(Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) * (Cast(Round((case when IsNull(br.RebatePercentage,0) >0 then br.RebatePercentage else IsNull(r.RebatePercentage,0) end),2) as decimal(18,2))/100) as decimal(18,2)) as ''Builder Rebate '' '
				set @QryStr+= '    ,Cast(Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) - (Cast(Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) * (Cast(Round((case when IsNull(br.RebatePercentage,0) >0 then br.RebatePercentage else IsNull(r.RebatePercentage,0) end),2) as decimal(18,2))/100) as decimal(18,2))) as decimal(18,2)) as ''CBUSA Fee '''			
			
				set @QryStr+='	From NcpEvaluteConstructFormula a inner join Quater q on a.QuaterId = cast(q.QuaterId as varchar) and a.Year = q.Year Inner Join Market m on a.MarketName = m.MarketName and m.RowStatusId =1
				 		inner join ContractBuilder cb on cb.ContractId = a.ContractId and cb.BuilderId = a.BuilderId and cb.RowStatusId = 1
						inner join ContractRebate r on r.ContractId = a.ContractId and r.RowStatusId =1 and r.ContractStatusId = cb.ContractStatusId
						left outer join ContractRebateBuilder br on br.ContractId = a.ContractId and br.BuilderId = a.BuilderId and br.RowStatusId =1
						Where a.ContractId = ' + @ContractIdChar + ''
				if(@QuarterName is not null)
				begin 
				set @QryStr+='	and q.QuaterName = ''' + @QuarterName + ''''
				end		
				if(@Year is not null)
				begin 
				set @QryStr+='	and q.Year = ''' + @Year + ''''
				end	
				if(@MktIdCurr is not null)
				begin 
				set @QryStr+='	and m.MarketId = '''+ @MktIdCurr + ''''
				end	
				if(@GridQuestionRowsWhereClause is not null)
				begin
					if (len(@GridQuestionRowsWhereClause) >1)
					begin
						set @QryStr+= ' and ('+ @GridQuestionRowsWhereClause + ' )'
					end
				end 
			end
			
			Fetch Next From CurrFormulaRep Into @ContIdCurr, @QtrIdCurr, @QtrNameCurr, @YearCurr, @MktIdCurr, @SurveyId, @FormulaBuild
		End -- End of Fetch		
	Close CurrFormulaRep
	set @QryStr+= ' ) a'
	set @QryStr+= ' order by 1, 2, 3,4 '
	--print @QryStr
	--select @QryStr
	--Insert into @ColumnValue(CalcValue) Exec (@QryStr)
	if (@NCPResponseorFormulaFound = 'Y')	
		Exec (@QryStr)	
	else
		print 'No Formula set for The Report'
		--set @QryStr = 'No Formula set for The Report'
		--Exec (@QryStr)
	--Select CalcValue
	--From @ColumnValue
END






