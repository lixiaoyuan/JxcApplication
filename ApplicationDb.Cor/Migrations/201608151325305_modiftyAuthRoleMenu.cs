namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modiftyAuthRoleMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthRoleMenu", "RibbonNodeId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthRoleMenu", "RibbonNodeId");
        }
    }
}
