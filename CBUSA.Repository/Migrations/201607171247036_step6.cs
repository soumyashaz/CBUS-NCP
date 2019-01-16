namespace CBUSA.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class step6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contract", "ContractStatusId", "dbo.ContractStatus");
            DropForeignKey("dbo.ContractStatusHistory", "ContractStatus_ContractStatusId", "dbo.ContractStatus");
            DropIndex("dbo.Contract", new[] { "ContractStatusId" });
            DropIndex("dbo.ContractStatusHistory", new[] { "ContractStatus_ContractStatusId" });
            DropColumn("dbo.ContractStatusHistory", "ContractStatusId");
            RenameColumn(table: "dbo.ContractStatusHistory", name: "ContractStatus_ContractStatusId", newName: "ContractStatusId");
            DropPrimaryKey("dbo.ContractStatus");
            AddColumn("dbo.Contract", "ContractStatus_ContractStatusId", c => c.Long());
            AlterColumn("dbo.ContractStatus", "ContractStatusId", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ContractStatusHistory", "ContractStatusId", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.ContractStatus", "ContractStatusId");
            CreateIndex("dbo.Contract", "ContractStatus_ContractStatusId");
            CreateIndex("dbo.ContractStatusHistory", "ContractStatusId");
            AddForeignKey("dbo.Contract", "ContractStatus_ContractStatusId", "dbo.ContractStatus", "ContractStatusId");
            AddForeignKey("dbo.ContractStatusHistory", "ContractStatusId", "dbo.ContractStatus", "ContractStatusId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContractStatusHistory", "ContractStatusId", "dbo.ContractStatus");
            DropForeignKey("dbo.Contract", "ContractStatus_ContractStatusId", "dbo.ContractStatus");
            DropIndex("dbo.ContractStatusHistory", new[] { "ContractStatusId" });
            DropIndex("dbo.Contract", new[] { "ContractStatus_ContractStatusId" });
            DropPrimaryKey("dbo.ContractStatus");
            AlterColumn("dbo.ContractStatusHistory", "ContractStatusId", c => c.Int());
            AlterColumn("dbo.ContractStatus", "ContractStatusId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Contract", "ContractStatus_ContractStatusId");
            AddPrimaryKey("dbo.ContractStatus", "ContractStatusId");
            RenameColumn(table: "dbo.ContractStatusHistory", name: "ContractStatusId", newName: "ContractStatus_ContractStatusId");
            AddColumn("dbo.ContractStatusHistory", "ContractStatusId", c => c.Long(nullable: false));
            CreateIndex("dbo.ContractStatusHistory", "ContractStatus_ContractStatusId");
            CreateIndex("dbo.Contract", "ContractStatusId");
            AddForeignKey("dbo.ContractStatusHistory", "ContractStatus_ContractStatusId", "dbo.ContractStatus", "ContractStatusId");
            AddForeignKey("dbo.Contract", "ContractStatusId", "dbo.ContractStatus", "ContractStatusId");
        }
    }
}
