namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExpensesAndCostType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        Enable = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PaymentAccountId = c.Guid(),
                        CustomerId = c.Guid(),
                        Acontact = c.Guid(),
                        Bill = c.String(maxLength: 40, unicode: false),
                        Order = c.Int(nullable: false),
                        Attn = c.Guid(),
                        CreteUserId = c.Guid(),
                        CreteDate = c.DateTime(),
                        CreteIp = c.String(maxLength: 20, unicode: false),
                        ModiftyUserId = c.Guid(),
                        ModiftyDate = c.DateTime(),
                        ModiftyIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExpensesDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ExpensesId = c.Guid(nullable: false),
                        CostTypeId = c.Guid(nullable: false),
                        Mark = c.String(maxLength: 300, unicode: false),
                        Cost = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Expenses", t => t.ExpensesId)
                .Index(t => t.ExpensesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpensesDetail", "ExpensesId", "dbo.Expenses");
            DropIndex("dbo.ExpensesDetail", new[] { "ExpensesId" });
            DropTable("dbo.ExpensesDetail");
            DropTable("dbo.Expenses");
            DropTable("dbo.CostType");
        }
    }
}
