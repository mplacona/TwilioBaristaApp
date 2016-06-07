namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "To", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "To");
        }
    }
}
