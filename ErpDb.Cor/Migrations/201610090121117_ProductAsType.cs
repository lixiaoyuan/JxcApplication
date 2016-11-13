namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductAsType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductAsType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 30, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        Sort = c.Int(nullable: false),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Product", "ProductAsType", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "ProductAsType");
            DropTable("dbo.ProductAsType");
        }
    }
}
