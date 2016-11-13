namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store", "ProductType", c => c.String(maxLength: 4, unicode: false));
            DropColumn("dbo.Store", "BusinessTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Store", "BusinessTypeId", c => c.Guid());
            DropColumn("dbo.Store", "ProductType");
        }
    }
}
