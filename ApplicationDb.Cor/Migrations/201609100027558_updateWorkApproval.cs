namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateWorkApproval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkApproval", "DataType", c => c.String(maxLength: 300, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkApproval", "DataType");
        }
    }
}
