namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwilioMappedCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "Customer_id", "dbo.Customer");
            DropIndex("dbo.Order", new[] { "Customer_id" });
            DropPrimaryKey("dbo.Customer");
            DropPrimaryKey("dbo.Order");
            DropColumn("dbo.Customer", "id");
            DropColumn("dbo.Order", "Id");
            

            RenameColumn(table: "dbo.Order", name: "Customer_id", newName: "CustomerId");
            AlterColumn("dbo.Order", "CustomerId", c => c.Int(nullable: false));

            AddColumn("dbo.Customer", "CustomerId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Customer", "Number");
            AddColumn("dbo.Customer", "From", c => c.String());
            AddColumn("dbo.Customer", "Body", c => c.String());
            AddColumn("dbo.Order", "OrderId", c => c.Int(nullable: false, identity: true));
            
            AddPrimaryKey("dbo.Customer", "CustomerId");
            AddPrimaryKey("dbo.Order", "OrderId");
            CreateIndex("dbo.Order", "CustomerId");
            AddForeignKey("dbo.Order", "CustomerId", "dbo.Customer", "CustomerId", cascadeDelete: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Order", new[] { "CustomerId" });
            DropPrimaryKey("dbo.Order");
            DropPrimaryKey("dbo.Customer");
            DropColumn("dbo.Order", "OrderId");
            DropColumn("dbo.Customer", "Body");
            DropColumn("dbo.Customer", "From");


            AddColumn("dbo.Order", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Customer", "Number", c => c.String());

            DropColumn("dbo.Customer", "CustomerId");

            AddColumn("dbo.Customer", "id", c => c.Int(nullable: false, identity: true));
            
            AlterColumn("dbo.Order", "CustomerId", c => c.Int());
            
            AddPrimaryKey("dbo.Order", "Id");
            AddPrimaryKey("dbo.Customer", "id");
            RenameColumn(table: "dbo.Order", name: "CustomerId", newName: "Customer_id");
            CreateIndex("dbo.Order", "Customer_id");
            AddForeignKey("dbo.Order", "Customer_id", "dbo.Customer", "id");
        }
    }
}
