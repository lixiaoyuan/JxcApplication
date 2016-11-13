namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModiftyProductType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductType", "IsSell", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductType", "IsSell");
        }
    }
}
