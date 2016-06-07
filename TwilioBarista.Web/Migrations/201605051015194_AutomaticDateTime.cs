namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutomaticDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order", "Time", c => c.DateTime(defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "Time", c => c.DateTime());
        }
    }
}
