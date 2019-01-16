CREATE function [dbo].[SplitVolumeFeeFormula](@InputFormula varchar(max))
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
			@TmpVal9 varchar(8000),
			@Chk int,
			@Cntr int,
			@ChkOccurenceToSplit int,
			@tocount varchar(50)
		--@InputFormula varchar(max)
		--set @InputFormula = '([199_828_0=#Overhead Door Series 170 180/WD Series 8000~#]^[199_829_1]*500)+([199_828_0=#Overhead Door Series 370/Wayne-Dalton Series 5145(5140) 9405 (9400)~#]^[199_829_1]*200)' -- '([190_793_0=#Primed~#]^[190_794_1]*10) + (500)'  -- '[204_848_0] * 1000' --
	--set @InputFormula = '([199_828_0=#Overhead Door Series 170 180/WD Series 8000~#]^[199_829_1]*100)+([199_828_0=#Overhead Door Series 511-521 FV 160 300/Wayne-Dalton Series  6600 9700 9800~#]^[199_829_1]*200)+([199_828_0=#Courtyard Series Wood Carriage Doors and Full View Aluminum Doors~#]^[199_829_1]*10000)+ 105'
	--set @InputFormula = '([199_828_0=#Overhead Door Series 170 180/WD Series 8000~#]^[199_829_1]*500)+([199_828_0=#Overhead Door Series 370/Wayne-Dalton Series 5145(5140) 9405 (9400)~#]^[199_829_1]*200)'
	set @InputFormula = replace(@InputFormula,' + ','+')
	set @InputFormula = replace(@InputFormula,' - ','-')
	set @InputFormula = replace(@InputFormula,' * ','*')
	set @InputFormula = replace(@InputFormula,' / ','/')
	set @tocount ='^'
	--select @Chk = CHARINDEX('^','[186_743_0_Merit]^[186_744_1]*20')
	select @Chk = CHARINDEX('^',@InputFormula)
	--select @ChkOccurenceToSplit= (len('[186_743_0_Merit]^[186_744_1]*20') - len(replace('[186_743_0_Merit]^[186_744_1]*20', @tocount,''))) / LEN(@tocount)
	select @ChkOccurenceToSplit= (len(@InputFormula) - len(replace(@InputFormula, @tocount,''))) / LEN(@tocount)
	set @TmpVal7 =''
	if @Chk >0 
	begin
		set @Cntr =0
		set @TmpVal5 =''
		set @TmpVal7 =''
		While @Cntr < @ChkOccurenceToSplit
		Begin
			--print @ChkOccurenceToSplit
			set @Cntr+=1
			--print @Cntr
			--select @TmpVal1 = dbo.GetFormulaColumnValue('[186_743_0_Merit]^[186_744_1]*20','^', 1)
			select @TmpVal1 = dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr)
			--select @TmpVal2 = dbo.GetFormulaColumnValue('[186_743_0_Merit]^[186_744_1]*20','^', 2)
			select @TmpVal2 = dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+1)			
			--select stuff('[199_829_1]*300)+([199_828_0=#Overhead Door Series 170 180/WD Series 8000~#]', charindex('([', '[199_829_1]*300)+([199_828_0=#Overhead Door Series 170 180/WD Series 8000~#]'), len('[199_829_1]*300)+([199_828_0=#Overhead Door Series 170 180/WD Series 8000~#]'), '') rr
			set @TmpVal2 = stuff(@TmpVal2, charindex('([', @TmpVal2), len(@TmpVal2), '') 
			set @TmpVal9 = dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+100)			
			----print '@TmpVal2 ' + @TmpVal2
			--select @TmpVal3 = RTrim(LTrim(dbo.RemoveNonAlphaCharactersInFormula(@TmpVal1)))
			select @TmpVal3 = RTrim(LTrim(substring(@TmpVal1, CHARINDEX('=#',@TmpVal1), CHARINDEX('~#',@TmpVal1))))
			--if(charIndex('=#',@TmpVal1) = 0 )
			--begin
			--	set @TmpVal1 = replace(replace(@TmpVal1,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ')
			--end
			select @Chk = CHARINDEX('=#',@TmpVal2)
			
			--if @Chk >0
			--begin
			--	set @TmpVal6 = RTrim(LTrim(substring(@TmpVal2, CHARINDEX('=#',@TmpVal2), CHARINDEX('~#',@TmpVal2))))
			--	set @TmpVal7 = dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+2)
			--	set @TmpVal5 = 'Case When ' + Replace(@TmpVal2, @TmpVal6,'') + '] = ''' + replace(replace(replace(@TmpVal6,'=#',''),'~#',''),']','') +''' Then (' + @TmpVal7 + ') Else 0 End '
			--	set @TmpVal8 = replace(replace(@TmpVal2, '([', ' (case when ['), @TmpVal6, '] = ''' + replace(replace(replace(@TmpVal6,'=#',''),'~#',''),']','') +''' Then (' + replace(replace(@TmpVal7,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') + ') Else 0 End')
			--	set @TmpVal2 = @TmpVal8
			--	--print 'hii'
			--	set @TmpVal4 = 'Case When ' + Replace(@TmpVal1, @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + @TmpVal2 + ') Else 0 End '
			--end
			--else
			--begin
			--	set @TmpVal2 = replace(replace(@TmpVal2,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ')
			--end			
			if @Chk >0
			begin
				----print ' if >0'
				set @TmpVal8=1
				--set @TmpVal4 = '(Case When ' + Replace(replace(@TmpVal1,'(',''), @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then ' + @TmpVal2 + ')'
				--set @TmpVal4 = replace(@TmpVal4,'+ (case when [', ' Else 0 end) + (case when [')
				--set @TmpVal4 = replace(@TmpVal4,'- (case when [', ' Else 0 end) - (case when [')
				--set @TmpVal4 = replace(@TmpVal4,'* (case when [', ' Else 0 end) * (case when [')
				--set @TmpVal4 = replace(@TmpVal4,'/ (case when [', ' Else 0 end) / (case when [')
				--set @TmpVal4 = replace(@TmpVal4,') Else 0 end)', ' Else 0 end)')
			end
			else
			begin
				--print 'hello'
				----print '@TmpVal1 '+@TmpVal1
				----print '@TmpVal2 '+@TmpVal2
				----print '@TmpVal3 '+@TmpVal3
				--print '@TmpVal5 '+@TmpVal5
				if(@TmpVal5 != '')
				begin
					--print ' tmp5 not null ' +@TmpVal5
					--print ' @TmpVal2 not null ' +@TmpVal2
					--print ' @TmpVal3 not null ' +@TmpVal3
					--print ' @TmpVal9 not null ' +@TmpVal9
					if(@TmpVal2 != @TmpVal3)
					begin
						--print ' hello 12'
						--set @TmpVal2 = replace(replace(@TmpVal2,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') 
						set @TmpVal4 = '(Case When' + Replace(Replace(Replace(@TmpVal1,'([','['), @TmpVal3,''), @TmpVal5,'')  + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + replace(replace(@TmpVal2,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') + ') Else 0 End '
						set @TmpVal6 = right(RTrim(LTrim(stuff(@TmpVal4, charindex(') Else 0 End', @TmpVal4), len(@TmpVal4), ''))), 1) 
						set @TmpVal4 = replace(@TmpVal4, 'Case When ([','Case When [')
						set @TmpVal4 = Replace(@TmpVal4, @TmpVal6 +') Else 0 End', ' Else 0 End)' + @TmpVal6)
						----print ' counter pos is now ' + cast((@ChkOccurenceToSplit) - (@Cntr) as varchar)
						if(@TmpVal3 != @TmpVal9 and (@ChkOccurenceToSplit - @Cntr) =0)
						begin							
							set @TmpVal4+= replace(@TmpVal9, @TmpVal2,'')
						end
						--print ' hello 13 @TmpVal4 === ' +@TmpVal4
					end
					else
					begin
						----print 'last set of data'
						
						if(@TmpVal2 != null)
						begin
							set @TmpVal4 = 'Case When' + Replace(Replace(Replace(@TmpVal1, @TmpVal3,''), @TmpVal5,''), '([','[')   + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + @TmpVal2 + ') Else 0 End '
							set @TmpVal4 = replace(@TmpVal4, 'Case When ([','Case When [')
							set @TmpVal4 = Replace(@TmpVal4, @TmpVal6 +') Else 0 End', ' Else 0 End)' + @TmpVal6)
						end
						else
						begin							
							set @TmpVal2 = dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+2)
							----print '@TmpVal1 '+@TmpVal1
							--print '@TmpVal2 '+@TmpVal2
							----print '@TmpVal3 '+@TmpVal3
							--print 'check here 1'
							--print '@TmpVal6 '+@TmpVal6
							if(charindex(')',@TmpVal2) >0)
							begin
								if(charindex('+',@TmpVal2) >0 or charindex('-',@TmpVal2) >0 or charindex('*',@TmpVal2) >0 or charindex('/',@TmpVal2) >0)
								begin
									set @TmpVal9 = substring(@TmpVal2, charindex(')',@TmpVal2), len(@TmpVal2))
									set @TmpVal9 = replace(@TmpVal9,')','')
									--print ' found last part here is : ' +@TmpVal9
									 -- start new block 22/03/2017
										set @TmpVal4 = '(Case When' + Replace(Replace(Replace(@TmpVal1, @TmpVal3,''), @TmpVal5,''), '([','[')   + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + replace(@TmpVal2, @TmpVal9,'') + ') Else 0 End ' + @TmpVal9
										set @TmpVal4 = replace(@TmpVal4, 'Case When ([','Case When [')
										--print '@TmpVal4 ' + @TmpVal4
										if (@TmpVal6 != null)
										begin								
											--print 'is it null tmpval 6 now ?'
											set @TmpVal4 = Replace(@TmpVal4, @TmpVal6 +') Else 0 End', ' Else 0 End)' + @TmpVal6)
										end
										else
										begin
											--print 'check here 2'
											--print '@TmpVal6 '+@TmpVal6
											set @TmpVal4 = Replace(@TmpVal4, ') Else 0 End', ' Else 0 End)')
										end
									 -- end new block 22/03/2017
									set @TmpVal9 = ''
								end
							end
							else
							begin
								set @TmpVal4 = '(Case When' + Replace(Replace(Replace(@TmpVal1, @TmpVal3,''), @TmpVal5,''), '([','[')   + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + @TmpVal2 + ') Else 0 End '
								set @TmpVal4 = replace(@TmpVal4, 'Case When ([','Case When [')
								--print '@TmpVal4 ' + @TmpVal4
								if (@TmpVal6 != null)
								begin								
									--print 'is it null tmpval 6 now ?'
									set @TmpVal4 = Replace(@TmpVal4, @TmpVal6 +') Else 0 End', ' Else 0 End)' + @TmpVal6)
								end
								else
								begin
									--print 'check here 2'
									--print '@TmpVal6 '+@TmpVal6
									set @TmpVal4 = Replace(@TmpVal4, ') Else 0 End', ' Else 0 End)')
								end
							end
						end
					end
				end
				else
				begin
					----print 'xxx'
					----print '@TmpVal1 ' + @TmpVal1
					----print '@TmpVal2 ' + @TmpVal2
					----print '@TmpVal3 ' + @TmpVal3
					
					if(@TmpVal2 != null)
					begin
						--print ' ram'
						set @TmpVal4 = '(Case When ' + Replace(@TmpVal1, @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + replace(replace(@TmpVal2,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') + ') Else 0 End '
						set @TmpVal6 = right(stuff(@TmpVal4, charindex(') Else 0 End', @TmpVal4), len(@TmpVal4), ''), 1) 
						set @TmpVal4 = replace(@TmpVal4, 'Case When ([','Case When [')
						set @TmpVal4 = Replace(@TmpVal4, @TmpVal6 +') Else 0 End', ' Else 0 End)' + @TmpVal6)	
						set @TmpVal6 = ''
					end
					else
					begin
						----print 'sam'
						if ((@ChkOccurenceToSplit - @Cntr) =0)
						begin
							set @TmpVal2 = dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+1)
							--select #TmpVal6 = right(@TmpVal2,1)
							select @TmpVal6 = isnull(substring(@TmpVal2, 0, charindex(')', @TmpVal2)),'')
							----print '@TmpVal1 -> ' +@TmpVal1
							----print '@TmpVal2 -> ' +@TmpVal2
							----print '@TmpVal3 -> ' +@TmpVal3
							----print '@TmpVal6 -> ' +@TmpVal6
							if(@TmpVal6 != '')
							begin
								select @TmpVal6 = substring(rtrim(ltrim(replace(@TmpVal2, @TmpVal6,''))), 2, len(rtrim(ltrim(replace(@TmpVal2, @TmpVal6,'')))))
								--set @TmpVal4 = ' (Case When ' + Replace(replace(@TmpVal1,'(',''), @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + replace(replace(replace(replace(@TmpVal2,')',''), @TmpVal6,''),'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') + ') Else 0 End) ' + @TmpVal6
								set @TmpVal4 = ' (Case When ' + Replace(replace(@TmpVal1,'(',''), @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + replace(replace(replace(replace(@TmpVal2, @TmpVal6,''),')',''),'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') + ') Else 0 End) ' + @TmpVal6
								set @TmpVal6 = ''
							end
							else
							begin
								set @TmpVal4 = '(Case When ' + Replace(replace(@TmpVal1,'(',''), @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then ' + @TmpVal2 + ' Else 0 end )'
							end
							----print '@TmpVal1 -> ' +@TmpVal1
							----print '@TmpVal2 -> ' +@TmpVal2
							----print '@TmpVal3 -> ' +@TmpVal3
							
							----print 'TmpVal4 66666 ' + @TmpVal2 + '  66 '+@TmpVal6
						end
						else
						begin
							----print ' ram'
							set @TmpVal4 = '(Case When ' + Replace(@TmpVal1, @TmpVal3,'') + '] = ''' + replace(replace(replace(@TmpVal3,'=#',''),'~#',''),']','') +''' Then (' + replace(replace(@TmpVal2,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') + ') Else 0 End '
							set @TmpVal6 = right(stuff(@TmpVal4, charindex(') Else 0 End', @TmpVal4), len(@TmpVal4), ''), 1) 
							set @TmpVal4 = replace(@TmpVal4, 'Case When ([','Case When [')
							set @TmpVal4 = Replace(@TmpVal4, @TmpVal6 +') Else 0 End', ' Else 0 End)' + @TmpVal6)
							set @TmpVal6 = ''
						end
					end
				end
			end
			----print '1 ' + dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr)
			----print '2 ' + dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+1)
			----print '3 ' + dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+2)
			----print '4 ' + dbo.GetFormulaColumnValue(@InputFormula,'^', @Cntr+3)
			----print @TmpVal4
			set @TmpVal7 += @TmpVal4
			set @TmpVal5 = @TmpVal2
		End
	end
	else
		begin
		--select @TmpVal1 = dbo.GetFormulaColumnValue('[186_743_0_Merit]^[186_744_1]*20','_', 2)
		select @TmpVal4 = replace(replace(@InputFormula,'[','Cast(replace(replace(['),']','],''$'',''''),'','','''') as Decimal(18,2)) ') 
		--set @TmpVal3 = @TmpVal1
		set @TmpVal7 += @TmpVal4
	end
	----print ' final ->  select ' + replace(replace(@TmpVal7,'[',' IsNull(['),']','], 0) ') + ' from NcpEvaluteConstructFormula'
	return replace(replace(@TmpVal7,'[',' IsNull(['),']','], 0) ')
end









