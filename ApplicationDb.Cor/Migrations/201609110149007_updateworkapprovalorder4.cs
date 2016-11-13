namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateworkapprovalorder4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkApprovalOrder", "OrderStatusType", c => c.String(maxLength: 30, unicode: false));
            AddColumn("dbo.WorkApprovalOrder", "OrderResultType", c => c.String(maxLength: 30, unicode: false));
            AddColumn("dbo.WorkApprovalOrderUser", "ResultType", c => c.String(maxLength: 30, unicode: false));
            DropColumn("dbo.WorkApprovalOrder", "OrderStatus");
            DropColumn("dbo.WorkApprovalOrder", "OrderResult");
            DropColumn("dbo.WorkApprovalOrderUser", "Result");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkApprovalOrderUser", "Result", c => c.Int(nullable: false));
            AddColumn("dbo.WorkApprovalOrder", "OrderResult", c => c.Int(nullable: false));
            AddColumn("dbo.WorkApprovalOrder", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.WorkApprovalOrderUser", "ResultType");
            DropColumn("dbo.WorkApprovalOrder", "OrderResultType");
            DropColumn("dbo.WorkApprovalOrder", "OrderStatusType");
        }
    }
}
