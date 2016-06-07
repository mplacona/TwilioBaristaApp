namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        Name = c.String(),
                        fulfilled = c.Boolean(nullable: false),
                        Customer_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.Customer_id)
                .Index(t => t.Customer_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "Customer_id", "dbo.Customer");
            DropIndex("dbo.Order", new[] { "Customer_id" });
            DropTable("dbo.Order");
            DropTable("dbo.Customer");
        }
    }
}
