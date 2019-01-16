namespace CBUSA.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class step41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "BuilderId", c => c.Long(nullable: false));
            CreateIndex("dbo.Project", "StateId");
            CreateIndex("dbo.Project", "CityId");
            CreateIndex("dbo.Project", "BuilderId");
            AddForeignKey("dbo.Project", "BuilderId", "dbo.Builder", "BuilderId");
            AddForeignKey("dbo.Project", "CityId", "dbo.City", "CityId");
            AddForeignKey("dbo.Project", "StateId", "dbo.State", "StateId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Project", "StateId", "dbo.State");
            DropForeignKey("dbo.Project", "CityId", "dbo.City");
            DropForeignKey("dbo.Project", "BuilderId", "dbo.Builder");
            DropIndex("dbo.Project", new[] { "BuilderId" });
            DropIndex("dbo.Project", new[] { "CityId" });
            DropIndex("dbo.Project", new[] { "StateId" });
            DropColumn("dbo.Project", "BuilderId");
        }
    }
}
