namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBodyFromTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customer", "Body");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customer", "Body", c => c.String());
        }
    }
}
