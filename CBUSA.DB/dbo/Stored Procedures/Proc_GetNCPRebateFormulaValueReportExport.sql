
CREATE procedure [dbo].[Proc_GetNCPRebateFormulaValueReportExport](@InputContractName varchar(500), @InputQuarterName varchar(2), @InputQuarterYear varchar(4), @ExportDateTime varchar(50) = null)
AS
BEGIN

	DECLARE @QryStr VARCHAR(max), @QryStoredProc varchar(max)
	Declare @ColumnValue VARCHAR(max), @ContractId BigInt, @NCPSurveyId BigInt
	Declare @Result varchar(max), @Formula as varchar(max), @ColIndex int
	declare @ConstructFormulaId as bigint, @MarketName as varchar(200), @FormulaBuild as varchar(max), @FormulaDescription as varchar(max)
	declare @ContractIdChar varchar(10), @GetSplitedFormula varchar(max), @DynamicQuestionCols varchar(max), @SurveyId varchar(10)
	declare @ContIdCurr varchar(10), @QtrIdCurr varchar(10), @QtrNameCurr varchar(50), @YearCurr varchar(10), @MktIdCurr varchar(10), @MktNameCurr Varchar(3000)
	declare @GridQuestionRowsWhereClause varchar(max), @NCPResponseorFormulaFound varchar(1)
	declare @MaxTransactionNo BigInt
	--set @ContractIdChar = cast(@ContractId as varchar)
	select @ContractId = IsNull(ContractId,0) from Contract Where ContractName = @InputContractName
	select @MaxTransactionNo = cast(Isnull(max(TransactionId),0) as BigInt) from NCPVolumeFeeExportData
	if(@MaxTransactionNo <=0)
	begin
		set @MaxTransactionNo = 0
	end
	if (IsNull(@ContractId,0) = 0)
	begin
		print 'No Details Found For Input Contract : ' + @InputContractName
		return
	end
	select @NCPSurveyId =  Max(IsNull(SurveyId,0)) from ConstructFormula Where ContractId = @ContractId and Quarter = @InputQuarterName and Year = @InputQuarterYear
	if (IsNull(@ContractId,0) = 0)
	begin
		print 'No NCP Survey Found In Construct Formula For Input Contract : ' + @InputContractName
		return
	end
	exec Proc_NcpConstructFormulaEvalute_Cols @NCPSurveyId
	--set @QryStoredProc = 'exec Proc_NcpConstructFormulaEvalute_Cols ' + @NCPSurveyId
	--exec (@QryStoredProc)	
	if(IsNull(@ExportDateTime,'xx')  = 'xx')
	begin
		set @ExportDateTime = GETDATE()
	end
	set @ColIndex = 0
	IF CURSOR_STATUS('global','CurrFormulaRepExp')>=-1
	BEGIN
		DEALLOCATE CurrFormulaRepExp
	END
	
	set @FormulaBuild =''
	declare CurrFormulaRepExp CURSOR READ_ONLY	
	for
		select distinct b.ContractId, q.QuaterId, b.Quarter, b.Year, c.MarketId, b.SurveyId, isnull(b.FormulaBuild,'No Data') FormulaBuild
		from ConstructFormula b
		inner join ConstructFormulaMarket c on b.ConstructFormulaId = c.ConstructFormulaId
		Inner join Quater q on b.Quarter = q.QuaterName and b.Year = q.Year
		where b.ContractId = @ContractId
			and b.Quarter = @InputQuarterName
			and b.Year = @InputQuarterYear
		--	and isnull(@MarketId, c.MarketId) = c.MarketId
		open CurrFormulaRepExp
		Fetch Next From CurrFormulaRepExp Into @ContIdCurr, @QtrIdCurr, @QtrNameCurr, @YearCurr, @MktIdCurr, @SurveyId, @FormulaBuild
		set @ColIndex+= 0
		set @NCPResponseorFormulaFound = 'N'
		--print '@MaxTransactionNo  : ' + Cast(@MaxTransactionNo as Varchar)
		set @QryStr = ' Select Right(''00000000''  + Cast( ' + cast(@MaxTransactionNo as varchar) + ' + ROW_NUMBER() over(order by a.ContractId) as varchar),8) TransactionNo, Cast(a.ContractId as BigInt) ContractId, a.Year, Cast(Case When a.Quarter = ''Q1'' Then 1 Else Case When a.Quarter = ''Q2'' Then 2 Else Case When a.Quarter = ''Q3'' Then 3 else 4 end end end as Numeric) Quarter, Cast(a.HistoricId as BigInt) BuilderId, Cast(Sum(IsNull(a.BuilderRebate,0)) as Decimal(18,2)) BuilderRebate, Cast(Sum(IsNull(a.CBUSAVolumeFee,0)) as Decimal(18,2)) CBUSAVolumeFee , cast(''' + @ExportDateTime + ''' as datetime) DateTimeStamp
		                from ('
		While @@Fetch_Status = 0 Begin			
			set @ColIndex+= 1
			set @ContractIdChar = cast(@ContIdCurr as varchar)
			set @NCPResponseorFormulaFound = 'Y'
			
			if @FormulaBuild <> 'No Data' and len(@FormulaBuild) >0
			begin 
				select @DynamicQuestionCols = dbo.GetVolumeFeeDynamicQuestionHeader(@SurveyId)
				select @GridQuestionRowsWhereClause = dbo.GetVolumeFeeDynamicQuestionWhereClause(@SurveyId,'')
				if @ColIndex > 1
				begin
					set @QryStr+= ' Union All '
				end
				set @QryStr+= ' Select  a.ContractId, b.HistoricId, a.MarketName as ''Market '', a.BuilderCompany as ''Company'', a.ProjectName as ''Project'', a.ProjectAddress as ''Project Address'', a.QuaterName as Quarter, a.Year '
				if len(ltrim(rtrim(@DynamicQuestionCols))) >0 
				begin 
					set @QryStr+='	,' + @DynamicQuestionCols 
				end
				--print 'dynamic ques cols --> ' + @DynamicQuestionCols
				set @GetSplitedFormula = dbo.SplitVolumeFeeFormula(replace(replace(replace(@FormulaBuild,'%','/100'),'[[','['),']]',']'))
				--set @GetSplitedFormula = replace(replace(@GetSplitedFormula,'[','Cast(['),']','] as Decimal(18,2)) ')
				set @QryStr+= ',	Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) as GrossRebate '
				set @QryStr+= '    ,Cast(Round((case when IsNull(br.RebatePercentage,0) >0 then br.RebatePercentage else IsNull(r.RebatePercentage,0) end),2) as decimal(18,2)) as BuilderRate '
				set @QryStr+= '    ,Cast(Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) * (Cast(Round((case when IsNull(br.RebatePercentage,0) >0 then br.RebatePercentage else IsNull(r.RebatePercentage,0) end),2) as decimal(18,2))/100) as decimal(18,2)) as BuilderRebate '
				set @QryStr+= '    ,Cast(Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) - (Cast(Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2)) * (Cast(Round((case when IsNull(br.RebatePercentage,0) >0 then br.RebatePercentage else IsNull(r.RebatePercentage,0) end),2) as decimal(18,2))/100) as decimal(18,2))) as decimal(18,2)) as CBUSAVolumeFee '			
			
				set @QryStr+='	From NcpEvaluteConstructFormula a inner join Quater q on a.QuaterId = cast(q.QuaterId as varchar) and a.Year = q.Year Inner Join Market m on a.MarketName = m.MarketName and m.RowStatusId =1
				 		inner join ContractBuilder cb on cb.ContractId = a.ContractId and cb.BuilderId = a.BuilderId and cb.RowStatusId = 1
						inner join ContractRebate r on r.ContractId = a.ContractId and r.RowStatusId =1 and r.ContractStatusId = cb.ContractStatusId
						left outer join ContractRebateBuilder br on br.ContractId = a.ContractId and br.BuilderId = a.BuilderId and br.RowStatusId =1
						inner join Builder b on a.BuilderId = b.BuilderId 
						Where a.ContractId = ' + @ContractIdChar + ''
				if(@QtrNameCurr is not null)
				begin 
				set @QryStr+='	and q.QuaterName = ''' + @QtrNameCurr + ''''
				end		
				if(@YearCurr is not null)
				begin 
				set @QryStr+='	and q.Year = ''' + @YearCurr + ''''
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
			Fetch Next From CurrFormulaRepExp Into @ContIdCurr, @QtrIdCurr, @QtrNameCurr, @YearCurr, @MktIdCurr, @SurveyId, @FormulaBuild
		End -- End of Fetch		
	Close CurrFormulaRepExp
	set @QryStr+= ' ) a '
	set @QryStr+= ' group by a.ContractId, a.Year, a.Quarter, a.HistoricId '
	set @QryStr+= ' Having (Sum(IsNull(a.BuilderRebate,0)) + Sum(IsNull(a.CBUSAVolumeFee,0))) >0 '
	set @QryStr+= ' order by 1, 2, 3, 4 '

	--print @QryStr
	--select @QryStr
	--Insert into @ColumnValue(CalcValue) Exec (@QryStr)
	--print ' max tran id ' + cast(@MaxTransactionNo as varchar)
	if (@NCPResponseorFormulaFound = 'Y')
	begin	
		Insert into NCPVolumeFeeExportData
			(
				TransactionId,
				ProgramId,
				Year,
				Quarter,
				BuilderId,
				BuilderRebate,
				CBUSAVolumeFee,
				DataGenerationDateTime
			)	
		Exec (@QryStr)	
	end
	else
	begin
		print 'No Formula set for The Report'
		--set @QryStr = 'No Formula set for The Report'
		--Exec (@QryStr)
	--Select CalcValue
	--From @ColumnValue
	end
	
END



