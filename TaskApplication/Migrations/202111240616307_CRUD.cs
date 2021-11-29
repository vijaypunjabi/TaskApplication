namespace TaskApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CRUD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsActive");
            DropColumn("dbo.Categories", "IsActive");
        }
    }
}
