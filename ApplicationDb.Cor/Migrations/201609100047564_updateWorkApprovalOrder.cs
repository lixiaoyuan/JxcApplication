namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateWorkApprovalOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkApprovalOrder", "StopTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkApprovalOrder", "StopTime", c => c.DateTime(nullable: false));
        }
    }
}
