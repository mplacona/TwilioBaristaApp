namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Drinks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drink",
                c => new
                    {
                        DrinkId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DrinkId);
            
            CreateTable(
                "dbo.DrinkType",
                c => new
                    {
                        DrinkTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DrinkId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DrinkTypeId)
                .ForeignKey("dbo.Drink", t => t.DrinkId, cascadeDelete: true)
                .Index(t => t.DrinkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrinkType", "DrinkId", "dbo.Drink");
            DropIndex("dbo.DrinkType", new[] { "DrinkId" });
            DropTable("dbo.DrinkType");
            DropTable("dbo.Drink");
        }
    }
}
