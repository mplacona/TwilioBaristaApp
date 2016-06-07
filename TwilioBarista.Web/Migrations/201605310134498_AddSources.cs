namespace TwilioBarista.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSources : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Source",
                c => new
                    {
                        SourceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.SourceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Source");
        }
    }
}
