namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsort : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkApprovalOrderUser", "Sort", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkApprovalOrderUser", "Sort");
        }
    }
}
