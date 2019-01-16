
CREATE procedure [dbo].[Proc_CheckConstructFormulaCreation](@SurveyId as varchar(10), @FormulaToCheck as varchar(max))
AS
BEGIN
	DECLARE @QryStr VARCHAR(max)
	Declare @Result varchar(max), @Formula as varchar(max), @ColIndex int
	declare @SurveyIdConv bigint, @GetSplitedFormula varchar(max)
	declare @ColumnValue as Table(CalcValue decimal(18,2))
	if @FormulaToCheck is null or @SurveyId is null
	begin	
		--print ' returned'	
		return
	end
	--print ' returned'
	BEGIN TRY
    BEGIN TRANSACTION;
		select @SurveyIdConv = cast( @SurveyId as bigint)
		exec Proc_NcpConstructFormulaEvalute_Cols @SurveyIdConv
		--print ' returned'
		set @GetSplitedFormula = isnull(dbo.SplitVolumeFeeFormula(replace(replace(replace(@FormulaToCheck,'%','/100'),'[[','['),']]',']')),'')

		if(@GetSplitedFormula = '')
		begin
			--print ' returned 2'
			set @QryStr = ' Select	error + error1 GrossRebate 
							From NcpEvaluteConstructFormula 
							Where 1= 2'
			--print @QryStr
			Insert into @ColumnValue(CalcValue) Exec (@QryStr)
		end
		else
		begin
			--print ' returned 3'
			--set @GetSplitedFormula = replace(replace(@GetSplitedFormula,'[','Cast(['),']','] as Decimal(18,2)) ')
			set @QryStr = ' Select	Max(Cast(Round((IsNull(' + @GetSplitedFormula + ',0)),2) as decimal(18,2))) GrossRebate 
							From NcpEvaluteConstructFormula '
							
			--print @QryStr
			Insert into @ColumnValue(CalcValue) Exec (@QryStr)
		end
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
 
    DECLARE @ErrorNumber INT = ERROR_NUMBER();
    DECLARE @ErrorLine INT = ERROR_LINE();
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();
 
    PRINT 'Actual error number: ' + CAST(@ErrorNumber AS VARCHAR(10));
    PRINT 'Actual line number: ' + CAST(@ErrorLine AS VARCHAR(10));
 
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
  END CATCH
end





