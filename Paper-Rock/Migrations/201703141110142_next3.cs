namespace Paper_Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Players", "Points", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Players", "Points", c => c.Int());
        }
    }
}
