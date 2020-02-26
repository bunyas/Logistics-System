namespace mascis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MD5Hash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MD5Hash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MD5Hash");
        }
    }
}
