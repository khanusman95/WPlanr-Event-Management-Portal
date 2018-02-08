namespace may222017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataannotationupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Places", "PlaceName", c => c.String(nullable: false));
            AlterColumn("dbo.Places", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Places", "Telephone", c => c.String(nullable: false));
            AlterColumn("dbo.Places", "CellNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Places", "EventType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Places", "EventType", c => c.String());
            AlterColumn("dbo.Places", "CellNumber", c => c.String());
            AlterColumn("dbo.Places", "Telephone", c => c.String());
            AlterColumn("dbo.Places", "Address", c => c.String());
            AlterColumn("dbo.Places", "PlaceName", c => c.String());
        }
    }
}
