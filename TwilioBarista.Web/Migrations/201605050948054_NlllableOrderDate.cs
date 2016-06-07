namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NlllableOrderDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order", "Time", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "Time", c => c.DateTime(nullable: false));
        }
    }
}
