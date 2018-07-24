namespace EFCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedByPersonId = c.Int(nullable: false),
                        ProcessedByPersonId = c.Int(),
                        Comments = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.CreatedByPersonId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.ProcessedByPersonId)
                .Index(t => t.CreatedByPersonId)
                .Index(t => t.ProcessedByPersonId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        BirthDay = c.DateTime(nullable: false, storeType: "date"),
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ProcessedByPersonId", "dbo.Person");
            DropForeignKey("dbo.Orders", "CreatedByPersonId", "dbo.Person");
            DropIndex("dbo.Orders", new[] { "ProcessedByPersonId" });
            DropIndex("dbo.Orders", new[] { "CreatedByPersonId" });
            DropTable("dbo.Person");
            DropTable("dbo.Orders");
        }
    }
}
