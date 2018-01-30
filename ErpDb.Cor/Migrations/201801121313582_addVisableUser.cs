namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVisableUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "VisableUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "VisableUser");
        }
    }
}
