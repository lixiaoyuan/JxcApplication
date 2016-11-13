namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 20, unicode: false),
                        Name = c.String(maxLength: 50, unicode: false),
                        Balance = c.Decimal(storeType: "money"),
                        CreateUserId = c.Guid(),
                        CreateDate = c.DateTime(),
                        CreateIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccountRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(),
                        TransactionType = c.Short(),
                        TransactionBefore = c.Decimal(storeType: "money"),
                        TransactionBalance = c.Decimal(storeType: "money"),
                        TransactionAfter = c.Decimal(storeType: "money"),
                        CreateUserId = c.Guid(),
                        CreateDate = c.DateTime(),
                        CreateIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Acontact",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 20, unicode: false),
                        Name = c.String(maxLength: 30, unicode: false),
                        Sex = c.Boolean(),
                        MainAcontact = c.Boolean(),
                        Tel = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 50, unicode: false),
                        Qq = c.String(maxLength: 50, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        Area = c.String(maxLength: 10, fixedLength: true),
                        Address = c.String(maxLength: 300, unicode: false),
                        Enable = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        Name = c.String(maxLength: 200, unicode: false),
                        AcontactId = c.Guid(),
                        Tel = c.String(maxLength: 50, unicode: false),
                        Fax = c.String(maxLength: 30, unicode: false),
                        Area = c.String(maxLength: 150, unicode: false),
                        Address = c.String(maxLength: 500, unicode: false),
                        Credibility = c.Decimal(storeType: "money"),
                        Balance = c.Decimal(storeType: "money"),
                        Enable = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Acontact", t => t.AcontactId)
                .Index(t => t.AcontactId);
            
            CreateTable(
                "dbo.CustomerAccountRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomerId = c.Guid(),
                        TransactionType = c.Short(),
                        TransactionBefore = c.Decimal(storeType: "money"),
                        TransactionBalance = c.Decimal(storeType: "money"),
                        TransactionAfter = c.Decimal(storeType: "money"),
                        CreateUserId = c.Guid(),
                        CreateDate = c.DateTime(),
                        CreateIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 20, unicode: false),
                        Name = c.String(maxLength: 20, unicode: false),
                        AllName = c.String(maxLength: 100, unicode: false),
                        ZipCode = c.String(maxLength: 10, unicode: false),
                        Fax = c.String(maxLength: 20, unicode: false),
                        AContactId = c.Guid(),
                        AContact = c.String(maxLength: 10, unicode: false),
                        AContactPhone = c.String(maxLength: 20, unicode: false),
                        AContactQq = c.String(maxLength: 15, unicode: false),
                        AContactEmail = c.String(maxLength: 50, unicode: false),
                        AContactAddress = c.String(maxLength: 300, unicode: false),
                        Enable = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Acontact", t => t.AContactId)
                .Index(t => t.AContactId);
            
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50, unicode: false),
                        Enable = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Charge",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 20),
                        CustomerId = c.Guid(),
                        PaymentAccountId = c.Guid(),
                        SumPrice = c.Decimal(storeType: "money"),
                        Discount = c.Short(),
                        DiscountPrice = c.Decimal(storeType: "money"),
                        RealPrice = c.Decimal(storeType: "money"),
                        BusinessUser = c.Guid(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        CreateUser = c.Guid(),
                        CreateDate = c.DateTime(),
                        CreateIp = c.String(maxLength: 20),
                        ModiftyDate = c.DateTime(),
                        ModiftyIp = c.String(maxLength: 20),
                        ModiftyUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChargeDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ChargeId = c.Guid(),
                        OrderType = c.String(maxLength: 4, unicode: false),
                        RefId = c.Guid(),
                        RefCode = c.String(maxLength: 20),
                        RefDate = c.DateTime(),
                        SumPrice = c.Decimal(storeType: "money"),
                        Paid = c.Decimal(storeType: "money"),
                        LastPrice = c.Decimal(storeType: "money"),
                        ThisPrice = c.Decimal(storeType: "money"),
                        Remark = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Charge", t => t.ChargeId)
                .Index(t => t.ChargeId);
            
            CreateTable(
                "dbo.GenerateOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(unicode: false),
                        GenerateTime = c.DateTime(storeType: "date"),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 10),
                        Name = c.String(maxLength: 50, unicode: false),
                        ReportUri = c.String(maxLength: 500, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductType = c.String(nullable: false, maxLength: 4, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        Name = c.String(maxLength: 200, unicode: false),
                        PyCode = c.String(maxLength: 200, unicode: false),
                        Specification = c.String(maxLength: 50, unicode: false),
                        Unit = c.String(maxLength: 50, unicode: false),
                        UnitPrice = c.Decimal(storeType: "money"),
                        StockRemind = c.Decimal(precision: 10, scale: 2),
                        Suppliers = c.Guid(),
                        Manufacturer = c.String(maxLength: 200, unicode: false),
                        LifeDay = c.Int(),
                        Enable = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductInStorageDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderType = c.String(nullable: false, maxLength: 4, unicode: false),
                        PutInId = c.Guid(),
                        ReturnInId = c.Guid(),
                        StorageId = c.Guid(),
                        ProductId = c.Guid(),
                        ProductCode = c.String(maxLength: 50, unicode: false),
                        ProductSpecification = c.String(maxLength: 50, unicode: false),
                        ProductUnit = c.String(maxLength: 50, unicode: false),
                        OriginalStock = c.Decimal(nullable: false, precision: 10, scale: 2),
                        LastStock = c.Decimal(nullable: false, precision: 10, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, storeType: "money"),
                        SumPrice = c.Decimal(nullable: false, storeType: "money"),
                        SortCode = c.Short(nullable: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.ProductInStorage", t => t.PutInId)
                .ForeignKey("dbo.ProductReturnInStorage", t => t.ReturnInId)
                .Index(t => t.PutInId)
                .Index(t => t.ReturnInId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductInStorage",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderType = c.String(nullable: false, maxLength: 4, unicode: false),
                        Code = c.String(maxLength: 20, unicode: false),
                        StorageId = c.Guid(),
                        ProducingDate = c.DateTime(),
                        LifeDay = c.Int(),
                        UnitsPurchased = c.String(maxLength: 200, unicode: false),
                        BuyTime = c.DateTime(),
                        WarrantyExpirationTime = c.DateTime(),
                        Acontact = c.Guid(),
                        Suppliers = c.Guid(),
                        DeliveredUser = c.Guid(),
                        Factory = c.String(maxLength: 300, unicode: false),
                        SumPrice = c.Decimal(storeType: "money"),
                        Remark = c.String(maxLength: 300, unicode: false),
                        StatusFlag = c.Int(),
                        CreteUserId = c.Guid(),
                        CreteDate = c.DateTime(),
                        CreteIp = c.String(maxLength: 20, unicode: false),
                        ModiftyUserId = c.Guid(),
                        ModiftyDate = c.DateTime(),
                        ModiftyIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StorageId)
                .Index(t => t.StorageId);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 2, unicode: false),
                        Name = c.String(maxLength: 20, unicode: false),
                        Address = c.String(maxLength: 200, unicode: false),
                        BusinessTypeId = c.Guid(),
                        ReMark = c.String(maxLength: 300, unicode: false),
                        Enable = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductOutInStorageDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PutInStorageId = c.Guid(),
                        PutOutStorageId = c.Guid(),
                        OriginalStock = c.Decimal(precision: 10, scale: 2),
                        SubtractStock = c.Decimal(precision: 10, scale: 2),
                        LasetStock = c.Decimal(precision: 10, scale: 2),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.Guid(),
                        CreateIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductOutStorageDetail", t => t.PutOutStorageId)
                .ForeignKey("dbo.ProductInStorageDetail", t => t.PutInStorageId)
                .Index(t => t.PutInStorageId)
                .Index(t => t.PutOutStorageId);
            
            CreateTable(
                "dbo.ProductOutStorageDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderType = c.String(maxLength: 4, unicode: false),
                        PutOutId = c.Guid(),
                        ProductId = c.Guid(),
                        ProductCode = c.String(maxLength: 50, unicode: false),
                        ProductSpecification = c.String(maxLength: 50, unicode: false),
                        ProductUnit = c.String(maxLength: 50, unicode: false),
                        OutStock = c.Decimal(nullable: false, precision: 10, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, storeType: "money"),
                        SumPrice = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.ProductOutStorage", t => t.PutOutId)
                .Index(t => t.PutOutId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductOutStorage",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderType = c.String(nullable: false, maxLength: 4, unicode: false),
                        Code = c.String(maxLength: 20, unicode: false),
                        StorageId = c.Guid(),
                        PaymentDate = c.DateTime(),
                        BusinessUser = c.Guid(),
                        CustomerId = c.Guid(),
                        AcontackId = c.Guid(),
                        GiveAddress = c.String(maxLength: 300, unicode: false),
                        GiveArea = c.String(maxLength: 20, unicode: false),
                        AcontackTel = c.String(maxLength: 20, unicode: false),
                        SumPrice = c.Decimal(storeType: "money"),
                        Paid = c.Decimal(storeType: "money"),
                        Remark = c.String(maxLength: 200, unicode: false),
                        StatusFlag = c.Int(),
                        CreateUserId = c.Guid(),
                        CreateDate = c.DateTime(),
                        CreateIp = c.String(maxLength: 20, unicode: false),
                        ModiftyUserId = c.Guid(),
                        ModiftyDate = c.DateTime(),
                        ModiftyIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductReturnInStorage",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderType = c.String(maxLength: 4, unicode: false),
                        Code = c.String(maxLength: 20, unicode: false),
                        StorageId = c.Guid(),
                        CustomerId = c.Guid(),
                        BusinessUser = c.Guid(),
                        AcontackId = c.Guid(),
                        PaymentType = c.String(maxLength: 50, unicode: false),
                        PaymentAccountId = c.Guid(),
                        SumPrice = c.Decimal(storeType: "money"),
                        Paid = c.Decimal(storeType: "money"),
                        StatusFlag = c.Int(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        CreateUserId = c.Guid(),
                        CreateDate = c.DateTime(),
                        CreateIp = c.String(maxLength: 20, unicode: false),
                        ModiftyUserId = c.Guid(),
                        ModiftyDate = c.DateTime(),
                        ModiftyIp = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 4, unicode: false),
                        Name = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductInStorageDetail", "ReturnInId", "dbo.ProductReturnInStorage");
            DropForeignKey("dbo.ProductOutInStorageDetail", "PutInStorageId", "dbo.ProductInStorageDetail");
            DropForeignKey("dbo.ProductOutStorageDetail", "PutOutId", "dbo.ProductOutStorage");
            DropForeignKey("dbo.ProductOutInStorageDetail", "PutOutStorageId", "dbo.ProductOutStorageDetail");
            DropForeignKey("dbo.ProductOutStorageDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductInStorage", "StorageId", "dbo.Store");
            DropForeignKey("dbo.ProductInStorageDetail", "PutInId", "dbo.ProductInStorage");
            DropForeignKey("dbo.ProductInStorageDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ChargeDetails", "ChargeId", "dbo.Charge");
            DropForeignKey("dbo.Suppliers", "AContactId", "dbo.Acontact");
            DropForeignKey("dbo.CustomerAccountRecord", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Customer", "AcontactId", "dbo.Acontact");
            DropForeignKey("dbo.AccountRecord", "AccountId", "dbo.Account");
            DropIndex("dbo.ProductOutStorageDetail", new[] { "ProductId" });
            DropIndex("dbo.ProductOutStorageDetail", new[] { "PutOutId" });
            DropIndex("dbo.ProductOutInStorageDetail", new[] { "PutOutStorageId" });
            DropIndex("dbo.ProductOutInStorageDetail", new[] { "PutInStorageId" });
            DropIndex("dbo.ProductInStorage", new[] { "StorageId" });
            DropIndex("dbo.ProductInStorageDetail", new[] { "ProductId" });
            DropIndex("dbo.ProductInStorageDetail", new[] { "ReturnInId" });
            DropIndex("dbo.ProductInStorageDetail", new[] { "PutInId" });
            DropIndex("dbo.ChargeDetails", new[] { "ChargeId" });
            DropIndex("dbo.Suppliers", new[] { "AContactId" });
            DropIndex("dbo.CustomerAccountRecord", new[] { "CustomerId" });
            DropIndex("dbo.Customer", new[] { "AcontactId" });
            DropIndex("dbo.AccountRecord", new[] { "AccountId" });
            DropTable("dbo.ProductType");
            DropTable("dbo.ProductReturnInStorage");
            DropTable("dbo.ProductOutStorage");
            DropTable("dbo.ProductOutStorageDetail");
            DropTable("dbo.ProductOutInStorageDetail");
            DropTable("dbo.Store");
            DropTable("dbo.ProductInStorage");
            DropTable("dbo.ProductInStorageDetail");
            DropTable("dbo.Product");
            DropTable("dbo.OrderType");
            DropTable("dbo.GenerateOrder");
            DropTable("dbo.ChargeDetails");
            DropTable("dbo.Charge");
            DropTable("dbo.Area");
            DropTable("dbo.Suppliers");
            DropTable("dbo.CustomerAccountRecord");
            DropTable("dbo.Customer");
            DropTable("dbo.Acontact");
            DropTable("dbo.AccountRecord");
            DropTable("dbo.Account");
        }
    }
}
