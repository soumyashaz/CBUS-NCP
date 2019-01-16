

create function [dbo].[SplitVolumeFeeFormula_bkp](@InputFormula varchar(max))
	returns varchar(max)
as
begin
	declare @QuestionId bigint,
			@QuestionTypeId int,
			@QuestionGridSettingId bigint,
			@TmpVal1 varchar(8000),
			@TmpVal2 varchar(8000),			
			@TmpVal3 varchar(8000),
			@TmpVal4 varchar(8000),
			@TmpVal5 varchar(8000),
			@TmpVal6 varchar(8000),
			@TmpVal7 varchar(8000),
			@TmpVal8 varchar(8000),
			@Chk int,
			@ChkOccurenceToSplit int,
			@tocount varchar(50)

	set @tocount ='^'
	--select @Chk = CHARINDEX('^','[186_743_0_Merit]^[186_744_1]*20')
	select @Chk = CHARINDEX('^',@InputFormula)
	--select @ChkOccurenceToSplit= (len('[186_743_0_Merit]^[186_744_1]*20') - len(replace('[186_743_0_Merit]^[186_744_1]*20', @tocount,''))) / LEN(@tocount)
	select @ChkOccurenceToSplit= (len(@InputFormula) - len(replace(@InputFormula, @tocount,''))) / LEN(@tocount)
	if @Chk >0 
	begin
		set @Chk =0
		--While @Chk < @ChkOccurenceToSplit
		--Begin
			--select @TmpVal1 = dbo.GetFormulaColumnValue('[186_743_0_Merit]^[186_744_1]*20','^', 1)
			select @TmpVal1 = dbo.GetFormulaColumnValue(@InputFormula,'^', 1)
			--select @TmpVal2 = dbo.GetFormulaColumnValue('[186_743_0_Merit]^[186_744_1]*20','^', 2)
			select @TmpVal2 = dbo.GetFormulaColumnValue(@InputFormula,'^', 2)
			-- select substring('[199_828_0=#Overhead Door Series 190 290 391 399 490/Wayne-Dalton Series 5120 9100 9600 8300 8700~#]', CHARINDEX('=#', '[199_828_0=#Overhead Door Series 190 290 391 399 490/Wayne-Dalton Series 5120 9100 9600 8300 8700~#]'),CHARINDEX('~#', '[199_828_0=#Overhead Door Series 190 290 391 399 490/Wayne-Dalton Series 5120 9100 9600 8300 8700~#]')) b
			
			--select @TmpVal3 = RTrim(LTrim(dbo.RemoveNonAlphaCharactersInFormula(@TmpVal1)))
			select @TmpVal3 = RTrim(LTrim(substring(@TmpVal1, CHARINDEX('=#',@TmpVal1), CHARINDEX('~#',@TmpVal1))))
			if(charIndex('=#',@TmpVal1) = 0 )
			begin
				set @TmpVal1 = replace(replace(@TmpVal1,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ')
			end
			select @Chk = CHARINDEX('=#',@TmpVal2)
			
			if @Chk >0
			begin
				set @TmpVal6 = RTrim(LTrim(substring(@TmpVal2, CHARINDEX('=#',@TmpVal2), CHARINDEX('~#',@TmpVal2))))
				set @TmpVal7 = dbo.GetFormulaColumnValue(@InputFormula,'^', 3)
				set @TmpVal5 = 'Case When ' + Replace(@TmpVal2, @TmpVal6,'') + '] = ''' + replace(replace(replace(@TmpVal6,'=#',''),'~#',''),']','') +''' Then (' + @TmpVal7 + ') Else 0 End '
				set @TmpVal8 = replace(replace(@TmpVal2, '([', ' (case when ['), @TmpVal6, '] = ''' + replace(replace(replace(@TmpVal6,'=#',''),'~#',''),']','') +''' Then (' + replace(replace(@TmpVal7,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') + ') Else 0 End')
				set @TmpVal2 = @TmpVal8
				
			end
			else
			begin
				set @TmpVal2 = replace(replace(@TmpVal2,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ')
			end			
			if @Chk >0
			begin
				set @TmpVal4 = '(Case When ' + Replace(replace(@TmpVal1,'(',''), @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then ' + @TmpVal2 + ')'
				set @TmpVal4 = replace(@TmpVal4,'+ (case when [', ' Else 0 end) + (case when [')
				set @TmpVal4 = replace(@TmpVal4,'- (case when [', ' Else 0 end) - (case when [')
				set @TmpVal4 = replace(@TmpVal4,'* (case when [', ' Else 0 end) * (case when [')
				set @TmpVal4 = replace(@TmpVal4,'/ (case when [', ' Else 0 end) / (case when [')
				set @TmpVal4 = replace(@TmpVal4,') Else 0 end)', ' Else 0 end)')
			end
			else
				set @TmpVal4 = 'Case When ' + Replace(@TmpVal1, @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + @TmpVal2 + ') Else 0 End '
		--End
	end
	else
		begin
		--select @TmpVal1 = dbo.GetFormulaColumnValue('[186_743_0_Merit]^[186_744_1]*20','_', 2)
		select @TmpVal4 = replace(replace(@InputFormula,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') 
		--set @TmpVal3 = @TmpVal1
	end
	return @TmpVal4
end


