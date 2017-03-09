namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLockAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "LockAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "LockAmount");
        }
    }
}
