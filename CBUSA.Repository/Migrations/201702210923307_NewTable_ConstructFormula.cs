namespace CBUSA.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTable_ConstructFormula : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConstructFormula",
                c => new
                    {
                        ConstructFormulaId = c.Long(nullable: false, identity: true),
                        ContractId = c.Long(nullable: false),
                        SurveyId = c.Long(nullable: false),
                        Quarter = c.String(nullable: false),
                        Year = c.String(),
                        MarketId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        QuestionColumnId = c.Long(nullable: false),
                        QuestionColumnValueId = c.Long(nullable: false),
                        FormulaDescription = c.String(),
                        FormulaBuild = c.String(),
                        QuestionTypeId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                        RowGUID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ConstructFormulaId)
                .ForeignKey("dbo.Contract", t => t.ContractId)
                .ForeignKey("dbo.Question", t => t.QuestionId)
                .ForeignKey("dbo.RowStatus", t => t.RowStatusId)
                .ForeignKey("dbo.Survey", t => t.SurveyId)
                .Index(t => t.ContractId)
                .Index(t => t.SurveyId)
                .Index(t => t.QuestionId)
                .Index(t => t.RowStatusId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConstructFormula");           
        }
    }
}
