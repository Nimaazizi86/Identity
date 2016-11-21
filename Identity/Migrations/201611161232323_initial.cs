namespace Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PersonalNumber = c.String(),
                        countryId = c.Int(),
                        cityId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.cityId)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .Index(t => t.countryId)
                .Index(t => t.cityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "countryId", "dbo.Countries");
            DropForeignKey("dbo.People", "cityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropIndex("dbo.People", new[] { "cityId" });
            DropIndex("dbo.People", new[] { "countryId" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropTable("dbo.People");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
        }
    }
}
