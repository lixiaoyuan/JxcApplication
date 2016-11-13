namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExpensesOrderType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "OrderType", c => c.String(maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "OrderType");
        }
    }
}
