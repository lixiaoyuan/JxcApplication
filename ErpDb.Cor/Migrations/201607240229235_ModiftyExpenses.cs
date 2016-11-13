namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModiftyExpenses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Code", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.Expenses", "SumMoney", c => c.Decimal(storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "SumMoney");
            DropColumn("dbo.Expenses", "Code");
        }
    }
}
