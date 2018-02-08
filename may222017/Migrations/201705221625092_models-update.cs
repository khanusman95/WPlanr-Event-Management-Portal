namespace may222017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelsupdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaceName = c.String(),
                        Address = c.String(),
                        Town = c.String(),
                        Telephone = c.String(),
                        Email = c.String(),
                        CellNumber = c.String(),
                        Website = c.String(),
                        EventType = c.String(),
                        SoundSystem = c.Boolean(nullable: false),
                        ColdDrink = c.Boolean(nullable: false),
                        BridalRoom = c.Boolean(nullable: false),
                        AirConditioning = c.Boolean(nullable: false),
                        PartyLight = c.Boolean(nullable: false),
                        Screen = c.Boolean(nullable: false),
                        Accomodation = c.Int(nullable: false),
                        MinPriceRange = c.Int(nullable: false),
                        MaxPriceRange = c.Int(nullable: false),
                        IsCateringAvailable = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ImageLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageData = c.Binary(),
                        PlaceId = c.Int(nullable: false),
                        Places_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.Places_Id)
                .Index(t => t.Places_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Places", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ImageLinks", "Places_Id", "dbo.Places");
            DropIndex("dbo.ImageLinks", new[] { "Places_Id" });
            DropIndex("dbo.Places", new[] { "User_Id" });
            DropTable("dbo.ImageLinks");
            DropTable("dbo.Places");
        }
    }
}
