namespace Paper_Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cosxx : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rooms", "Player1Id", "dbo.Players");
            AddColumn("dbo.Rooms", "Player_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Rooms", "Player_Id");
            AddForeignKey("dbo.Rooms", "Player_Id", "dbo.Players", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "Player_Id", "dbo.Players");
            DropIndex("dbo.Rooms", new[] { "Player_Id" });
            DropColumn("dbo.Rooms", "Player_Id");
            AddForeignKey("dbo.Rooms", "Player1Id", "dbo.Players", "Id");
        }
    }
}
