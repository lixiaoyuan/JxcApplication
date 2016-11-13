namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WageMap",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WageDate = c.DateTime(nullable: false),
                        SumPrice = c.Decimal(nullable: false, storeType: "money"),
                        PaymentAccountId = c.Guid(nullable: false),
                        Remark = c.String(maxLength: 100, unicode: false),
                        CreateUserId = c.Guid(),
                        CreateDate = c.DateTime(),
                        CreateIp = c.String(maxLength: 20, unicode: false),
                        ModiftyUserId = c.Guid(),
                        ModiftyDate = c.DateTime(),
                        ModiftyIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WageDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WageId = c.Guid(nullable: false),
                        UserId = c.Guid(),
                        Name = c.String(maxLength: 20, unicode: false),
                        WorkDay = c.Decimal(nullable: false, precision: 4, scale: 2),
                        C1 = c.Decimal(storeType: "money"),
                        C2 = c.Decimal(storeType: "money"),
                        C3 = c.Decimal(storeType: "money"),
                        C4 = c.Decimal(storeType: "money"),
                        C5 = c.Decimal(storeType: "money"),
                        C6 = c.Decimal(storeType: "money"),
                        C7 = c.Decimal(storeType: "money"),
                        C8 = c.Decimal(storeType: "money"),
                        C9 = c.Decimal(storeType: "money"),
                        C10 = c.Decimal(storeType: "money"),
                        C11 = c.Decimal(storeType: "money"),
                        C12 = c.Decimal(storeType: "money"),
                        C13 = c.Decimal(storeType: "money"),
                        C14 = c.Decimal(storeType: "money"),
                        C15 = c.Decimal(storeType: "money"),
                        C16 = c.Decimal(storeType: "money"),
                        X1 = c.Decimal(storeType: "money"),
                        X2 = c.Decimal(storeType: "money"),
                        X3 = c.Decimal(storeType: "money"),
                        PreTaxSum = c.Decimal(storeType: "money"),
                        C17 = c.Decimal(storeType: "money"),
                        AfterTaxSum = c.Decimal(storeType: "money"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WageMap", t => t.WageId)
                .Index(t => t.WageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WageDetail", "WageId", "dbo.WageMap");
            DropIndex("dbo.WageDetail", new[] { "WageId" });
            DropTable("dbo.WageDetail");
            DropTable("dbo.WageMap");
        }
    }
}
