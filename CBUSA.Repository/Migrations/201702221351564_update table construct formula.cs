namespace CBUSA.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableconstructformula : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QBBillDataReceived",
                c => new
                    {
                        TranId = c.Long(nullable: false, identity: true),
                        QBTxnID = c.String(),
                        TimeCreated = c.DateTime(),
                        TimeModified = c.DateTime(),
                        EditSequence = c.String(),
                        TxnNumber = c.Long(nullable: false),
                        VendorRefListID = c.String(),
                        VendorRefFullName = c.String(),
                        APAcountRefListId = c.String(),
                        APAcountRefFullName = c.String(),
                        TxnDate = c.DateTime(),
                        DueDate = c.DateTime(),
                        AmountDue = c.Double(nullable: false),
                        TermsRefListId = c.String(),
                        TermsRefFullName = c.String(),
                        Memo = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        OpenAmount = c.Double(nullable: false),
                        BuilderId = c.Long(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                        RowGUID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TranId)
                .ForeignKey("dbo.Builder", t => t.BuilderId)
                .ForeignKey("dbo.RowStatus", t => t.RowStatusId)
                .Index(t => t.BuilderId)
                .Index(t => t.RowStatusId);
            
            CreateTable(
                "dbo.QBCategoryDataReceived",
                c => new
                    {
                        TranId = c.Long(nullable: false, identity: true),
                        ListID = c.String(nullable: false, maxLength: 1000),
                        TimeCreated = c.DateTime(),
                        TimeModified = c.DateTime(),
                        EditSequence = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        FullName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Sublevel = c.Int(nullable: false),
                        AccountType = c.String(),
                        AccountNumber = c.String(nullable: false, maxLength: 100),
                        Desc = c.String(),
                        Balance = c.Double(nullable: false),
                        TotalBalance = c.Double(nullable: false),
                        TaxLineID = c.Int(nullable: false),
                        TaxLineName = c.String(),
                        CashFlowClassification = c.String(),
                        BuilderId = c.Long(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                        RowGUID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TranId)
                .ForeignKey("dbo.Builder", t => t.BuilderId)
                .ForeignKey("dbo.RowStatus", t => t.RowStatusId)
                .Index(t => t.BuilderId)
                .Index(t => t.RowStatusId);
            
            CreateTable(
                "dbo.QBVendorDataReceived",
                c => new
                    {
                        TranId = c.Long(nullable: false, identity: true),
                        ListID = c.String(nullable: false, maxLength: 1000),
                        TimeCreated = c.DateTime(),
                        TimeModified = c.DateTime(),
                        EditSequence = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        AccountNumber = c.String(),
                        TermsRefListId = c.String(),
                        TermsRefFullName = c.String(),
                        CreditLimit = c.Double(nullable: false),
                        VendorTaxIdent = c.String(),
                        IsVendorEligibleFor1099 = c.Boolean(nullable: false),
                        Balance = c.Double(nullable: false),
                        BuilderId = c.Long(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                        RowGUID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TranId)
                .ForeignKey("dbo.Builder", t => t.BuilderId)
                .ForeignKey("dbo.RowStatus", t => t.RowStatusId)
                .Index(t => t.BuilderId)
                .Index(t => t.RowStatusId);
            
            AddColumn("dbo.ConstructFormula", "Quater_QuaterId", c => c.Long());
            CreateIndex("dbo.ConstructFormula", "MarketId");
            CreateIndex("dbo.ConstructFormula", "Quater_QuaterId");
            AddForeignKey("dbo.ConstructFormula", "MarketId", "dbo.Market", "MarketId");
            AddForeignKey("dbo.ConstructFormula", "Quater_QuaterId", "dbo.Quater", "QuaterId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QBVendorDataReceived", "RowStatusId", "dbo.RowStatus");
            DropForeignKey("dbo.QBVendorDataReceived", "BuilderId", "dbo.Builder");
            DropForeignKey("dbo.QBCategoryDataReceived", "RowStatusId", "dbo.RowStatus");
            DropForeignKey("dbo.QBCategoryDataReceived", "BuilderId", "dbo.Builder");
            DropForeignKey("dbo.QBBillDataReceived", "RowStatusId", "dbo.RowStatus");
            DropForeignKey("dbo.QBBillDataReceived", "BuilderId", "dbo.Builder");
            DropForeignKey("dbo.ConstructFormula", "Quater_QuaterId", "dbo.Quater");
            DropForeignKey("dbo.ConstructFormula", "MarketId", "dbo.Market");
            DropIndex("dbo.QBVendorDataReceived", new[] { "RowStatusId" });
            DropIndex("dbo.QBVendorDataReceived", new[] { "BuilderId" });
            DropIndex("dbo.QBCategoryDataReceived", new[] { "RowStatusId" });
            DropIndex("dbo.QBCategoryDataReceived", new[] { "BuilderId" });
            DropIndex("dbo.QBBillDataReceived", new[] { "RowStatusId" });
            DropIndex("dbo.QBBillDataReceived", new[] { "BuilderId" });
            DropIndex("dbo.ConstructFormula", new[] { "Quater_QuaterId" });
            DropIndex("dbo.ConstructFormula", new[] { "MarketId" });
            DropColumn("dbo.ConstructFormula", "Quater_QuaterId");
            DropTable("dbo.QBVendorDataReceived");
            DropTable("dbo.QBCategoryDataReceived");
            DropTable("dbo.QBBillDataReceived");
        }
    }
}
