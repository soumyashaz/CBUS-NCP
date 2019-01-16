namespace CBUSA.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class step0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Long(nullable: false, identity: true),
                        StudentName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
        }
    }
}
