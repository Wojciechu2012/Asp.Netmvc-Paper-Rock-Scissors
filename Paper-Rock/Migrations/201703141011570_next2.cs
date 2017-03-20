namespace Paper_Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "InGame", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "InGame");
        }
    }
}
