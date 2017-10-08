namespace TelerikMovies.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedLikesAddedImgUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ImgUrl", c => c.String());
            DropColumn("dbo.Movies", "LikesNumber");
            DropColumn("dbo.Movies", "DislikesNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "DislikesNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Movies", "LikesNumber", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "ImgUrl");
        }
    }
}
