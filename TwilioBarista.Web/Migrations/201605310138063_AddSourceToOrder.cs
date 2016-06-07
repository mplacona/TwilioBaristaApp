namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "SourceId", c => c.Int(nullable: true));
            CreateIndex("dbo.Order", "SourceId");
            AddForeignKey("dbo.Order", "SourceId", "dbo.Source", "SourceId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "SourceId", "dbo.Source");
            DropIndex("dbo.Order", new[] { "SourceId" });
            DropColumn("dbo.Order", "SourceId");
        }
    }
}
