CREATE Procedure [dbo].[Proc_GetNCPVolumeFeeRebateData](@InputQuarterName varchar(2), @InputQuarterYear varchar(4))
AS
BEGIN

	DECLARE @QryStr VARCHAR(max), @QryStoredProc varchar(max)
	Declare @ColumnValue VARCHAR(max), @ContractId BigInt, @NCPSurveyId BigInt
	Declare @Result varchar(max), @Formula as varchar(max), @ColIndex int
	declare @ConstructFormulaId as bigint, @MarketName as varchar(200), @FormulaBuild as varchar(max), @FormulaDescription as varchar(max)
	declare @ContractIdChar varchar(10), @GetSplitedFormula varchar(max), @DynamicQuestionCols varchar(max), @SurveyId varchar(10)
	declare @ContIdCurr varchar(10), @QtrIdCurr varchar(10), @QtrNameCurr varchar(50), @YearCurr varchar(10), @MktIdCurr varchar(10), @MktNameCurr Varchar(3000)
	declare @ContNameCurr varchar(500)
	declare @GridQuestionRowsWhereClause varchar(max), @NCPResponseorFormulaFound varchar(1)
	declare @ExportDateTime varchar(50)

	set @ColIndex = 0
	IF CURSOR_STATUS('global','CurrFormulaRepExpData')>=-1
	BEGIN
		DEALLOCATE CurrFormulaRepExpData
	END
	set @FormulaBuild =''
	SELECT @ExportDateTime = GETDATE()
	declare CurrFormulaRepExpData CURSOR READ_ONLY	
	for
		select distinct b.ContractId, q.QuaterId, b.Quarter, b.Year, b.SurveyId, con.ContractName
		from ConstructFormula b
		inner join ConstructFormulaMarket c on b.ConstructFormulaId = c.ConstructFormulaId
		Inner join Quater q on b.Quarter = q.QuaterName and b.Year = q.Year
		Inner join Contract con on con.ContractId = b.ContractId and con.ContractStatusId = 1
		where b.Quarter = @InputQuarterName
		  and b.Year = @InputQuarterYear
		open CurrFormulaRepExpData
		Fetch Next From CurrFormulaRepExpData Into @ContIdCurr, @QtrIdCurr, @QtrNameCurr, @YearCurr,@SurveyId, @ContNameCurr
		set @ColIndex+= 0
		While @@Fetch_Status = 0 Begin			
			
			exec Proc_GetNCPRebateFormulaValueReportExport @ContNameCurr, @QtrNameCurr, @YearCurr, @ExportDateTime
			
			Fetch Next From CurrFormulaRepExpData Into @ContIdCurr, @QtrIdCurr, @QtrNameCurr, @YearCurr,@SurveyId, @ContNameCurr
		End -- End of Fetch		
	Close CurrFormulaRepExpData
END


