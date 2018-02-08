namespace may222017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class votesUpdatemigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Places", "Votes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Places", "Votes");
        }
    }
}
