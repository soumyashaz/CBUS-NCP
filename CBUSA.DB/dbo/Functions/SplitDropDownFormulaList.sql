
CREATE FUNCTION [dbo].[SplitDropDownFormulaList](@InputString AS Varchar(max), @InputChartoSplit as varchar(1))
RETURNS
      @Result TABLE(RowId BigInt, Value Varchar(max))
AS
BEGIN
      DECLARE @str VARCHAR(20)
      DECLARE @ind Int, @RowId Bigint
      IF(@InputString is not null)
      BEGIN
            set @RowId =1
			SET @ind = CharIndex(@InputChartoSplit,@InputString)
            WHILE @ind > 0
            BEGIN
                  SET @str = SUBSTRING(@InputString,1,@ind - 1)
                  SET @InputString = SUBSTRING(@InputString,@ind+1,LEN(@InputString)-@ind)
                  INSERT INTO @Result(RowId, Value) values (@RowId, @str)
                  SET @ind = CharIndex(@InputChartoSplit,@InputString)
				  set @RowId +=1
            END
            SET @str = @InputString
            INSERT INTO @Result values (@RowId, @str)
      END
      RETURN
END








