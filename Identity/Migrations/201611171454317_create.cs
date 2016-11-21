namespace Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "creater", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "creater");
        }
    }
}
