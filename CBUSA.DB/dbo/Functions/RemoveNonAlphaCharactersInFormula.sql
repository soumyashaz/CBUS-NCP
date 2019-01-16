CREATE Function [dbo].[RemoveNonAlphaCharactersInFormula](@InputString VarChar(max))
Returns VarChar(max)
AS
Begin

    Declare @KeepValues as varchar(50)
    Set @KeepValues = '%[^a-z]%'
    While PatIndex(@KeepValues, @InputString) > 0
        Set @InputString = Stuff(@InputString, PatIndex(@KeepValues, @InputString), 1, '')

    Return @InputString
End







