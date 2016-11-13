namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAcontackName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorage", "AcontackName", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorage", "AcontackName");
        }
    }
}
