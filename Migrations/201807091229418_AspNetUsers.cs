namespace mascis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserTypeId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "FacilityId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "FacilityId");
            DropColumn("dbo.AspNetUsers", "UserTypeId");
        }
    }
}
