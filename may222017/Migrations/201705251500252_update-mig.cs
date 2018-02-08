namespace may222017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemig : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Places", "Votes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Places", "Votes", c => c.String());
        }
    }
}
