
CREATE procedure [dbo].[Proc_GetReturnedEvaluatedFormulaValue](@SQLQueryToEvaluteValue as varchar(max), @ValueToEvalute as decimal(18,2) = null)
AS

begin
	--select @ValueToEvalute	
	if(@SQLQueryToEvaluteValue is not null)
	begin 
		Exec  (@SQLQueryToEvaluteValue)
	end	
	else
		select 0
end






